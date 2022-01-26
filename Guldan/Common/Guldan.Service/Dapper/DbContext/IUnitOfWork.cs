using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.Dapper.DbContext
{
    public interface IUnitOfWork
    {
        Guid Id { get; } 
        IDbConnection Conn { get; } 
        IDbTransaction Tran { get; }
        void BeginTran();
        void Commit();
        void Rollback();
    }
}
