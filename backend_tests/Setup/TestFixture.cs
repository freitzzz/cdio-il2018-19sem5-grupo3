using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using backend;

namespace backend_tests.Setup
{
    //!All test classes must inherit from this generic TestFixture class
    /// <summary>
    /// Generic test context class.
    /// Based on this solution <a href = https://logcorner.com/asp-net-web-api-core-integration-testing-using-inmemory-entityframeworkcore-sqlite-or-localdb-and-xunit2/></a>
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly TestServer testServer;
        public HttpClient httpClient { get; }

        public TestFixture()
        {
            var webHostBuilder = new WebHostBuilder()
            .UseConfiguration(new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(@"../../../../backend_tests"))
            .AddJsonFile("appsettings.json", optional: false).Build())
            .UseStartup<Startup>();
            testServer = new TestServer(webHostBuilder);

            httpClient = testServer.CreateClient();
            httpClient.BaseAddress = new Uri("http://localhost:5001");
        }

        public void Dispose()
        {
            httpClient.Dispose();
            testServer.Dispose();
        }
    }
}