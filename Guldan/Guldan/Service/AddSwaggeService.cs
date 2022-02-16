using Guldan.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Guldan.Service
{
    public static class SwaggeServic
    {
        public static void AddSwaggeService(this IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {
                typeof(GuldanVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    options.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = "Guldan",
                        Description = "这是一个基于.NET,支持动态WebApi的平台",
                        //TermsOfService = new Uri("None"),
                        Contact = new OpenApiContact
                        {
                            Name = "Ricov",
                            Email = "leegradylll@gmail.com",
                            //Url = new Uri("https://github.com/GFLEE/Guldan")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "许可证名字",
                            // Url = new Uri("None")

                        }


                    });
                });

                options.ResolveConflictingActions(apiDescription => apiDescription.First());
                options.CustomSchemaIds(x => x.FullName);
                options.DocInclusionPredicate((docName, description) => true);

                var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                var xmlFile = System.AppDomain.CurrentDomain.FriendlyName + ".xml";
                var xmlPath = Path.Combine(baseDirectory, "xml", xmlFile);
                options.IncludeXmlComments(xmlPath);


            });

        }
    }


}
