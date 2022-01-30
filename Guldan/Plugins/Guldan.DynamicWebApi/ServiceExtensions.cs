using System;
using System.Collections.Generic;
using System.Reflection;
using Guldan.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
 
namespace Guldan.DynamicWebApi
{
    /// <summary>
    /// Add Dynamic WebApi
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Use Dynamic WebApi to Configure
        /// </summary>
        /// <param name="application"></param> 
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDynamicWebApi(this IApplicationBuilder application, Action<IServiceProvider,Options> optionsAction)
        {
            var options = new Options();

            optionsAction?.Invoke(application.ApplicationServices,options);

            options.Valid();

            DynamicApiConsts.DefaultAreaName = options.DefaultAreaName;
            DynamicApiConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            DynamicApiConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            DynamicApiConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            DynamicApiConsts.ActionPostfixes = options.RemoveActionPostfixes;
            DynamicApiConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            DynamicApiConsts.GetRestFulActionName = options.GetRestFulActionName;
            DynamicApiConsts.AssemblyDynamicWebApiOptions = options.AssemblyDynamicWebApiOptions;

            var partManager = application.ApplicationServices.GetRequiredService<ApplicationPartManager>();

            // Add a custom controller checker
            var featureProviders = application.ApplicationServices.GetRequiredService<ControllerFeatureProvider>();
            partManager.FeatureProviders.Add(featureProviders);

            foreach(var assembly in options.AssemblyDynamicWebApiOptions.Keys)
            {
                var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);

                foreach(var part in partFactory.GetApplicationParts(assembly))
                {
                    partManager.ApplicationParts.Add(part);
                }
            }


            var mvcOptions = application.ApplicationServices.GetRequiredService<IOptions<MvcOptions>>();
            var dynamicWebApiConvention = application.ApplicationServices.GetRequiredService<Convention>();

            mvcOptions.Value.Conventions.Add(dynamicWebApiConvention);

            return application;
        }

        public static IServiceCollection AddDynamicWebApiCore<TSelectController, TActionRouteFactory>(this IServiceCollection services)
            where TSelectController: class,ISelectController
            where TActionRouteFactory: class, IActionRouteFactory
        {
            services.AddSingleton<ISelectController, TSelectController>();
            services.AddSingleton<IActionRouteFactory, TActionRouteFactory>();
            services.AddSingleton<Convention>();
            services.AddSingleton<ControllerFeatureProvider>();
            return services;
        }

        /// <summary>
        /// Add Dynamic WebApi to Container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services, Options options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            options.Valid();

            DynamicApiConsts.DefaultAreaName = options.DefaultAreaName;
            DynamicApiConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            DynamicApiConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            DynamicApiConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            DynamicApiConsts.ActionPostfixes = options.RemoveActionPostfixes;
            DynamicApiConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            DynamicApiConsts.GetRestFulActionName = options.GetRestFulActionName;
            DynamicApiConsts.AssemblyDynamicWebApiOptions = options.AssemblyDynamicWebApiOptions;

            var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddDynamicWebApi\" must be after \"AddMvc\".");
            }

            // Add a custom controller checker
            partManager.FeatureProviders.Add(new ControllerFeatureProvider(options.SelectController));

            services.Configure<MvcOptions>(o =>
            {
                // Register Controller Routing Information Converter
                o.Conventions.Add(new Convention(options.SelectController, options.ActionRouteFactory));
            });

            return services;
        }

        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services)
        {
            return AddDynamicWebApi(services, new Options());
        }

        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services, Action<Options> optionsAction)
        {
            var dynamicWebApiOptions = new Options();

            optionsAction?.Invoke(dynamicWebApiOptions);

            return AddDynamicWebApi(services, dynamicWebApiOptions);
        }

    }
}