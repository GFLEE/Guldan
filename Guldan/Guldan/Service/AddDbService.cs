using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeSql;
using Guldan.Cache;
using Guldan.Common;
using Guldan.Common.Config;
using Guldan.Common.Extension;
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
        public static Task AddDbService(this IServiceCollection services, IHostEnvironment env)
        {
            var dbConfigs = new ConfigHelper().GetConfig<DbConfig>("dbconfig", env.EnvironmentName);
            services.AddSingleton(dbConfigs);

            services.AddScoped<GldWorkManager>();

            var dbConfig = dbConfigs.configs.Where(p => p.isEnable).FirstOrDefault();
            dbConfig.IsObJNull().ThenException("没有找到可用的数据库配置 ！");
            var flbulder = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.type, dbConfig.connectionString)
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

            var timeSpan = dbConfigs.idleTime > 0 ? TimeSpan.FromMinutes(dbConfigs.idleTime) : TimeSpan.MaxValue;
            IdleBus<IFreeSql> ib = new IdleBus<IFreeSql>(timeSpan);

            services.AddSingleton(ib);

            return Task.CompletedTask;
        }

    }
}
