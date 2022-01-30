using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Common
{
    public static class ReflectionHelper
    {

        public static TAttribute CheckIfAttributeUsedRecurse<TAttribute>(TypeInfo info)
            where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute);
            if (info.IsDefined(attributeType, true))
            {
                return info.GetCustomAttributes(attributeType, true).Cast<TAttribute>().First();
            }
            else
            {
                foreach (var implInter in info.ImplementedInterfaces)
                {
                    var res = CheckIfAttributeUsedRecurse<TAttribute>(implInter.GetTypeInfo());

                    if (res != null)
                    {
                        return res;
                    }
                }
            }

            return null;
        }

        public static TAttribute CheckIfAttributeUsed<TAttribute>(MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true)
       where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute);
            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                return memberInfo.GetCustomAttributes(attributeType, inherit).Cast<TAttribute>().First();
            }

            return defaultValue;
        }

         
        public static TAttribute CheckIfContainAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();
            if (attrs.Length > 0)
            {
                return (TAttribute)attrs[0];
            }

            return default(TAttribute);
        }


        public static TAttribute CheckBaseIfContainAttribute<TAttribute>(this Type type, bool inherit = true)
            where TAttribute : Attribute
        {
            var attr = type.GetTypeInfo().CheckIfContainAttribute<TAttribute>();
            if (attr != null)
            {
                return attr;
            }

            if (type.GetTypeInfo().BaseType == null)
            {
                return null;
            }

            return type.GetTypeInfo().BaseType.CheckBaseIfContainAttribute<TAttribute>(inherit);
        }

    }

}
