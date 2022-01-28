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
        public DataType Type { get; set; }

        public string ConnectionString { get; set; }

        public int IdleTime { get; set; } = 10;

        public bool MonitorCommand { get; set; } = false;

        public bool Curd { get; set; } = false;

    }
}
