namespace Hazel.Assessment.Domain
{
    public interface IThrottleStoryIndexer
    {
        int CurrentIndexStoriesCount { get; }
        Task IndexStories(int maxCount, CancellationToken cancellation = default);
    }
}
