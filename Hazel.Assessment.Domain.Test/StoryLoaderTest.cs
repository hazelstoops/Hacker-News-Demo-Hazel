using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using RichardSzalay.MockHttp;
using System.Net;

namespace Hazel.Assessment.Domain.Test
{
    public class StoryLoaderTest
    {

        [Fact]
        public async Task GetNewStoryIds_ShouldReturnEmptyList_WhenNoNewStories()
        {
            // Arrange
            var url = $"{_options.Value.BaseUrl}/newstories.json";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url)
                    .Respond(HttpStatusCode.OK, "application/json", "[]");
            var loader = CreateLoader(mockHttp.ToHttpClient());

            // Act
            var ids = await loader.GetNewStoryIds(CancellationToken.None);

            // Assert
            Assert.Empty(ids);
        }

        [Fact]
        public async Task GetNewStoryIds_ShouldReturnListOfIds_WhenNewStoriesExist()
        {
            // Arrange
            var url = $"{_options.Value.BaseUrl}/newstories.json";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url)
                    .Respond(HttpStatusCode.OK, "application/json", "[1, 2, 3]");
            var loader = CreateLoader(mockHttp.ToHttpClient());
            // Act
            var ids = await loader.GetNewStoryIds(CancellationToken.None);
            // Assert
            Assert.Equal([1, 2, 3], ids);
        }

        [Fact]
        public async Task GetStoryById_ShouldReturnNull_WhenStoryNotFound()
        {
            // Arrange
            var url = $"{_options.Value.BaseUrl}/item/999.json";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url)
                    .Respond(HttpStatusCode.NoContent);
            var loader = CreateLoader(mockHttp.ToHttpClient());
            // Act
            var story = await loader.GetStoryById(999, CancellationToken.None);
            // Assert
            Assert.Null(story);
        }

        [Fact]
        public async Task GetStoryById_ShouldReturnStory_WhenStoryExists()
        {
            // Arrange
            var url = $"{_options.Value.BaseUrl}/item/1.json";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url)
                    .Respond(HttpStatusCode.OK, "application/json", "{ \"id\": 1, \"title\": \"Test Story\" }");
            var loader = CreateLoader(mockHttp.ToHttpClient());
            // Act
            var story = await loader.GetStoryById(1, CancellationToken.None);
            // Assert
            Assert.NotNull(story);
            Assert.Equal(1, story.Id);
            Assert.Equal("Test Story", story.Title);
        }

        private StoryLoader CreateLoader(HttpClient client)
        {
            return new StoryLoader(_logger, _options, client);
        }

        private readonly IOptions<StoryLoaderOptions> _options = Options.Create(new StoryLoaderOptions { });
        private readonly ILogger<StoryLoader> _logger = Substitute.For<ILogger<StoryLoader>>();
    }
}
