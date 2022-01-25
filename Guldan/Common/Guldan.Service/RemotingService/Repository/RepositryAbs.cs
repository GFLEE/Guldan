using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Guldan.Service.RemotingService.Repository
{
    public abstract class RepositryAbs<T, TRepository>
    {
        public virtual void OnBeforeAdd(IDbContext context, T entity)
        {
        }

        public virtual T OnBeforeUpate(IDbContext context, T entity)
        {
        }

        public virtual void OnBeforeDelete(IDbContext context, T entity)
        {


        }
        public virtual void OnBeforeGetList(IDbContext context, List<T> entities)
        {

        }

        public virtual void OnAfterGetList(IDbContext context, List<T> entities)
        {

        }

        public virtual void OnAfterAdd(IDbContext context, T entity)
        {

        }

        public virtual T OnAfterUpate(IDbContext context, T entity)
        {


        }

        public virtual void OnAfterDelete(IDbContext context, T entity)
        {
        }



    }
}
