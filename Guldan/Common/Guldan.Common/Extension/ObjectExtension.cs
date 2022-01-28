using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Common.Extension
{
    public static class ObjectExtension
    {
        public static bool IsObJNull(this object o)
        {
            return o == null ? true : false;
        }


        public static string ThenException(this bool r, string msg)
        {
            if (r)
            {
                throw new Exception($"{msg}");
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
