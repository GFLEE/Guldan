using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Utilities.Collections;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Guldan.Service.FreeSql
{
    public static class IdleBusExtesions
    { /// <summary>
      /// 获得FreeSql实例
      /// </summary>
      /// <param name="ib"></param>
      /// <param name="serviceProvider"></param>
      /// <returns></returns>
        public static IFreeSql GetFreeSql(this IdleBus<IFreeSql> ib, IServiceProvider serviceProvider)
        {
            //var user = serviceProvider.GetRequiredService<IUser>();
            //var appConfig = serviceProvider.GetRequiredService<AppConfig>();

            {
                var freeSql = serviceProvider.GetRequiredService<IFreeSql>();
                return freeSql;
            }
        }
    }
}
