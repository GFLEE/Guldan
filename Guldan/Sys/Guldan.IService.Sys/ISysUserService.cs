using Guldan.Data;
using Guldan.Data.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Guldan.IService.Sys
{
    public interface ISysUserService
    {

        SYS_USER AddUser(SysUserDto userDto);



    }
}