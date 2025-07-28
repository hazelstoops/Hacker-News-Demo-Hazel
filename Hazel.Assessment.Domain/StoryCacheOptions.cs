namespace Hazel.Assessment.Domain
{
    public class StoryCacheOptions
    {
        public TimeSpan NewStoryCacheDuration { get; init; } = TimeSpan.FromMinutes(60);
    }
}
