using System;
using System.Reflection;
using Guldan.Common;

namespace Guldan.DynamicWebApi
{
    public class ServiceLocalSelectController : ISelectController
    {
        public bool IsController(Type type)
        {
            return type.IsPublic && type.GetCustomAttribute<ServiceAttribute>() != null;
        }
    }
}