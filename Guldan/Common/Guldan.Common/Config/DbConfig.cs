using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType = FreeSql.DataType;

namespace Guldan.Common.Config
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DbConfig
    {

        public List<DbConfigDetail> configs { get; set; } = new List<DbConfigDetail>();
        public int idleTime { get; set; } = 10;

        public bool monitorCommand { get; set; } = false;

        public bool curd { get; set; } = false;

    }

    public class DbConfigDetail
    {
        public DataType type { get; set; }
        public string connectionString { get; set; }
        public bool isEnable { get; set; }

    }
}
