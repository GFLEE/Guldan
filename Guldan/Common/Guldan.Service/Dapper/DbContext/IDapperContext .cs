using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.Dapper.DbContext
{
    public interface IDapperContext : IDisposable
    {
        IDbConnection Conn { get; }
        void InitConnection(); 

    }
}
