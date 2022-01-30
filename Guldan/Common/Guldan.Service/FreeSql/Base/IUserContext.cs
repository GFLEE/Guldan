using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.FreeSql.Base
{
    public interface IUserContext
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 用户编码
        /// </summary>
        string Code { get; }
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
