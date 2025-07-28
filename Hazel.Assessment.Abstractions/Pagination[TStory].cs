using System.Collections.Generic;

namespace Hazel.Assessment.Abstractions
{
    public class Pagination<TStory>
    {
        public IEnumerable<TStory> Items { get; set; } = new List<TStory>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
