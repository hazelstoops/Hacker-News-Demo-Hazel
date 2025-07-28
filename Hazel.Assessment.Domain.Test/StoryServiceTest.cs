using Hazel.Assessment.Abstractions;
using NSubstitute;

namespace Hazel.Assessment.Domain.Test
{
    public class StoryServiceTest
    {
        [Fact]
        public async Task GetNewStories_ShouldReturnPaginatedStories()
        {
            // Arrange
            var storyLoaderMock = Substitute.For<IStoryLoader>();
            var storyIndexerMock = Substitute.For<IStoryIndexer>();
            var throttleStoryIndexerMock = Substitute.For<IThrottleStoryIndexer>();
            var storyService = new StoryService(storyLoaderMock, storyIndexerMock, throttleStoryIndexerMock);
            var cancellationToken = CancellationToken.None;
            var expectedStories = new List<Story>
            {
                new Story { Id = 1, Title = "Story 1" },
                new Story { Id = 2, Title = "Story 2" }
            };
            storyLoaderMock.GetNewStoryIds(Arg.Any<CancellationToken>()).Returns([1, 2]);
            storyLoaderMock.GetStoryById(1, cancellationToken).Returns(expectedStories[0]);
            storyLoaderMock.GetStoryById(2, cancellationToken).Returns(expectedStories[1]);

            // Act
            var result = await storyService.GetNewStories(1, 10, cancellationToken);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task SearchNewStories_ShouldReturnPaginatedStories()
        {
            // Arrange
            var storyLoaderMock = Substitute.For<IStoryLoader>();
            var storyIndexerMock = Substitute.For<IStoryIndexer>();
            var throttleStoryIndexerMock = Substitute.For<IThrottleStoryIndexer>();
            var storyService = new StoryService(storyLoaderMock, storyIndexerMock, throttleStoryIndexerMock);
            var cancellationToken = CancellationToken.None;
            var searchTerm = "test";
            var expectedStories = new List<Story>
            {
                new Story { Id = 1, Title = "Test Story 1" },
                new Story { Id = 2, Title = "Test Story 2" }
            };
            storyIndexerMock.Search(searchTerm).Returns(new List<int> { 1, 2 });
            storyLoaderMock.GetStoryById(1, cancellationToken).Returns(expectedStories[0]);
            storyLoaderMock.GetStoryById(2, cancellationToken).Returns(expectedStories[1]);
            // Act
            var result = await storyService.SearchNewStories(searchTerm, 1, 10, cancellationToken);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Equal(2, result.TotalCount);
        }
    }
}
