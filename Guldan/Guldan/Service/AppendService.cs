using Guldan.QuartzNet.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guldan.Service
{
    public static class AppendService
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddQuartz();
            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });
            services.AddHostedService<QuartzConfiguration>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Guldan", Version = "v1" });
            });
        }

    }
}
