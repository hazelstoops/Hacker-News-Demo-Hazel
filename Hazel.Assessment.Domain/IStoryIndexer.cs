using Hazel.Assessment.Abstractions;

namespace Hazel.Assessment.Domain
{
    public interface IStoryIndexer
    {
        void Add(Story story);
        IEnumerable<int> Search(string query);
    }
}
