using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace Guldan.Service.FreeSql
{
    public class GldWorkManager : UnitOfWorkManager
    {
        public GldWorkManager(IdleBus<IFreeSql> ib, IServiceProvider serviceProvider)
            : base(ib.GetFreeSql(serviceProvider))
        {


        }
    }


}
