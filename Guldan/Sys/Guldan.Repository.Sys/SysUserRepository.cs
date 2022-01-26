using System;
using Guldan.Data;
using Guldan.Service.Dapper.DbContext;
using Guldan.Service.RemotingService.Repository;

namespace Guldan.Repository.Sys
{
    public class SysUserRepository : RepositoryBase<Sys_User, SysUserRepository>
    {
        public SysUserRepository()
        {



        }

        public override void OnBeforeAdd(IDbContext context, Sys_User entity)
        {
            base.OnBeforeAdd(context, entity);
        }



    }
}
