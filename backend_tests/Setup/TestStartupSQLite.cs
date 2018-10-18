using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using backend;
using backend.persistence.ef;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using core.persistence;
using System.Reflection;
using backend.Controllers;

namespace backend_tests.Setup
{
    /// <summary>
    /// Class that starts the SQLite InMemory database that is used by integration test classes
    /// Based on this solution <a href = https://logcorner.com/asp-net-web-api-core-integration-testing-using-inmemory-entityframeworkcore-sqlite-or-localdb-and-xunit2/></a>
    /// </summary>
    public class TestStartupSQLite : Startup
    {
        public TestStartupSQLite(IConfiguration configuration) : base(configuration) { }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ProductRepository, EFProductRepository>();
            services.AddScoped<ProductCategoryRepository, EFProductCategoryRepository>();
            services.AddScoped<MaterialRepository, EFMaterialRepository>();

            services.AddScoped<CommercialCatalogueRepository, EFCommercialCatalogueRepository>();

            services.AddScoped<CustomizedProductRepository, EFCustomizedProductRepository>();
            services.AddScoped<CustomizedProductCollectionRepository, EFCustomizedProductCollectionRepository>();
            services.AddScoped<CommercialCatalogueRepository, EFCommercialCatalogueRepository>();

            services.AddMvc().AddApplicationPart(Assembly.Load(typeof(MaterialsController).Assembly.GetName()));
        }
    }
}