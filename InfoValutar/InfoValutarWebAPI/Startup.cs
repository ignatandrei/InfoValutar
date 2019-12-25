using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfovalutarDB;
using InfovalutarLoadAndSave;
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
using Microsoft.Net.Http.Headers;
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
            services.AddSingleton<InMemoryDB>();
            
            services.AddScoped<IRetrieve>(s => new RetrieveSqlServer(s.GetService<InMemoryDB>()));
            services.AddScoped<ISave>(s => new SaveSqlServer(s.GetService<InMemoryDB>()));
            services.AddScoped<LoadAndSaveLastData>();
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
        private static void HandleBank(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var req = context.Request;
                var newUrl = new StringBuilder().Append(req.Scheme + "://").Append(req.Host).Append(req.PathBase).Append("/api/v1.0/rates").Append(req.Path).Append(req.QueryString);
                var p = "/api/v1.0/rates" + context.Request.Path;
                await Task.Delay(10);
                context.Response.Redirect(newUrl.ToString());                
                return;
                
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
            app.UseOpenApi();
            app.UseSwaggerUi3();
            var service = app.ApplicationServices.GetService<LoadExchangeProviders>();
            foreach (var item in service.Banks())
            {
                app.MapWhen(cnt =>
                cnt.Request.Path.Value.StartsWith("/" + item + "/", StringComparison.InvariantCultureIgnoreCase)
                ||
                cnt.Request.Path.Value.EndsWith("." + item, StringComparison.InvariantCultureIgnoreCase)
                , HandleBank);


            }
            //maybe do with https://docs.microsoft.com/en-us/aspnet/core/fundamentals/url-rewriting?view=aspnetcore-3.0
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/")
            //    {
            //        context.Response.Redirect("/swagger");
            //        return;
            //    }
            //    await next.Invoke();
            //});
            
            var sa = app.ServerFeatures.Get<IServerAddressesFeature>();
            var urls =string.Join(",", sa.Addresses.Select(it => it + "/swagger"));
            Console.WriteLine("please use " + urls);

        }
    }
}
