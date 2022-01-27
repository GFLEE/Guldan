using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guldan.Common.DI;
using Guldan.Service.Dapper.DbContext;
using Guldan.Service.RemotingService;
using Microsoft.AspNetCore.Http;

namespace Guldan.Service.Factory
{
    public class BizContextFactory
    {
        public static IDbContext GetBizContext()
        {
            return IocService.Resolve<IDbContext>();
        }
    }
}
