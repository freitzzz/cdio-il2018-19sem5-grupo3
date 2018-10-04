using backend.persistence.ef;
using core.persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace backend.config
{
    /// <summary>
    /// Class responsible for reading database configuration from file and loading it accordingly.
    /// </summary>
    public static class DatabaseConfiguration
    {

        /// <summary>
        /// Configures the database connection.
        /// </summary>
        /// <param name="configuration">Application configuration properties.</param>
        /// <param name="services">Collection of service descriptors.</param>
        public static void ConfigureDatabase(IConfiguration configuration, IServiceCollection services)
        {
            //read which database provider to use
            DatabaseProviders provider = configuration.GetValue<DatabaseProviders>("DatabaseProvider");

            switch (provider)
            {
                case DatabaseProviders.MySQL:
                    configureMySQL(configuration, services);
                    break;

                case DatabaseProviders.SQLServer:
                    configureSQLServer(configuration, services);
                    break;

                case DatabaseProviders.SQLite:
                    configureSQLite(configuration, services);
                    break;

                default:
                case DatabaseProviders.InMemory:
                    configureInMemory(configuration, services);
                    break;
            }

            services.AddScoped<ProductRepository, EFProductRepository>();
            services.AddScoped<ProductCategoryRepository, EFProductCategoryRepository>();
            services.AddScoped<MaterialRepository, EFMaterialRepository>();
        }

        /// <summary>
        /// Configures the MySQL database.
        /// </summary>
        /// <param name="configuration">Application configuration properties.</param>
        /// <param name="services">Collection of service descriptors.</param>
        private static void configureMySQL(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<MyCContext>(options =>
                        options.UseLazyLoadingProxies().UseMySql(configuration.GetConnectionString(DatabaseProviders.MySQL.ToString())));
        }

        /// <summary>
        /// Configures the SQL Server database.
        /// </summary>
        /// <param name="configuration">Application configuration properties.</param>
        /// <param name="services">Collection of service descriptors.</param>
        private static void configureSQLServer(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<MyCContext>(options =>
                        options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString(DatabaseProviders.SQLServer.ToString())));
        }

        /// <summary>
        /// Configures the SQLite database.
        /// </summary>
        /// <param name="configuration">Application configuration properties.</param>
        /// <param name="services">Collection of service descriptors.</param>
        private static void configureSQLite(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<MyCContext>(options =>
                        options.UseLazyLoadingProxies().UseSqlite(configuration.GetConnectionString(DatabaseProviders.SQLite.ToString())));
        }

        /// <summary>
        /// Configures the InMemory database.
        /// </summary>
        /// <param name="configuration">Application configuration properties.</param>
        /// <param name="services">Collection of service descriptors.</param>
        private static void configureInMemory(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<MyCContext>(options =>
                        options.UseLazyLoadingProxies().UseInMemoryDatabase(configuration.GetConnectionString(DatabaseProviders.InMemory.ToString())));
        }

    }
}