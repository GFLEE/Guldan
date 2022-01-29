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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Guldan.Cache;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Guldan
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Console.Title = "Guldan";
            Configuration = configuration;
            Env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomServices();
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            #region ������

            services.AddControllers(options =>
            {
                //options.Filters.Add<AdminExceptionFilter>();
                //if (_appConfig.Log.Operation)
                //{
                //    options.Filters.Add<LogActionFilter>();
                //}
                //��ֹȥ��ActionAsync��׺
                options.SuppressAsyncSuffixInActionNames = false;
            }) 
            .AddNewtonsoftJson(options =>
            {
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //ʹ���շ� ����ĸСд
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            })
            .AddControllersAsServices();

            #endregion ������

            services.AddMemoryCache();
            services.AddSingleton<ICache, MemoryCache>();

            services.AddConfigServices(Env).Wait();

            services.AddDbService(Env).Wait();

            var mapper_ass = Assembly.Load("Guldan.Data");
            services.AddAutoMapper(mapper_ass);
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
