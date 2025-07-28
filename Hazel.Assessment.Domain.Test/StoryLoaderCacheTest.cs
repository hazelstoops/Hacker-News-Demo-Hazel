using Hazel.Assessment.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Runtime.Caching;

namespace Hazel.Assessment.Domain.Test
{
    public class StoryLoaderCacheTest
    {
        [Fact]
        public async Task GetNewStoryIds_ShouldCallLoader_WhenCacheIsEmpty()
        {
            // Arrange
            var cache = CreateCache();
            // Act
            _ = await cache.GetNewStoryIds();
            // Assert
            await _storyLoader!.Received(1).GetNewStoryIds(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetNewStoryIds_ShouldReturnCachedValue_WhenAvailable()
        {
            // Arrange
            var cache = CreateCache();
            var expectedIds = new List<int> { 1, 2, 3 };
            _cache!.Get(StoryLoaderCache.NewStoryIds).Returns(expectedIds);
            // Act
            var result = await cache.GetNewStoryIds();
            // Assert
            Assert.Equal(expectedIds, result);
            await _storyLoader!.DidNotReceive().GetNewStoryIds(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetNewStoryIds_ShouldCacheValue_WhenLoaderReturnsNewIds()
        {
            // Arrange
            var cache = CreateCache();
            var expectedIds = new List<int> { 1, 2, 3 };
            _storyLoader!.GetNewStoryIds(Arg.Any<CancellationToken>()).Returns(expectedIds);
            // Act
            var result = await cache.GetNewStoryIds();
            // Assert
            Assert.Equal(expectedIds, result);
            _cache!.Received(1).Set(StoryLoaderCache.NewStoryIds, expectedIds, Arg.Any<DateTimeOffset>());
        }

        [Fact]
        public async Task GetStoryById_ShouldCallLoader_OnCacheMiss()
        {
            // Arrange
            var cache = CreateCache();
            var expectedStory = new Story { Id = 1 };
            _cache!.Get(StoryLoaderCache.GetStoryById(expectedStory.Id)).Returns((object?)null);
            _storyLoader!.GetStoryById(expectedStory.Id, Arg.Any<CancellationToken>()).Returns(expectedStory);
            // Act
            var result = await cache.GetStoryById(expectedStory.Id);
            // Assert
            Assert.Equal(expectedStory, result);
            await _storyLoader.Received(1).GetStoryById(expectedStory.Id, Arg.Any<CancellationToken>());
        }

        private StoryLoaderCache CreateCache()
        {
            _storyLoader = Substitute.For<IStoryLoader>();
            _cache = Substitute.For<ObjectCache>();
            return new StoryLoaderCache(_options, _storyLoader, _cache);
        }

        private readonly IOptions<StoryCacheOptions> _options = Options.Create(new StoryCacheOptions());
        private IStoryLoader? _storyLoader;
        private ObjectCache? _cache;
    }
}
