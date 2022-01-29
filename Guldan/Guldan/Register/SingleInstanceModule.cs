
using Autofac;
using Guldan.Common;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace Guldan.Register
{
    public class SingleInstanceModule : Module
    {
        private readonly string _prefixName;

        /// <summary>
        /// 单例注入
        /// </summary>
        /// <param name="prefixName">前缀名</param>
        public SingleInstanceModule(string prefixName = "Guldan.")
        {
            _prefixName = prefixName;
        }

        protected override void Load(ContainerBuilder builder)
        {
             Assembly[] assemblies = DependencyContext.Default.RuntimeLibraries
                .Where(a => a.Name.StartsWith(_prefixName))
                .Select(o => Assembly.Load(new AssemblyName(o.Name))).ToArray();

            //有继承接口
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
            .AsImplementedInterfaces()
            .SingleInstance()
            .PropertiesAutowired();
            
            //无继承接口
            builder.RegisterAssemblyTypes(assemblies)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
            .SingleInstance()
            .PropertiesAutowired();

      
        }
    }
}
