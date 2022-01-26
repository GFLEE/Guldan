using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Guldan.Common;
using Guldan.Common.Enum;
using Guldan.Common.Extension;
using Guldan.Service.Dapper.Dao;
using Guldan.Service.Dapper.Dao.Attributes;
using Guldan.Service.Dapper.Dao.Helper;
using Guldan.Service.Dapper.Dao.Resolver;

namespace Guldan.Service.Dapper
{
    public class DapperDao : IDapperDao
    {
        private static EuDialect _dialect = EuDialect.Oracle;
        private static string _encapsulation;
        private static string _getIdentitySql;
        private static string _getPagedListSql;

        private static readonly ConcurrentDictionary<Type, string> TableNames = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<string, string> ColumnNames = new ConcurrentDictionary<string, string>();

        private static readonly ConcurrentDictionary<string, string> StringBuilderCacheDict = new ConcurrentDictionary<string, string>();
        private static bool StringBuilderCacheEnabled = true;

        private static ITableNameResolver _tableNameResolver = new TableNameResolver();
        private static IColumnNameResolver _columnNameResolver = new ColumnNameResolver();

        public DapperDao()
        {
            SetDefaultConfigs(_dialect);
        }

        public static TKey Insert<TKey, TEntity>(this IDbConnection connection, TEntity entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if (typeof(TEntity).IsInterface) //FallBack to BaseType Generic Method : https://stackoverflow.com/questions/4101784/calling-a-generic-method-with-a-dynamic-type
            {
                return (TKey)typeof(DapperDao)
                    .GetMethods().Where(methodInfo => methodInfo.Name == nameof(Insert)
                    && methodInfo.GetGenericArguments().Count() == 2).Single()
                    .MakeGenericMethod(new Type[] { typeof(TKey), entityToInsert.GetType() })
                    .Invoke(null, new object[] { connection, entityToInsert, transaction, commandTimeout });
            }
            var idProps = DapperDaoHelper.GetIdProperties(entityToInsert).ToList();

            if (!idProps.Any())
            {
                throw new ArgumentException("Insert<T> 仅支持默认主键名称Id或带有KeyAttribute的键！");

            }

            var keyHasPredefinedValue = false;
            var baseType = typeof(TKey);
            var underlyingType = Nullable.GetUnderlyingType(baseType);
            var keytype = underlyingType ?? baseType;

            if (keytype != typeof(int)
                && keytype != typeof(uint)
                && keytype != typeof(long)
                && keytype != typeof(ulong)
                && keytype != typeof(short)
                && keytype != typeof(ushort)
                && keytype != typeof(Guid)
                && keytype != typeof(string))
            {
                throw new Exception("不是合法的主键类型！");
            }

            var name = DapperDaoHelper.GetTableName(entityToInsert, TableNames);
            var sb = new StringBuilder();
            sb.AppendFormat("insert into {0}", name);
            sb.Append(" (");
            BuildInsertParameters<TEntity>(sb);
            sb.Append(") ");
            sb.Append("values");
            sb.Append(" (");
            DapperDaoHelper.BuildInsertValues<TEntity>(sb, StringBuilderCacheDict);
            sb.Append(")");

            if (keytype == typeof(Guid))
            {   //赋一个默认主键
                var guidvalue = (Guid)idProps.First().GetValue(entityToInsert, null);
                if (guidvalue == Guid.Empty)
                {
                    var newguid = GuidWorker.SequentialGuid();
                    idProps.First().SetValue(entityToInsert, newguid, null);
                }
                else
                {
                    keyHasPredefinedValue = true;
                }
                sb.Append($";select '{idProps.First().GetValue(entityToInsert, null) }' as {GldConst.DefaultPrimaryKey}");
            }

            if ((keytype == typeof(int)
                || keytype == typeof(long))
                && Convert.ToInt64(idProps.First().GetValue(entityToInsert, null)) == 0)
            {
                sb.Append(";" + _getIdentitySql);
            }
            else
            {
                keyHasPredefinedValue = true;
            }
            if (keytype == typeof(string))
            {
                if (((string)(idProps.First().GetValue(entityToInsert, null).ToString())).IsNull())
                {
                    idProps.First().SetValue(entityToInsert, IdWorker.NewID, null);
                }
                else
                {
                    keyHasPredefinedValue = true;
                }
            }

            if (Debugger.IsAttached)
            {
                Trace.WriteLine(String.Format("Insert: {0}", sb));
            }

            var r = connection.Query(sb.ToString(), entityToInsert, transaction, true, commandTimeout);

            if (keytype == typeof(Guid) || keyHasPredefinedValue)
            {
                return (TKey)idProps.First().GetValue(entityToInsert, null);
            }
            return (TKey)r.First().id;
        }




        private static void BuildInsertParameters<T>(StringBuilder masterSb)
        {
            StringBuilderCache(masterSb, $"{typeof(T).FullName}_BuildInsertParameters", sb =>
            {
                //获取所有可编辑prop
                var props = DapperDaoHelper.GetScaffoldableProperties<T>().ToArray();

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
                        attr.GetType().Name == typeof(ReadOnlyAttribute).Name && DapperDaoHelper.IsReadOnly(property))) continue;

                    if (property.Name.Equals(GldConst.DefaultPrimaryKey, StringComparison.OrdinalIgnoreCase) && property.GetCustomAttributes(true).All(attr => attr.GetType().Name != typeof(RequiredAttribute).Name) && property.PropertyType != typeof(Guid)) continue;

                    sb.Append(DapperDaoHelper.GetColumnName(property, ColumnNames));
                    if (i < props.Count() - 1)
                        sb.Append(", ");
                }
                if (sb.ToString().EndsWith(", "))
                    sb.Remove(sb.Length - 2, 2);
            });
        }
        private static void StringBuilderCache(StringBuilder sb, string cacheKey, Action<StringBuilder> stringBuilderAction)
        {
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

        public static void SetDefaultConfigs(EuDialect EuDialect)
        {
            switch (EuDialect)
            {
                case EuDialect.PostgreSQL:
                    _dialect = EuDialect.PostgreSQL;
                    _encapsulation = "\"{0}\"";
                    _getIdentitySql = string.Format($"SELECT LASTVAL() AS {GldConst.DefaultPrimaryKey}");
                    _getPagedListSql = "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {RowsPerPage} OFFSET (({PageNumber}-1) * {RowsPerPage})";
                    break;
                case EuDialect.SQLite:
                    _dialect = EuDialect.SQLite;
                    _encapsulation = "\"{0}\"";
                    _getIdentitySql = string.Format($"SELECT LAST_INSERT_ROWID() AS {GldConst.DefaultPrimaryKey}");
                    _getPagedListSql = "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {RowsPerPage} OFFSET (({PageNumber}-1) * {RowsPerPage})";
                    break;
                case EuDialect.MySQL:
                    _dialect = EuDialect.MySQL;
                    _encapsulation = "`{0}`";
                    _getIdentitySql = string.Format("SELECT LAST_INSERT_ID() AS {GldConst.DefaultPrimaryKey}");
                    _getPagedListSql = "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {Offset},{RowsPerPage}";
                    break;
                case EuDialect.Oracle:
                    _dialect = EuDialect.Oracle;
                    _encapsulation = "\"{0}\"";
                    _getIdentitySql = "";
                    _getPagedListSql = "SELECT * FROM (SELECT ROWNUM PagedNUMBER, u.* FROM(SELECT {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy}) u) WHERE PagedNUMBER BETWEEN (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage})";
                    break;
                case EuDialect.DB2:
                    _dialect = EuDialect.DB2;
                    _encapsulation = "\"{0}\"";
                    _getIdentitySql = string.Format($"SELECT CAST(IDENTITY_VAL_LOCAL() AS DEC(31,0)) AS \"{GldConst.DefaultPrimaryKey}\" FROM SYSIBM.SYSDUMMY1");
                    _getPagedListSql = "Select * from (Select {SelectColumns}, row_number() over(order by {OrderBy}) as PagedNumber from {TableName} {WhereClause} Order By {OrderBy}) as t where t.PagedNumber between (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage})";
                    break;
                default:
                    _dialect = EuDialect.SQLServer;
                    _encapsulation = "[{0}]";
                    _getIdentitySql = string.Format($"SELECT CAST(SCOPE_IDENTITY()  AS BIGINT) AS [{GldConst.DefaultPrimaryKey}]");
                    _getPagedListSql = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {OrderBy}) AS PagedNumber, {SelectColumns} FROM {TableName} {WhereClause}) AS u WHERE PagedNumber BETWEEN (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage})";
                    break;
            }
        }

        public static string GetDialect()
        {
            return _dialect.ToString();
        }


    }
}
