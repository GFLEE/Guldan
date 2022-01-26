using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Guldan.Common.DI
{
    /// <summary>
    /// 依赖查找
    /// </summary>
    public class IocService
    {
        public static TService Resolve<TService>()
        {
            if (GuldanIOC.ServiceProvider == null)
            {
                return default;
            }

            return GuldanIOC.ServiceProvider.GetService<TService>();
        }

        public static object Resolve(Type type)
        {
            if (GuldanIOC.ServiceProvider == null)
            {
                return null;
            }

            return GuldanIOC.ServiceProvider.GetService(type);
        }


    }
}
