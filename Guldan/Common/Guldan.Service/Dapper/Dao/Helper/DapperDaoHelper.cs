using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Guldan.Common;
using Guldan.Common.Enum;
using Guldan.Service.Dapper.Dao.Attributes;
using Guldan.Service.Dapper.Dao.Resolver;

namespace Guldan.Service.Dapper.Dao.Helper
{
    public class DapperDaoHelper
    {
        private static ITableNameResolver _tableNameResolver { get; set; }
        private static IColumnNameResolver _columnNameResolver { get; set; }
        public DapperDaoHelper(ITableNameResolver tableNameResolver, IColumnNameResolver columnNameResolver)
        {
            _tableNameResolver = tableNameResolver;
            _columnNameResolver = columnNameResolver;

        }
        public static IEnumerable<PropertyInfo> GetIdProperties(object entity)
        {
            var type = entity.GetType();
            return GetIdProperties(type);
        }


        public static IEnumerable<PropertyInfo> GetScaffoldableProperties<T>()
        {
            IEnumerable<PropertyInfo> props = typeof(T).GetProperties();

            props = props.Where(p => p.GetCustomAttributes(true).Any(attr => attr.GetType().Name == typeof(EditableAttribute).Name && !IsEditable(p)) == false);


            return props.Where(p => p.PropertyType.IsSimpleType() || IsEditable(p));
        }
        public static bool IsEditable(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(false);
            if (attributes.Length > 0)
            {
                dynamic write = attributes.FirstOrDefault(x => x.GetType().Name == typeof(EditableAttribute).Name);
                if (write != null)
                {
                    return write.AllowEdit;
                }
            }
            return false;
        }
        public static void BuildInsertValues<T>(StringBuilder masterSb, ConcurrentDictionary<string, string> StringBuilderCacheDict)
        {
            StringBuilderCache(masterSb, $"{typeof(T).FullName}_BuildInsertValues", StringBuilderCacheDict, sb =>
            {

                var props = GetScaffoldableProperties<T>().ToArray();
                for (var i = 0; i < props.Count(); i++)
                {
                    var property = props.ElementAt(i);
                    if (property.PropertyType != typeof(Guid) && property.PropertyType != typeof(string)
                          && property.GetCustomAttributes(true).Any(attr => attr.GetType().Name == typeof(KeyAttribute).Name)
                          && property.GetCustomAttributes(true).All(attr => attr.GetType().Name != typeof(RequiredAttribute).Name))
                        continue;
                    if (property.GetCustomAttributes(true).Any(attr =>
                        attr.GetType().Name == typeof(IgnoreInsertAttribute).Name ||
                        attr.GetType().Name == typeof(NotMappedAttribute).Name ||
                        attr.GetType().Name == typeof(ReadOnlyAttribute).Name && IsReadOnly(property))
                    ) continue;

                    if (property.Name.Equals(GldConst.DefaultPrimaryKey, StringComparison.OrdinalIgnoreCase)
                    && property.GetCustomAttributes(true).All(attr => attr.GetType().Name != typeof(RequiredAttribute).Name)
                    && property.PropertyType != typeof(Guid)) continue;

                    sb.AppendFormat("@{0}", property.Name);
                    if (i < props.Count() - 1)
                        sb.Append(", ");
                }
                if (sb.ToString().EndsWith(", "))
                    sb.Remove(sb.Length - 2, 2);
            });
        }

        public static void StringBuilderCache(StringBuilder sb, string cacheKey, ConcurrentDictionary<string, string> StringBuilderCacheDict, Action<StringBuilder> stringBuilderAction)
        {
            var StringBuilderCacheEnabled = true;
            if (StringBuilderCacheEnabled && StringBuilderCacheDict.TryGetValue(cacheKey, out string value))
            {
                sb.Append(value);
                return;
            }

            StringBuilder newSb = new StringBuilder();
            stringBuilderAction(newSb);
            value = newSb.ToString();
            StringBuilderCacheDict.AddOrUpdate(cacheKey, value, (t, v) => value);
            sb.Append(value);
        }

        //Get all properties that are named Id or have the Key attribute
        //For Get(id) and Delete(id) we don't have an entity, just the type so this method is used
        public static IEnumerable<PropertyInfo> GetIdProperties(Type type)
        {
            var tp = type.GetProperties().Where(p => p.GetCustomAttributes(true).Any(attr => attr.GetType().Name == typeof(KeyAttribute).Name)).ToList();
            return tp.Any() ? tp : type.GetProperties().Where(p => p.Name.Equals(GldConst.DefaultPrimaryKey, StringComparison.OrdinalIgnoreCase));
        }

        public static string GetTableName(object entity, ConcurrentDictionary<Type, string> TableNames)
        {
            var type = entity.GetType();
            return GetTableName(type, TableNames);
}

        public static string GetTableName(Type type, ConcurrentDictionary<Type, string> TableNames, string _encapsulation, string _dialect)
        {
            string tableName;

            if (TableNames.TryGetValue(type, out tableName))
                return tableName;

            tableName = _tableNameResolver.ResolveTableName(type, _encapsulation, _dialect);

            TableNames.AddOrUpdate(type, tableName, (t, v) => tableName);

            return tableName;
        }
        public static string GetColumnName(PropertyInfo propertyInfo, ConcurrentDictionary<string, string> ColumnNames, string _encapsulation, string _dialect)
        {
            string columnName, key = string.Format("{0}.{1}", propertyInfo.DeclaringType, propertyInfo.Name);

            if (ColumnNames.TryGetValue(key, out columnName))
                return columnName;

            columnName = _columnNameResolver.ResolveColumnName(propertyInfo, _encapsulation, _dialect);

            ColumnNames.AddOrUpdate(key, columnName, (t, v) => columnName);

            return columnName;
        }
        public static bool IsReadOnly(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(false);
            if (attributes.Length > 0)
            {
                dynamic write = attributes.FirstOrDefault(x => x.GetType().Name == typeof(ReadOnlyAttribute).Name);
                if (write != null)
                {
                    return write.IsReadOnly;
                }
            }
            return false;
        }


    }
}
