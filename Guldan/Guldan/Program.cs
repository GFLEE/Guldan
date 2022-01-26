using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldan
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((context, logger) =>
                     {
                         string tmplate = "¡¾{Timestamp:HH:mm:ss} {Level:u3}¡¿ {Message:lj}{NewLine}{Exception}";
                         var path = $"{AppDomain.CurrentDomain}./Logs";
                         logger.ReadFrom.Configuration(context.Configuration);
                         logger.Enrich.FromLogContext()
                         .WriteTo.Console(theme: AnsiConsoleTheme.Code,
                             outputTemplate: tmplate)
                         .WriteTo.Async(a => a.File($"{path}/.txt", rollingInterval: RollingInterval.Day,
                                        fileSizeLimitBytes: 10485760, retainedFileCountLimit: 90,
                                        rollOnFileSizeLimit: true, shared: true, buffered: false, outputTemplate: tmplate));
                     }
                     );
                    webBuilder.UseStartup<Startup>();
                });
    }
}
