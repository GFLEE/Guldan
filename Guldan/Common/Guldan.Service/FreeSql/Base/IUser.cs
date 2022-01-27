using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.FreeSql.Base
{
    /// <summary>
    /// 用户信息接口
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 昵称
        /// </summary>
        string NickName { get; }

  
      
    }
}
