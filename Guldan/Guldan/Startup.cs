using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Guldan.Common.DI;
using Guldan.Common.QuartzNet;
using CrystalQuartz.AspNetCore;
using CrystalQuartz.Core.SchedulerProviders;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Guldan.QuartzNet.Base;
using Guldan.Service;
using Guldan.Common;
using Autofac.Core;
using System.Reflection;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Guldan
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Console.Title = "Guldan";
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCustomServices();
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);
            services.AddControllers();

            var timeSpan = TimeSpan.FromMinutes(30);
            IdleBus<IFreeSql> ib = new IdleBus<IFreeSql>(timeSpan);
            services.AddSingleton(ib);
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterBuildCallback((container) =>
            {
                GuldanIOC.ServiceProvider = new AutofacServiceProvider(container);
            });

            //var assbs = typeof(IInjection).
            builder.RegisterAssemblyTypes(Assembly.Load("Guldan.Service"));
            builder.RegisterAssemblyTypes(Assembly.Load("Guldan.Cache"));

            //   builder.RegisterAssemblyTypes(IServices, Services)
            //.Where(t => t.Name.EndsWith("Service"))
            //.AsImplementedInterfaces();
        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guldan v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCrystalQuartz(() => { return GuldanScheduler.Scheduler; });

            app.UseCrystalQuartz(new RemoteSchedulerProvider()
            {
                SchedulerHost = $"{QuartzModel.ChannelType}://{QuartzModel.Ip}:{QuartzModel.Port}/{QuartzModel.BindName}"
            });

        }
    }
}
