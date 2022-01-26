using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.Dapper.Dao
{
    public interface IDapperDao
    {

        public IDbConnection Conn { get; set; }

        public IDbConnection InitConn(IDbConnection dbConnection);

        public TKey Insert<TKey, TEntity>(TEntity entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null);





    }
}
