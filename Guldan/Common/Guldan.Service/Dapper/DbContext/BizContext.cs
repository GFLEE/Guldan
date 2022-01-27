using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.Dapper.DbContext
{
    public class BizContext : DapperContext, IBizContext
    {
        public BizContext()
        {


        }


        public override T Add<T>(T entity)
        {


            return base.Add(entity);
        }





    }
}
