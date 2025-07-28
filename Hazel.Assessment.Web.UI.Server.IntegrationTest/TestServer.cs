using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Hazel.Assessment.Web.UI.Server.IntegrationTest
{
    public class TestServer : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureAppConfiguration((context, config) => {
                config.Sources.Clear();
                var root = Directory.GetCurrentDirectory();
                var fileProvider = new PhysicalFileProvider(root);
                config.AddJsonFile(fileProvider, "TestSettings.json", optional: false, reloadOnChange: true);
            });
        }
    }
}
