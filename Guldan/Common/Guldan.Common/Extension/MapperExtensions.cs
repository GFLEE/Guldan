using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Guldan.Common
{
    /// <summary>
    /// AutoMapper 映射
    /// 如果需要忽略某个字段就在字段上面打标签如下：
    /// [IgnoreMap]
    /// public string IgnoreValue { get; set; }
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object Sync = new object();

        /// <summary>
        /// Mapper
        /// </summary>
        private static Mapper Mapper { get; set; }

        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="destination">目标对象</param>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            if (Mapper == null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
                Mapper = (Mapper)config.CreateMapper();

            }
            return MapTo<TDestination>(source, destination);
        }

        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源对象</param>
        public static TDestination MapTo<TDestination>(this object source) where TDestination : new()
        {
            if (Mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                            cfg.CreateMap(source.GetType(), typeof(TDestination)));
                Mapper = (Mapper)config.CreateMapper();

            }
            return MapTo(source, new TDestination());
        }

        /// <summary>
        /// 集合列表类型映射,默认字段名字一一对应
        /// </summary>
        /// <typeparam name="TDestination">转化之后的model，可以理解为viewmodel</typeparam>
        /// <typeparam name="TSource">要被转化的实体，Entity</typeparam>
        /// <param name="source">可以使用这个扩展方法的类型，任何引用类型</param>
        /// <returns>转化之后的实体列表</returns>
        public static IEnumerable<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
            where TDestination : class
            where TSource : class
        {
            if (source == null) return new List<TDestination>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }
        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        private static TDestination MapTo<TDestination>(object source, TDestination destination)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            var sourceType = GetType(source);
            var destinationType = GetType(destination);
            var map = GetMap(sourceType, destinationType);
            if (map != null)
                return Mapper.Map(source, destination);
            lock (Sync)
            {
                map = GetMap(sourceType, destinationType);
                if (map != null)
                    return Mapper.Map(source, destination);
                InitMaps(sourceType, destinationType);
            }
            return Mapper.Map(source, destination);
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        private static Type GetType(object obj)
        {
            var type = obj.GetType();
            if ((obj is System.Collections.IEnumerable) == false)
                return type;
            if (type.IsArray)
                return type.GetElementType();
            var genericArgumentsTypes = type.GetTypeInfo().GetGenericArguments();
            if (genericArgumentsTypes == null || genericArgumentsTypes.Length == 0)
                throw new ArgumentException("泛型类型参数不能为空");
            return genericArgumentsTypes[0];
        }

        /// <summary>
        /// 获取映射配置
        /// </summary>
        private static TypeMap GetMap(Type sourceType, Type destinationType)
        {
            try
            {
                return Mapper.ConfigurationProvider.FindTypeMapFor(sourceType, destinationType);
            }
            catch (InvalidOperationException)
            {
                lock (Sync)
                {
                    try
                    {
                        return Mapper.ConfigurationProvider.FindTypeMapFor(sourceType, destinationType);
                    }
                    catch (InvalidOperationException)
                    {
                        InitMaps(sourceType, destinationType);
                    }
                    return Mapper.ConfigurationProvider.FindTypeMapFor(sourceType, destinationType);
                }
            }
        }

        /// <summary>
        /// 初始化映射配置 ,程序集注册
        /// </summary>
        private static void InitMaps(Type sourceType, Type destinationType)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap(sourceType, destinationType));
                Mapper = config.CreateMapper() as Mapper;
                var maps = Mapper.ConfigurationProvider.GetAllTypeMaps();

                ClearConfig();


            }
            catch (InvalidOperationException)
            {


            }
        }




        /// <summary>
        /// 清空配置
        /// </summary>
        private static void ClearConfig()
        {
            var typeMapper = typeof(Mapper).GetTypeInfo();
            var configuration = typeMapper.GetDeclaredField("_configuration");
            if (configuration != null)
            {
                configuration.SetValue(null, null, BindingFlags.Static, null, CultureInfo.CurrentCulture);

            }
        }

        /// <summary>
        /// 将源集合映射到目标集合
        /// </summary>
        /// <typeparam name="TDestination">目标元素类型,范例：Sample,不要加List</typeparam>
        /// <param name="source">源集合</param>
        public static List<TDestination> MapToList<TDestination>(this System.Collections.IEnumerable source)
        {
            return MapTo<List<TDestination>>(source);
        }





    }
}
