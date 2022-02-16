using Guldan.DynamicWebApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Guldan.Service
{
    public static class DynamicWebApiService
    {
        public static void AddDynamicWebApiService(this IServiceCollection services)
        {
            services.AddDynamicWebApiCore<ServiceLocalSelectController, ServiceActionRouteFactory>();

            // services.AddDynamicWebApi((options) =>
            //{
       
            //    // 指定全局默认的 api 前缀
            //    //  options.DefaultApiPrefix = "api";

            //    /**
            //     * 清空API结尾，不删除API结尾;
            //     * 若不清空 CreatUserAsync 将变为 CreateUser
            //     */
            //    //   options.RemoveActionPostfixes.Clear();

            //    /**
            //     * 自定义 ActionName 处理函数;
            //     */
            //    //   options.GetRestFulActionName = (actionName) => actionName;

            //    /**
            //     * 指定程序集 配置 url 前缀为 apis
            //     * 如: http://localhost:8080/api/User/CreateUser
            //     */
            //    // options.AddAssemblyOptions(Assembly.GetExecutingAssembly(), apiPreFix: "api");

            //    /**
            //     * 指定程序集 配置所有的api请求方式都为 POST
            //     */
            //    // options.AddAssemblyOptions(Assembly.GetExecutingAssembly(), httpVerb: "POST");

            //    /**
            //     * 指定程序集 配置 url 前缀为 apis, 且所有请求方式都为POST
            //     * 如: http://localhost:8080/apis/User/CreateUser
            //     */
            //    //options.AddAssemblyOptions(Assembly.GetExecutingAssembly(), apiPreFix: "api", httpVerb: "POST");

            //});

        }


    }
}
