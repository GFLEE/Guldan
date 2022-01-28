using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeSql;
using Guldan.Cache;
using Guldan.Common;
using Guldan.Common.Config;
using Guldan.Service.FreeSql;
using Guldan.Service.FreeSql.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlX.XDevAPI;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Guldan.Service
{
    public static class DbService
    {
        public static Task AddDbAsync(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddScoped<GldWorkManager>();
            var cacheService = services.BuildServiceProvider().GetService<ICache>();
            var dbConfig = cacheService.Get<DbConfig>("DbConfig");
            var flbulder = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            flbulder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
           {
               Console.WriteLine($"{cmd.CommandText}\r\n");
           });


            var fsql = flbulder.Build();
            fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.Is_Deleted == false);

            var user = services.BuildServiceProvider().GetService<IUser>();

            fsql.Aop.AuditValue += (s, e) =>
           {
               DbHelper.AuditValue(e, TimeSpan.FromMilliseconds(0), user);
           };

            fsql.Aop.CurdBefore += (s, e) =>
            {
                Console.WriteLine($"{e.Sql}\r\n");
            };

            services.AddSingleton(fsql);

            return Task.CompletedTask;
        }

    }
}
