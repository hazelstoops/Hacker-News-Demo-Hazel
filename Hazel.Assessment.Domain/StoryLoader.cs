using Hazel.Assessment.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hazel.Assessment.Domain
{
    public class StoryLoader(
            ILogger<StoryLoader> logger,
            IOptions<StoryLoaderOptions> options,
            HttpClient httpClient
        ) : IStoryLoader
    {
        private readonly ILogger<StoryLoader> _logger = logger;
        private readonly IOptions<StoryLoaderOptions> _options = options;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IEnumerable<int>> GetNewStoryIds(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching new story IDs ...");
            var url = $"{_options.Value.BaseUrl}/newstories.json";
            var ids = await _httpClient.GetIfSuccess<List<int>>(url, cancellationToken) ?? [];
            _logger.LogInformation("Fetched {Count} new story IDs", ids.Count);
            return ids;
        }

        public async Task<Story?> GetStoryById(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching story with ID {Id} ...", id);
            var url = $"{_options.Value.BaseUrl}/item/{id}.json";
            var story = await _httpClient.GetIfSuccess<Story>(url, cancellationToken);
            if (story is null) {
                _logger.LogWarning("Story with ID {Id} not found", id);
            }
            return story;
        }
    }
}
