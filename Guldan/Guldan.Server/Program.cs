using Autofac.Extensions.DependencyInjection;
using Guldan.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldan.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Guldan.Server";
            Console.WriteLine($" Launching....\r");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog((context, logger) =>
                {

                    string tmplate = "��{Timestamp:HH:mm:ss} {Level:u3}�� {Message:lj}{NewLine}{Exception}";
                    var path = $"{AppDomain.CurrentDomain}./Logs";
                    logger.ReadFrom.Configuration(context.Configuration);
                    logger.Enrich.FromLogContext()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code,
                        outputTemplate: tmplate);
                    if (true)
                    {
                        logger.WriteTo.Async(a => a.File($"{path}/.txt", rollingInterval: RollingInterval.Day,
                        fileSizeLimitBytes: 10485760, retainedFileCountLimit: 90,
                        rollOnFileSizeLimit: true, shared: true, buffered: false, outputTemplate: tmplate));
                    }
                    if (true)
                    {
                        logger.MinimumLevel.Warning().WriteTo.RabbitMQ((clientConfig, sinkConfig) =>
                        {
                            clientConfig.Username = "admin";
                            clientConfig.Password = "admin";
                            clientConfig.Exchange = "GuldanExchange";
                            clientConfig.ExchangeType = "direct";
                            clientConfig.DeliveryMode = RabbitMQDeliveryMode.Durable;
                            clientConfig.RouteKey = "LogRouteKey";
                            clientConfig.Port = 5672;
                            clientConfig.Hostnames.Add("127.0.0.1");

                        });

                    }

                });
    }
}
