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
using Guldan.Server.Register;
using System.IO;
using Guldan.DynamicWebApi;
using Guldan.Service.Sys;
using Guldan.IService.Sys;
using Guldan.Server.Service;

namespace Guldan.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
            HostHelper.host = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomServices();
            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);

            #region 控制器 & Swagger

            services.AddControllers(options =>
            {
                //options.Filters.Add<AdminExceptionFilter>();
                //if (_appConfig.Log.Operation)
                //{
                //    options.Filters.Add<LogActionFilter>();
                //}
                //禁止去除ActionAsync后缀
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddControllersAsServices();

            services.AddDynamicWebApiService();
            services.AddSwaggeService();
            #endregion 控制器

            services.AddMemoryCache();
            services.AddSingleton<ICache, MemoryCache>();

            //services.AddConfigServices(Env).Wait();

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

            try
            {
                builder.RegisterType<SysUserService>().As<ISysUserService>();

                builder.RegisterTypes(typeof(SysUserService).Assembly.GetExportedTypes()
                    .Where(type => typeof(IDynamicWebApi).IsAssignableFrom(type)).ToArray());


                builder.RegisterTypes(Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(type => typeof(IDynamicWebApi).IsAssignableFrom(type)).ToArray());

                builder.RegisterModule(new ControllerModule());
                builder.RegisterModule(new SingleInstanceModule());
                builder.RegisterModule(new RepositoryModule());
                builder.RegisterModule(new ServiceModule());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.InnerException);
            }

            builder.RegisterAssemblyTypes(Assembly.Load("Guldan.Service"));
            builder.RegisterAssemblyTypes(Assembly.Load("Guldan.Cache"));


        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    typeof(GuldanVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Guldan {version}");
                    });
                    c.RoutePrefix = "ApiDoc";//直接根目录访问，如果是IIS发布可以注释该语句，并打开launchSettings.launchUrl
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    c.DefaultModelsExpandDepth(-1);//不显示Models
                });
                app.UseDeveloperExceptionPage();
            }

            app.UseDynamicWebApi((serviceProvider, options) =>
            {
                options.AddAssemblyOptions(typeof(SysUserService).Assembly);
            });

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
