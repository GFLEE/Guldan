using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.FreeSql
{
    public interface IRepositoryBase<TEntity> : IRepositoryBase<TEntity, string> where TEntity : class
    {


    }
}
