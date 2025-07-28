using Hazel.Assessment.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using System.Net;

namespace Hazel.Assessment.Domain.Test
{
    //https://github.com/richardszalay/mockhttp
    public class ExtentionsTest
    {
        [Fact]
        public async Task GetIfSuccess_ShouldThrowException_OnNotSuccess()
        {
            const string url = "http://localhost/story";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url)
                    .Respond(HttpStatusCode.NotFound);
            var client = mockHttp.ToHttpClient();

            await Assert.ThrowsAsync<HttpRequestException>(async () => _ = await client.GetIfSuccess<Story>(url, CancellationToken.None));
        }

        [Fact]
        public async Task GetIfSuccess_ShouldReturnNull_OnEmptyResponse()
        {
            const string url = "http://localhost/story";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url)
                    .Respond(HttpStatusCode.OK, "application/json", string.Empty);
            var client = mockHttp.ToHttpClient();
            var result = await client.GetIfSuccess<Story>(url, CancellationToken.None);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetIfSuccess_ShouldReturnObject_ForCaseInsenstiveJson()
        {
            const string url = "http://localhost/story";
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(url)
                    .Respond(HttpStatusCode.OK, "application/json", "{ \"iD\": 1, \"TITLE\": \"title\" }");
            var client = mockHttp.ToHttpClient();
            var result = await client.GetIfSuccess<Story>(url, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("title", result.Title);
        }

        [Fact]
        public async Task GetStories_ShouldThrowException_IfStoryLoaderIsNull()
        {
            IStoryLoader? loader = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => loader!.GetStories([], CancellationToken.None));
        }

        [Fact]
        public async Task GetStories_ShouldReturnEmptyList_IfNoStoryIds()
        {
            var mockLoader = Substitute.For<IStoryLoader>();
            var stories = await mockLoader.GetStories([], CancellationToken.None);
            Assert.Empty(stories);
        }

        [Fact]
        public async Task GetStories_ShouldReturnStories_ForValidStoryIds()
        {
            var mockLoader = Substitute.For<IStoryLoader>();
            var story1 = new Story { Id = 1, Title = "Story 1" };
            var story2 = new Story { Id = 2, Title = "Story 2" };
            mockLoader.GetStoryById(1, Arg.Any<CancellationToken>()).Returns(story1);
            mockLoader.GetStoryById(2, Arg.Any<CancellationToken>()).Returns(story2);
            var stories = await mockLoader.GetStories([1, 2], CancellationToken.None);
            Assert.NotNull(stories);
            Assert.Equal(2, stories.Count());
            Assert.Contains(stories, s => s.Id == 1 && s.Title == "Story 1");
            Assert.Contains(stories, s => s.Id == 2 && s.Title == "Story 2");
        }

        //TODO: Add more tests for ToPagination
        [Fact]
        public void ToPagination_ShouldThrowException_OnInvalidPageNumber()
        {
            var source = new List<int> { };
            Assert.Throws<ArgumentOutOfRangeException>(() => source.ToPagination(0, 10));
        }

        [Fact]
        public void ToPagination_ShouldThrowException_OnInvalidPageSize()
        {
            var source = new List<int> { };
            Assert.Throws<ArgumentOutOfRangeException>(() => source.ToPagination(1, 0));
        }

        [Fact]
        public void ToPagination_ShouldThrowException_OnNullSource()
        {
            List<int>? source = null;
            Assert.Throws<ArgumentNullException>(() => source!.ToPagination(1, 10));
        }

        [Fact]
        public void ToPagination_ShouldReturnSourceItems_WhenUseSourceAsItemsIsTrue()
        {
            var source = new List<int> { };
            var pagination = source.ToPagination(1, 10);
            Assert.NotNull(pagination);
            Assert.Equal(pagination.Items, source);
        }

        [Fact]
        public void ToPagination_ShouldSkipLowerPageItems_WhenUseSourceAsItemsIsFalse()
        {
            var source = new List<int> { 1, 2, 3, 4 };
            var pagination = source.ToPagination(2, 2);
            Assert.NotNull(pagination);
            Assert.Equal(3, pagination.Items.First());
            Assert.Equal(4, pagination.Items.Last());
        }

        [Fact]
        public void ToPagination_ReturnCorrectTotalCount()
        {
            var source = new List<int> { 1, 2, 3, 4 };
            var pagination = source.ToPagination(2, 2);
            Assert.NotNull(pagination);
            Assert.Equal(4, pagination.TotalCount);
        }
    }
}
