using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreeSql;
using Guldan.Common;
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
        /// <summary>
        /// 添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public static Task AddDbAsync(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddScoped<GldWorkManager>();

            var freeSqlBuilder = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString)
                    .UseAutoSyncStructure(false)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
           {
               Console.WriteLine($"{cmd.CommandText}\r\n");
           });


            var fsql = freeSqlBuilder.Build();
            fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDeleted == false);

            //配置实体
            var appConfig = new ConfigHelper().Get<AppConfig>("appconfig", env.EnvironmentName);
            DbHelper.ConfigEntity(fsql, appConfig);


            var user = services.BuildServiceProvider().GetService<IUser>();


            //计算服务器时间
            var serverTime = fsql.Select<DualEntity>().Limit(1).First(a => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            DbHelper.TimeOffset = timeOffset;
            fsql.Aop.AuditValue += (s, e) =>
            {
                DbHelper.AuditValue(e, timeOffset, user);
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
