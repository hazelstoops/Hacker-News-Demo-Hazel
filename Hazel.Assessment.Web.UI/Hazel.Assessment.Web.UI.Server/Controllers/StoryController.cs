using Hazel.Assessment.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Hazel.Assessment.Web.UI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController(
            IStoryService storyService
        ) : ControllerBase
    {
        private readonly IStoryService _storyService = storyService;

        [HttpGet]
        public async Task<Pagination<Story>> Get(int pageNumber = 1, int pageSize = 10, CancellationToken cancellation = default)
        {
            var stories = await _storyService.GetNewStories(pageNumber, pageSize, cancellation);
            return stories;
        }

        [HttpGet("search")]
        public async Task<Pagination<Story>> Search(string term, int pageNumber = 1, int pageSize = 10, CancellationToken cancellation = default)
        {
            var stories = await _storyService.SearchNewStories(term, pageNumber, pageSize, cancellation);
            return stories;
        }

        [HttpGet("count")]
        public int Count()
        {
            return _storyService.CurrentIndexStoriesCount;
        }
    }
}
