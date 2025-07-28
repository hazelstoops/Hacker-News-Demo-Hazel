using System.Threading;
using System.Threading.Tasks;

namespace Hazel.Assessment.Abstractions
{
    public interface IStoryService
    {
        int CurrentIndexStoriesCount { get; }
        Task<Pagination<Story>> GetNewStories(int pageNumber = 1, int pageSize = 10, CancellationToken cancellation = default);
        Task<Pagination<Story>> SearchNewStories(string term, int pageNumber = 1, int pageSize = 10, CancellationToken cancellation = default);
    }
}
