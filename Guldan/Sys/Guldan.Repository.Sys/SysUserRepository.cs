using System;
using FreeSql;
using Guldan.Data;
using Guldan.Repository.Sys.IRepository;
using Guldan.Service.Dapper.DbContext;
using Guldan.Service.FreeSql;
using Guldan.Service.RemotingService.Repository;

namespace Guldan.Repository.Sys
{
    public class SysUserRepository : GldRepositoryBase<SYS_USER>, ISysUserRepository
    {
        public SysUserRepository(GldWorkManager muowm)
            : base(muowm)
        {


        }
        //在这里增加 CURD 以外的方法

       


    }


    

 
}
