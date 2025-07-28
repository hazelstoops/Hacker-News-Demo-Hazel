
using Hazel.Assessment.Domain;

namespace Hazel.Assessment.Web.UI.Server
{
    public class IndexStoryHostedService(
           IThrottleStoryIndexer storyIndexer
        ) : BackgroundService
    {
        private readonly IThrottleStoryIndexer _storyIndexer = storyIndexer;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _storyIndexer.IndexStories(int.MaxValue, stoppingToken);
        }
    }
}
