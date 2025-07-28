using Hazel.Assessment.Abstractions;

namespace Hazel.Assessment.Domain
{
    public class StoryService(
            IStoryLoader storyLoader,
            IStoryIndexer storyIndexer,
            IThrottleStoryIndexer throttleStoryIndexer
        ) : IStoryService
    {
        private readonly IStoryLoader _storyLoader = storyLoader;
        private readonly IStoryIndexer _storyIndexer = storyIndexer;
        private readonly IThrottleStoryIndexer _throttleStoryIndexer = throttleStoryIndexer;

        public int CurrentIndexStoriesCount => _throttleStoryIndexer.CurrentIndexStoriesCount;

        public async Task<Pagination<Story>> GetNewStories(int pageNumber = 1, int pageSize = 10, CancellationToken cancellation = default)
        {
            var storyIds = await GetNewStoryIds(pageNumber, pageSize, cancellation);
            var stories = await _storyLoader.GetStories(storyIds.Items, cancellation);
            var pagination = stories.ToPagination(pageNumber, pageSize, useSourceItems: true);
            pagination.TotalCount = storyIds.TotalCount;
            return pagination;
        }

        private async Task<Pagination<int>> GetNewStoryIds(int pageNumber, int pageSize, CancellationToken cancellation)
        {
            var ids = await _storyLoader.GetNewStoryIds(cancellation);
            return ids.ToPagination(pageNumber, pageSize);
        }

        public async Task<Pagination<Story>> SearchNewStories(string term, int pageNumber = 1, int pageSize = 10, CancellationToken cancellation = default)
        {
            var ids = _storyIndexer.Search(term);
            var stories = await _storyLoader.GetStories(ids, cancellation);
            var pagination = stories.ToPagination(pageNumber, pageSize);
            pagination.TotalCount = ids.Count();
            return pagination;
        }
    }
}
