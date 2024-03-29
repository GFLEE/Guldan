﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FreeSql;
using Guldan.Cache;
using Guldan.Common;
using Guldan.Common.Config;
using Guldan.Common.DI;
using Guldan.Service.FreeSql;
using Guldan.Service.FreeSql.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlX.XDevAPI;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Guldan.Server.Service
{
    public static class ConfigService
    {
        public static Task AddConfigServices(this IServiceCollection services, IHostEnvironment env)
        {
            //Dictionary<string, Type> mapping = new Dictionary<string, Type>
            //{
            //    { "dbconfig", typeof(DbConfig) }
            //};
            ////var cacheService = IocService.Resolve<ICache>();
            //var cacheService = services.BuildServiceProvider().GetService<ICache>();
            //var files = FileHelper.GetAllFiles(new List<string>() { "json" }, Path.Combine(AppContext.BaseDirectory, "configs"));

            //foreach (var f in files)
            //{
            //    var name = Path.GetFileNameWithoutExtension(f);
            //    var str = FileHelper.ReadJsonFile(f);
            //    var obj = FileHelper.GetConfig(str, mapping[name]);
            //    cacheService.Set(name, obj);
            //}

            //cacheService.Set("key1", "hello world");


            return Task.CompletedTask;
        }
    }


}
