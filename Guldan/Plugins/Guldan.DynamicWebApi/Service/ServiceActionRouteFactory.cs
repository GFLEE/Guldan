using Guldan.Common;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Reflection;

namespace Guldan.DynamicWebApi
{
    public class ServiceActionRouteFactory : IActionRouteFactory
    {
        public string CreateActionRouteModel(string areaName, string controllerName, ActionModel action)
        {
            var controllerType = action.ActionMethod.DeclaringType;
            var serviceAttribute = controllerType.GetCustomAttribute<ServiceAttribute>();

            var _controllerName = serviceAttribute.ServiceName == string.Empty ? controllerName.Replace("Service", "") : serviceAttribute.ServiceName.Replace("Service", "");

            var res = $"api/{_controllerName}/{action.ActionName.Replace("Async", "")}";
            return res;
        }
    }
}