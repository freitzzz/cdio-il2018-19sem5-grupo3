using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using backend;
using backend.persistence.ef;

namespace backend_tests.Setup
{
    /// <summary>
    /// Class that starts the SQLite InMemory database that is used by integration test classes
    /// Based on this solution <a href = https://logcorner.com/asp-net-web-api-core-integration-testing-using-inmemory-entityframeworkcore-sqlite-or-localdb-and-xunit2/></a>
    /// </summary>
    public class TestStartupSQLite : Startup
    {
        public TestStartupSQLite(IHostingEnvironment env) : base(env)
        {
        }

        public override void setupDatabase(IServiceCollection services)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = ":memory:"
            };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            services
              .AddEntityFrameworkSqlite()
              .AddDbContext<MyCContext>(
                options => options.UseSqlite(connection)
              );
        }

        public override void ensureDatabaseIsCreated(MyCContext context)
        {
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
        }
    }
}