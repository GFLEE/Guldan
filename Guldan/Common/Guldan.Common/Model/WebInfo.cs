using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Common.Model
{
    public class WebJsonInfo
    {
        public WebJsonInfo()
        {
            code = "1";
            success = true;
            msg = "成功！";
        }
        public string code { get; set; }
        public string msg { get; set; }
        public bool success { get; set; }
        public object json { get; set; }

    }


    public class WebJsonInfo<T>
    {
        public string code { get; set; }
        public string errmsg { get; set; }
        public bool success { get; set; }
        public T json { get; set; }

    }

}
