
using Autofac;
using Autofac.Extras.DynamicProxy;
using Guldan.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace Guldan.Register
{
    public class ServiceModule : Module
    {
        private readonly string _assemblyName;
        private readonly string _suffixName;

        public ServiceModule()
        {
            _assemblyName = "Guldan.Service.%";
            _suffixName = "Service";
        }

        protected override void Load(ContainerBuilder builder)
        {
            ////事务拦截
            //var interceptorServiceTypes = new List<Type>();
            //if (_appConfig.Aop.Transaction)
            //{
            //    builder.RegisterType<TransactionInterceptor>();
            //    builder.RegisterType<TransactionAsyncInterceptor>();
            //    interceptorServiceTypes.Add(typeof(TransactionInterceptor));
            //}

            var assbs = AppDomain.CurrentDomain.GetAssemblies().
                   Where(assembly => assembly.GetName().Name.Like(_assemblyName));

            builder.RegisterAssemblyTypes(assbs.ToArray())
           .Where(a => a.Name.EndsWith(_suffixName))
           .AsImplementedInterfaces()
           .InstancePerLifetimeScope()
           .PropertiesAutowired()
           //.InterceptedBy(interceptorServiceTypes.ToArray())
           .EnableInterfaceInterceptors();

        }
    }
}
