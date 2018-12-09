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
            services.AddScoped<CustomizedProductSerialNumberRepository, EFCustomizedProductSerialNumberRepository>();
            services.AddScoped<CustomizedProductRepository, EFCustomizedProductRepository>();
            services.AddScoped<CustomizedProductCollectionRepository, EFCustomizedProductCollectionRepository>();
            services.AddScoped<CommercialCatalogueRepository, EFCommercialCatalogueRepository>();

            //Due to a bug with the mock test server, the MVC controller assemblies must be specified
            ///<a href="https://stackoverflow.com/questions/43669633/why-is-testserver-not-able-to-find-controllers-when-controller-is-in-separate-as?noredirect=1#comment74386164_43669633"</a>
            services.AddMvc().AddApplicationPart(Assembly.Load(typeof(MaterialsController).Assembly.GetName()));
        }
    }
}