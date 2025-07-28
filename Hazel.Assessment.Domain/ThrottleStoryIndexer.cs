namespace Hazel.Assessment.Domain
{
    public class ThrottleStoryIndexer(
            IStoryLoader storyLoader,
            IStoryIndexer storyIndexer
        ) : IThrottleStoryIndexer
    {
        private readonly IStoryLoader _storyLoader = storyLoader;
        private readonly IStoryIndexer _storyIndexer = storyIndexer;
        private readonly Lock _lock = new();

        public int CurrentIndexStoriesCount
        {
            get
            {
                lock (_lock) {
                    return _currentIndexStoriesCount;
                }
            }
        }
        private int _currentIndexStoriesCount = 0;

        public async Task IndexStories(int maxCount = int.MaxValue, CancellationToken cancellation = default)
        {
            if (RequestIndexStoryCountSatisfied(maxCount)) {
                return;
            }
            var ids = await _storyLoader.GetNewStoryIds(cancellation);
            foreach (var id in ids) {
                if (MaxCountReached(maxCount)) {
                    break;
                }
                if (await IndexStory(id, cancellation)) {
                    lock (_lock) {
                        _currentIndexStoriesCount++;
                    }
                }
            }
        }

        private bool RequestIndexStoryCountSatisfied(int maxCount)
        {
            lock (_lock) {
                return _currentIndexStoriesCount >= maxCount;
            }
        }

        private bool MaxCountReached(int maxCount)
        {
            lock (_lock) {
                return _currentIndexStoriesCount >= maxCount;
            }
        }

        private async Task<bool> IndexStory(int id, CancellationToken cancellation)
        {
            var story = await _storyLoader.GetStoryById(id, cancellation);
            if (story is not null) {
                _storyIndexer.Add(story);
                return true;
            }
            return false;
        }
    }
}
