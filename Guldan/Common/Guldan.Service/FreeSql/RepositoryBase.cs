using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace Guldan.Service.FreeSql
{
    public class GldRepositoryBase<TEntity> : RepositoryBase<TEntity, string>,
        IRepositoryBase<TEntity> where TEntity : class, new()
    {
        public GldRepositoryBase(GldWorkManager muowm) : base(muowm.Orm)
        {
            muowm.Binding(this); 
        }
    }
}
