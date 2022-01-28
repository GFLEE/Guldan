using Guldan.Common.Extension;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Guldan.Common.Config
{
    public interface IConfigHelper
    {
        IConfiguration LoadConfig(string fileName, string environmentName = "", bool reloadOnChange = false);

        T GetConfig<T>(string fileName, string environmentName = "", bool reloadOnChange = false);

        void BindJson(string fileName, object instance, string environmentName = "", bool reloadOnChange = false);


    }
    public class ConfigHelper
    {

        public T ReadConfigFile<T>(string path)
        {
            //var files




            return default(T);
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="environmentName">环境名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public IConfiguration LoadConfig(string fileName, string environmentName = "", bool reloadOnChange = false)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "configs");
            if (!Directory.Exists(path))
            {
                return null;
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile(fileName.ToLower() + ".json", true, reloadOnChange);

            //if (environmentName.NotNull())
            //{
            //    builder.AddJsonFile(fileName.ToLower() + "." + environmentName + ".json", optional: true, reloadOnChange: reloadOnChange);
            //}

            return builder.Build();
        }

        public T GetConfig<T>(string fileName, string environmentName = "", bool reloadOnChange = false)
        {
            var configuration = LoadConfig(fileName, environmentName, reloadOnChange);
            if (configuration == null)
                return default;

            return configuration.Get<T>();
        }


        public void BindJson(string fileName, object instance, string environmentName = "", bool reloadOnChange = false)
        {
            var configuration = LoadConfig(fileName, environmentName, reloadOnChange);
            if (configuration == null || instance == null)
                return;

            configuration.Bind(instance);
        }



    }
}
