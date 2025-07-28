using Hazel.Assessment.Abstractions;

namespace Hazel.Assessment.Domain.Test
{
    public class LuceneStoryIndexerTest
    {
        [Fact]
        public void Add_ShouldThrowException_WhenStoryIsNull()
        {
            // Arrange
            var indexer = LuceneStoryIndexer.Instance;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => indexer.Add(null!));
        }

        [Fact]
        public void Add_ShouldAddStoryToIndex()
        {
            // Arrange
            var indexer = LuceneStoryIndexer.Instance;
            var story = new Story {
                Id = 1,
                Title = "Test Story",
                Text = "This is a test story."
            };
            // Act
            indexer.Add(story);
            // Assert
            var searchResults = indexer.Search("test");
            Assert.Contains(1, searchResults);
        }

        [Fact]
        public void Add_SearchOnlyReturnsMatchingStories()
        {
            // Arrange
            var indexer = LuceneStoryIndexer.Instance;
            // Act
            indexer.Add(new Story {
                Id = 1,
                Title = "NASA Test Story",
                Text = "This is a test story."
            });
            indexer.Add(new Story {
                Id = 2,
                Title = "Test Story",
                Text = "This is a NASA story."
            });
            indexer.Add(new Story {
                Id = 3,
                Title = "Test Story",
                Text = "This is a test story."
            });
            // Assert
            var searchResults = indexer.Search("nasa");
            Assert.Contains(2, searchResults);
        }

        [Fact]
        public void Search_ShouldReturnEmpty_WhenNoMatches()
        {
            // Arrange
            var indexer = LuceneStoryIndexer.Instance;
            //Lucene.Net.Index.IndexNotFoundException : no segments* file found in RAMDirectory@27b1bbf lockFactory=Lucene.Net.Store.SingleInstanceLockFactory: files: []
            indexer.Add(new Story {
                Id = 1,
                Title = "Existing Story",
                Text = "This is an existing story."
            });
            // Act
            var results = indexer.Search("nonexistent");
            // Assert
            Assert.Empty(results);
        }
    }
}
