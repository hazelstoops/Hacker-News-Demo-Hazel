using Hazel.Assessment.Abstractions;
using Hazel.Assessment.Domain;
using System.Runtime.Caching;

namespace Hazel.Assessment.Web.UI.Server
{
    internal static class HazelServiceBuilderExtensions
    {
        public static IServiceCollection ConfigureAssessmentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StoryLoaderOptions>(configuration.GetSection("StoryService"));
            services.Configure<StoryCacheOptions>(configuration.GetSection("StoryCache"));
            services.AddHttpClient<IStoryLoader, StoryLoader>();
            services.AddSingleton<ObjectCache>(MemoryCache.Default);
            services.Decorate<IStoryLoader, StoryLoaderCache>();
            services.AddSingleton<IStoryIndexer>(LuceneStoryIndexer.Instance);
            services.AddSingleton<IThrottleStoryIndexer, ThrottleStoryIndexer>();
            services.AddScoped<IStoryService, StoryService>();
            if (configuration.GetValue<bool>("IndexStoriesOnStartup")) {
                services.AddHostedService<IndexStoryHostedService>();
            }
            return services;
        }
    }
}
