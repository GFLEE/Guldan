using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Guldan.Common;

namespace Guldan.DynamicWebApi
{
    public class Options
    {
        public Options()
        {
            RemoveControllerPostfixes = new List<string>() { "AppService", "ApplicationService" };
            RemoveActionPostfixes = new List<string>() { "Async" };
            FormBodyBindingIgnoredTypes = new List<Type>() { typeof(IFormFile) };
            DefaultHttpVerb = "POST";
            DefaultApiPrefix = "api";
            AssemblyDynamicWebApiOptions = new Dictionary<Assembly, WebApiOptions>();
        }



        public string DefaultHttpVerb { get; set; }

        public string DefaultAreaName { get; set; }


        public string DefaultApiPrefix { get; set; }


        public List<string> RemoveControllerPostfixes { get; set; }


        public List<string> RemoveActionPostfixes { get; set; }


        public List<Type> FormBodyBindingIgnoredTypes { get; set; }


        public Func<string, string> GetRestFulActionName { get; set; }


        public Dictionary<Assembly, WebApiOptions> AssemblyDynamicWebApiOptions { get; }

        public ISelectController SelectController { get; set; } = new DefaultSelectController();
        public IActionRouteFactory ActionRouteFactory { get; set; } = new DefaultActionRouteFactory();


        public void Valid()
        {
            if (string.IsNullOrEmpty(DefaultHttpVerb))
            {
                throw new ArgumentException($"{nameof(DefaultHttpVerb)} 不能为空！");
            }

            if (string.IsNullOrEmpty(DefaultAreaName))
            {
                DefaultAreaName = string.Empty;
            }

            if (string.IsNullOrEmpty(DefaultApiPrefix))
            {
                DefaultApiPrefix = string.Empty;
            }

            if (FormBodyBindingIgnoredTypes == null)
            {
                throw new ArgumentException($"{nameof(FormBodyBindingIgnoredTypes)} 不能为空！");
            }

            if (RemoveControllerPostfixes == null)
            {
                throw new ArgumentException($"{nameof(RemoveControllerPostfixes)} 不能为空！");
            }
        }


        public void AddAssemblyOptions(Assembly assembly, string apiPreFix = null, string httpVerb = null)
        {
            if (assembly == null)
            {
                throw new ArgumentException($"{nameof(assembly)} 不能为空！");
            }

            this.AssemblyDynamicWebApiOptions[assembly] = new WebApiOptions(apiPreFix, httpVerb);
        }
    }
}