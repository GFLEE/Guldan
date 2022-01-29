using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guldan.Common;
using Guldan.Common.Extension;
using Guldan.Data;
using Guldan.Data.Dto;
using Guldan.IService.Sys;
using Guldan.Repository.Sys.IRepository;

namespace Guldan.Service.Sys
{
    public class SysUserService : ISysUserService
    {
        private readonly ISysUserRepository _userRepository;

        public SysUserService(ISysUserRepository sysUserRepository)
        {

            _userRepository = sysUserRepository;

        }

        public SYS_USER AddUser(SysUserDto userDto)
        {
            //SYS_USER user = _userRepository.GetNew();

            SYS_USER user = userDto.MapTo<SYS_USER>();
            user.ID = IdWorker.NewID;
            _userRepository.InsertAsync(user);
            return user;
        }
    }
}
