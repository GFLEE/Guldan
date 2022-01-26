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
        private string Encapsulate(string databaseword, string _encapsulation)
        {
            return string.Format(_encapsulation, databaseword);
        }
        public virtual string ResolveColumnName(PropertyInfo propertyInfo, string _encapsulation, string _dialect)
        {
            string columnName;

            if (_dialect.ToString() == EuDialect.DB2.ToString())
            {
                columnName = propertyInfo.Name;
            }
            else
            {
                columnName = Encapsulate(propertyInfo.Name, _encapsulation);
            }

            var columnattr = propertyInfo.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == typeof(ColumnAttribute).Name) as dynamic;
            if (columnattr != null)
            {
                columnName = Encapsulate(columnattr.Name, _encapsulation);
                if (Debugger.IsAttached)
                    Trace.WriteLine(String.Format("列名由{0}重写至{1}", propertyInfo.Name, columnName));
            }
            return columnName;
        }
    }
}
