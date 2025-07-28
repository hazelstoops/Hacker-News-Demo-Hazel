using Hazel.Assessment.Abstractions;
using Hazel.Assessment.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Hazel.Assessment.Web.UI.Server.IntegrationTest
{
    [Collection("TestServer")]
    public class StoryControllerTest(
            TestServer testServer
        ) : IAsyncLifetime
    {

        private readonly TestServer _server = testServer;
        private HttpClient? _httpClient;
        private AsyncServiceScope _scope;
        protected IServiceProvider? _serviceProvider;

        public Task InitializeAsync()
        {
            _httpClient = _server.CreateClient(new WebApplicationFactoryClientOptions {
                BaseAddress = new Uri("https://localhost/")
            });
            _scope = _server.Services.CreateAsyncScope();
            _serviceProvider = _scope.ServiceProvider;
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _scope.DisposeAsync();
        }

        [Fact]
        public async Task GetStories_ReturnsOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/story");
            // Act
            var response = await _httpClient!.SendAsync(request);
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetStories_ReturnsCorrectPagination()
        {
            const int pageSize = 10;
            var indexer = _serviceProvider!.GetRequiredService<IThrottleStoryIndexer>();
            await indexer.IndexStories(15, CancellationToken.None);
            var countRequest = new HttpRequestMessage(HttpMethod.Get, "/story/count");
            var storyCount = await _httpClient!.GetIfSuccess<int>(countRequest);
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, $"/story?pageNumber=1&pageSize={pageSize}");
            // Act
            var response = await _httpClient!.GetIfSuccess<Pagination<Story>>(request);
            // Assert
            if (storyCount > 0) {
            }
        }

        [Fact]
        public async Task SearchStories_ReturnsOk()
        {
            // Arrange
            var story = new Story {
                Id = 1,
                Title = "Test Story",
                Text = "This is a test story."
            };
            var indexer = _serviceProvider!.GetRequiredService<IStoryIndexer>();
            indexer.Add(story);
            var request = new HttpRequestMessage(HttpMethod.Get, "/story/search?term=test");
            // Act
            var response = await _httpClient!.SendAsync(request);
            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

    }
}
