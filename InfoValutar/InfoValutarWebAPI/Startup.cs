using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using InfoValutarLoadingLibs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSwag;

namespace InfoValutarWebAPI
{
    class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton< LoadExchangeProviders>(new LoadExchangeProviders("plugins"));
            services.AddApiVersioning();
            services.AddOpenApiDocument(c=> {
                
                
                c.PostProcess = d =>
                {
                    d.Info.Title = "Infovalutar API";
                    d.Info.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    d.Info.Description = "Source code / docs at https://github.com/ignatandrei/InfoValutar/";
                    d.Info.Contact = new OpenApiContact
                    {
                        Name = "Andrei Ignat",
                        Email = string.Empty,
                        Url = "http://msprogrammer.serviciipeweb.ro/category/exchange-rates/"
                    };
                    d.Info.License = new OpenApiLicense()
                    {
                        Name = "Use under MIT",
                        //Url = "https://example.com/license"
                    };
                };
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseOpenApi();
            app.UseSwaggerUi3();

            var sa = app.ServerFeatures.Get<IServerAddressesFeature>();
            var urls =string.Join(",", sa.Addresses.Select(it => it + "/swagger"));
            Console.WriteLine("please use " + urls);
        }
    }
}
