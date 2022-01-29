
using Autofac;
using Guldan.Common.Extension;
using Guldan.Service.FreeSql;
using System;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace Guldan.Register
{
    public class RepositoryModule : Module
    {
        private readonly string _assemblyName;
        private readonly string _suffixName;

        public RepositoryModule()
        {
            _assemblyName = "Guldan.Repository.*";
            _suffixName = "Repository";
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assbs = AppDomain.CurrentDomain.GetAssemblies().
                        Where(assembly => assembly.GetName().Name.Like(_assemblyName));
            foreach (var ass in assbs)
            {
                builder.RegisterAssemblyTypes(ass)
                        .Where(a => a.Name.EndsWith(_suffixName))
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope()
                        .PropertiesAutowired();
            }

            builder.RegisterGeneric(typeof(GldRepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope();
        }
    }
}
