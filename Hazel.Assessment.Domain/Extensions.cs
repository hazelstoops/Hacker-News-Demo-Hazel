using Hazel.Assessment.Abstractions;
using System.Text.Json;

namespace Hazel.Assessment.Domain
{
    internal static class Extensions
    {
        public static async Task<T?> GetIfSuccess<T>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            return await GetIfSuccess<T>(httpClient, httpRequestMessage, cancellationToken);
        }

        public static async Task<T?> GetIfSuccess<T>(this HttpClient httpClient, HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.SendAsync(httpRequestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(content)) {
                return default;
            }
            return JsonSerializer.Deserialize<T>(content, _defaultJsonSerializeOptions);
        }

        private static readonly JsonSerializerOptions _defaultJsonSerializeOptions = new() {
            PropertyNameCaseInsensitive = true
        };

        public static async Task<IEnumerable<Story>> GetStories(this IStoryLoader storyLoader, IEnumerable<int> storyIds, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(storyLoader, nameof(storyLoader));
            var stories = new List<Story>(storyIds.Count());
            foreach (var id in storyIds) {
                var story = await storyLoader.GetStoryById(id, cancellation);
                if (story != null) {
                    stories.Add(story);
                }
            }
            return stories;
        }

        public static Pagination<T> ToPagination<T>(this IEnumerable<T> source, int pageNumber, int pageSize, bool useSourceItems = false)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page must be greater than or equal to 1");
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1");
            var items = useSourceItems
                ? source
                : source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new Pagination<T> {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = source.Count()
            };
        }
    }
}
