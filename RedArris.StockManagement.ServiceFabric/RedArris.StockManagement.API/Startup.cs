using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RedArris.StockManagement.Entity;
using RedArris.StockManagement.Repository.Interfaces;
using RedArris.StockManagement.Repository.Repositories;
using System;

namespace RedArris.StockManagement.API
{
    public class Startup
    {
        private static IConfiguration Configuration;
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("mySettings.json", true, reloadOnChange: true)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();
            Configuration = builder.Build();


        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(
          options => {
              options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
          }); 
            
            services.Configure<ConfigModel>(Configuration.GetSection("ConfigModel"));
            
            //Instead of inbuilt DI. using AutoFac, it allows to keep the type and concrete mapping in config.
            //register repo with Autofac container
            var builder = new Autofac.ContainerBuilder();
            builder.RegisterType<EXStockManagementRepositoy>().As<IStockManagerRepository>();
            //builder.RegisterType<NSEStockMnagementRepository>().As<IStockManagerRepository>();

            builder.Populate(services);
            var container = builder.Build();
           
            return new AutofacServiceProvider(container);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
