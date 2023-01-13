using Guldan.Common;
using Guldan.Common.Model;
using Guldan.Data;
using Guldan.Data.Dto;
using Guldan.DynamicWebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Guldan.IService.Sys
{
    public interface ISysUserService : IDynamicWebApi 
    {

        WebJsonInfo AddUser(SysUserDto userDto);



    }
}