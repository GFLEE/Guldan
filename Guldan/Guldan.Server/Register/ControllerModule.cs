
using Autofac;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace Guldan.Server.Register
{
    public class ControllerModule : Autofac.Module
    {
      
        public ControllerModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
            .ToArray();

             builder.RegisterTypes(controllerTypes).PropertiesAutowired();
        }
    }
}
