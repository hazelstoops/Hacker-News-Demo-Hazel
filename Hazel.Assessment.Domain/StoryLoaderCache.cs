using Hazel.Assessment.Abstractions;
using Microsoft.Extensions.Options;
using System.Runtime.Caching;

namespace Hazel.Assessment.Domain
{
    public class StoryLoaderCache(
            IOptions<StoryCacheOptions> options,
            IStoryLoader storyLoader,
            ObjectCache cache
        ) : IStoryLoader, IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly IOptions<StoryCacheOptions> _options = options;
        private readonly IStoryLoader _storyLoader = storyLoader;
        private readonly ObjectCache _cache = cache;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _semaphore.Dispose();
        }

        public async Task<IEnumerable<int>> GetNewStoryIds(CancellationToken cancellation = default)
        {
            return await Get(NewStoryIds, async () => await _storyLoader.GetNewStoryIds(cancellation), cancellation);
        }

        internal static readonly string NewStoryIds = "NewStoryIds";

        public async Task<Story?> GetStoryById(int id, CancellationToken cancellation = default)
        {
            var cacheKey = GetStoryById(id);
            return await Get(cacheKey, async () => await _storyLoader.GetStoryById(id, cancellation), cancellation);
        }

        internal static string GetStoryById(int id) => $"{id}-Story";

        private async Task<T> Get<T>(string cacheKey, Func<Task<T>> fetchFunc, CancellationToken cancellation)
        {
            if (_cache.Get(cacheKey) is T cachedValue) {
                return cachedValue;
            }
            return await GetFromService(cacheKey, fetchFunc, cancellation);
        }

        private async Task<T> GetFromService<T>(string cacheKey, Func<Task<T>> fetchFunc, CancellationToken cancellation)
        {
            await _semaphore.WaitAsync(cancellation);
            try {
                if (_cache.Get(cacheKey) is T cachedValue) {
                    return cachedValue;
                }
                var value = await fetchFunc();
                _cache.Set(cacheKey, value, DateTimeOffset.Now.Add(_options.Value.NewStoryCacheDuration));
                return value;
            }
            finally {
                _semaphore.Release();
            }
        }
    }
}
