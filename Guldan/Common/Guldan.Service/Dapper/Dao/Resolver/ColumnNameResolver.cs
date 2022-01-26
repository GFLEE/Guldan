using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Guldan.Common.Enum;

namespace Guldan.Service.Dapper.Dao.Resolver
{
    public class ColumnNameResolver : IColumnNameResolver
{
        public virtual string ResolveColumnName(PropertyInfo propertyInfo)
        {
            string columnName;

            if (GetDialect() == Dialect.DB2.ToString())
            {
                columnName = propertyInfo.Name;
            }
            else
            {
                columnName = Encapsulate(propertyInfo.Name);
            }

            var columnattr = propertyInfo.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == typeof(ColumnAttribute).Name) as dynamic;
            if (columnattr != null)
            {
                columnName = Encapsulate(columnattr.Name);
                if (Debugger.IsAttached)
                    Trace.WriteLine(String.Format("Column name for type overridden from {0} to {1}", propertyInfo.Name, columnName));
            }
            return columnName;
        }
    }
}
