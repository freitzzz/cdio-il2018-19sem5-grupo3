using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using backend.config;
using System.Security;
using Microsoft.EntityFrameworkCore;
using backend.persistence.ef;

namespace backend
{
    public class Startup
    {
        private IHostingEnvironment env;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(IHostingEnvironment env)
        {
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            /*services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });*/
            DatabaseConfiguration.ConfigureDatabase(Configuration, services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            /*else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();*/

            app.UseMvc();
        }

        /// <summary>
        /// Method that is overriden by integration test classes to use a test database
        /// </summary>
        /// <param name="services">Test database services</param>
        public virtual void setupDatabase(IServiceCollection services)
        {
            services.AddDbContext<MyCContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("MyCContext"),
            sqlOptions => sqlOptions.MigrationsAssembly("backend")));
        }

        /// <summary>
        /// Method that is overriden by integration test classes to use a test database
        /// </summary>
        /// <param name="context">Test database context</param>
        public virtual void ensureDatabaseIsCreated(MyCContext context){
            context.Database.Migrate();
        }
    }
}
