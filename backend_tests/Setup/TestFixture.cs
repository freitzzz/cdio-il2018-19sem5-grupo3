using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using backend.persistence.ef;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace backend_tests.Setup
{
    //!All test classes must inherit from this generic TestFixture class
    /// <summary>
    /// Generic test context class.
    /// Based on this solution <a href = https://logcorner.com/asp-net-web-api-core-integration-testing-using-inmemory-entityframeworkcore-sqlite-or-localdb-and-xunit2/></a>
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public class TestFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {

        protected override IWebHostBuilder CreateWebHostBuilder()
        {

            IWebHostBuilder builder = new WebHostBuilder().UseStartup<TStartup>();

            return builder;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //Create a new service provider
                var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlite()
                .BuildServiceProvider();

                //Add the MyCContext
                var connectionStringBuilder = new SqliteConnectionStringBuilder
                {
                    DataSource = ":memory:"
                };
                var connectionString = connectionStringBuilder.ToString();
                var connection = new SqliteConnection(connectionString);
                services
                  .AddEntityFrameworkSqlite()
                  .AddDbContext<MyCContext>(
                    options =>
                    {
                        options.UseSqlite(connection);
                        options.UseInternalServiceProvider(serviceProvider);
                    }
                  );

                //Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MyCContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<TestFixture<TStartup>>>();

                    // Ensure the database is created.
                    db.Database.OpenConnection();
                    db.Database.EnsureCreated();
                }

            }
            );
        }
    }
}