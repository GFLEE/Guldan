using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.Dapper.Dao.Resolver
{
    public interface ITableNameResolver
    {
        string ResolveTableName(Type type, string _encapsulation, string _dialect);

    }


}
