using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guldan.Common.Enum;
using Microsoft.CSharp.RuntimeBinder;

namespace Guldan.Service.Dapper.Dao.Resolver
{
    public class TableNameResolver : ITableNameResolver
    {
        private string Encapsulate(string databaseword, string _encapsulation)
        {
            return string.Format(_encapsulation, databaseword);
        }
        public virtual string ResolveTableName(Type type, string _encapsulation, string _dialect)
        {
            string tableName;

            if (_dialect.ToString() == EuDialect.DB2.ToString())
            {
                tableName = type.Name;
            }
            else
            {
                tableName = Encapsulate(type.Name, _encapsulation);
            }

            var tableattr = type.GetCustomAttributes(true).SingleOrDefault(attr => attr.GetType().Name == typeof(TableAttribute).Name) as dynamic;
            if (tableattr != null)
            {
                tableName = Encapsulate(tableattr.Name, _encapsulation);
                try
                {
                    if (!String.IsNullOrEmpty(tableattr.Schema))
                    {
                        string schemaName = Encapsulate(tableattr.Schema, _encapsulation);
                        tableName = String.Format("{0}.{1}", schemaName, tableName);
                    }
                }
                catch (RuntimeBinderException)
                {
                    //Schema doesn't exist on this attribute.
                }
            }

            return tableName;
        }
    }

}
