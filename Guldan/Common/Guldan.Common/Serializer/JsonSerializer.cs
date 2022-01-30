using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Common
{
    public static class JsonSerializer
    {
        
         

        public static T DeSerialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static string Serialize(this object obj, Formatting formatting = Formatting.None, bool flag = false)
        {
            return JsonConvert.SerializeObject(obj);
        }



        public static T DeSerialize<T>(this string str, bool flag = false)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static object DeSerialize(this string str, Type type, bool flag = false)
        {
            return JsonConvert.DeserializeObject(str, type);
        }
    }
}
