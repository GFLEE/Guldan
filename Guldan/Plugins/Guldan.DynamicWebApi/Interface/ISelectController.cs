using Guldan.Common;
using Panda.DynamicWebApi.Helpers;
using System;
using System.Reflection;

namespace Guldan.DynamicWebApi
{
    public interface ISelectController
    {
        bool IsController(Type type);
    }

    internal class DefaultSelectController : ISelectController
    {
        public bool IsController(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (!typeof(IDynamicWebApi).IsAssignableFrom(type) ||
                !typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
            {
                return false;
            }


            var attr = ReflectionHelper.CheckIfAttributeUsedRecurse<DynamicWebApiAttribute>(typeInfo);

            if (attr == null)
            {
                return false;
            }

            if (ReflectionHelper.CheckIfAttributeUsedRecurse<NonDynamicWebApiAttribute>(typeInfo) != null)
            {
                return false;
            }

            return true;
        }
    }
}