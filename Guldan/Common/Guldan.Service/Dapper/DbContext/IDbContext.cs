using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guldan.Service.RemotingService;

namespace Guldan.Service.Dapper.DbContext
{
    public interface IDbContext : IDisposable
    {
        void UseTransaction(Action p);







    }
}
