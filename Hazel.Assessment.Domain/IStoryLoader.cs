using Hazel.Assessment.Abstractions;

namespace Hazel.Assessment.Domain
{
    public interface IStoryLoader
    {
        Task<IEnumerable<int>> GetNewStoryIds(CancellationToken cancellation = default);
        Task<Story?> GetStoryById(int id, CancellationToken cancellation = default);
    }
}
