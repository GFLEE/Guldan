using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.Dapper.DbContext
{
    public abstract class DisposableObject : IDisposable
    {
        ~DisposableObject()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {


        }


        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            this.ExplicitDispose();
        }
    }
}
