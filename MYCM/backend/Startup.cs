using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using backend.config;
using System.Security;
using Microsoft.EntityFrameworkCore;
using backend.persistence.ef;
using System;

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {

            /*services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });*/
            DatabaseConfiguration.ConfigureDatabase(Configuration, services);

            services.AddCors();
            services.AddHttpClient("CurrencyConversion", client =>
            {
                client.BaseAddress = new Uri("http://rate-exchange-1.appspot.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "CurrencyConversionAgent");
            });
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
            app.UseCookiePolicy(); */

            //Allow requests from MYC's website
            app.UseCors(builder => 
            builder.WithOrigins("http://localhost:8080")
            .AllowAnyMethod().AllowAnyHeader());

            app.UseMvc();
        }
    }
}
