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
using System.Collections.Generic;
using backend.middleware;

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
            DatabaseConfiguration.ConfigureDatabase(Configuration, services);

            services.AddCors(options =>
                options.AddPolicy("Website",
                    builder => builder
                        .WithOrigins(Configuration.GetSection("WEBSITE_URL").Value)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                ));

            services.AddHttpClient("CurrencyConversion", client =>
            {
                client.BaseAddress = new Uri("http://rate-exchange-1.appspot.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "CurrencyConversionAgent");
            });

            services.AddHttpClient("MYCA", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Program.configuration.GetValue<string>("MYCA_ENTRYPOINT"));
                httpClient.DefaultRequestHeaders.Add("Accept", new List<string>(new[] { "application/json", "text/html" }));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSerilogMiddleware();

/*             if (!env.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            } */

            //Allow requests from MYC's website
            app.UseCors("Website");

            app.UseMvc();
        }
    }
}
