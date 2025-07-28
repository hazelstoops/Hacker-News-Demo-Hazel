using Hazel.Assessment.Abstractions;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace Hazel.Assessment.Domain
{
    public class LuceneStoryIndexer : IStoryIndexer, IDisposable
    {
        private readonly RAMDirectory _directory = new();
        private readonly IndexWriter _indexWriter;

        private LuceneStoryIndexer()
        {
            _indexWriter = CreateWriter();
        }

        public static readonly LuceneStoryIndexer Instance = new();

        private IndexWriter CreateWriter()
        {
            var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer) {
                OpenMode = OpenMode.CREATE
            };
            return new IndexWriter(_directory, config);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _directory.Dispose();
            _indexWriter.Dispose();
        }

        public void Add(Story story)
        {
            ArgumentNullException.ThrowIfNull(story, nameof(story));
            var doc = CreateDocument(story);
            _indexWriter.AddDocument(doc);
            _indexWriter.Flush(triggerMerge: false, applyAllDeletes: false);
            _indexWriter.Commit();
        }

        private static Document CreateDocument(Story story)
        {
            var doc = new Document {
                new Int32Field(nameof(Story.Id), story.Id, Field.Store.YES),
                new StringField(nameof(Story.Title), (story.Title ?? string.Empty).ToLowerInvariant(), Field.Store.YES),
                new StringField(nameof(Story.Text), (story.Text ?? string.Empty).ToLowerInvariant(), Field.Store.YES)
            };
            return doc;
        }

        public IEnumerable<int> Search(string term)
        {
            var query = CreateQuery(term);
            using var indexReader = DirectoryReader.Open(_directory);
            var searcher = new IndexSearcher(indexReader);
            var topDocs = searcher.Search(query, _maxTopDocCount);
            return MapDocIds(searcher, topDocs);
        }
        private const int _maxTopDocCount = 1000;

        private static BooleanQuery CreateQuery(string term)
        {
            var wildCardTerm = $"*{term.ToLowerInvariant()}*";
            var titleTerm = new WildcardQuery(new Term(nameof(Story.Title), wildCardTerm));
            var textTerm = new WildcardQuery(new Term(nameof(Story.Text), wildCardTerm));
            return new BooleanQuery{
                { titleTerm, Occur.SHOULD },
                { textTerm, Occur.SHOULD }
            };
        }

        private static List<int> MapDocIds(IndexSearcher searcher, TopDocs topDocs)
        {
            var ids = new List<int>(topDocs.TotalHits);
            for (var i = 0; i < topDocs.ScoreDocs.Length; i++) {
                var doc = searcher.Doc(topDocs.ScoreDocs[i].Doc);
                var docId = doc.GetField(nameof(Story.Id)).GetInt32Value();
                if (!docId.HasValue) continue;
                ids.Add(docId.Value);
            }
            return ids;
        }
    }
}
