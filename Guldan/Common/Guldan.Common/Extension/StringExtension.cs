using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guldan.Common.Extension
{
    public static class StringExtensions
    {
        public static bool Like(this string toSearch, string toFind)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }

      
        public static bool IsNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
         
        public static bool NotNull(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }
 
        public static bool EqualsIgnoreCase(this string s, string value)
        {
            return s.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

         
        public static string FirstCharToLower(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToLower() + s.Substring(1);
            return str;
        }
         
        public static string FirstCharToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToUpper() + s.Substring(1);
            return str;
        }
 
        public static string ToBase64(this string s)
        {
            return s.ToBase64(Encoding.UTF8);
        }

  
        public static string ToBase64(this string s, Encoding encoding)
        {
            if (s.IsNull())
                return string.Empty;

            var bytes = encoding.GetBytes(s);
            return bytes.ToBase64();
        }

        public static string ToPath(this string s)
        {
            if (s.IsNull())
                return string.Empty;

            return s.Replace(@"\", "/");
        }

        public static string Format(this string str, object obj)
        {
            if (str.IsNull())
            {
                return str;
            }
            string s = str;
            if (obj.GetType().Name == "JObject")
            {
                foreach (var item in (Newtonsoft.Json.Linq.JObject)obj)
                {
                    var k = item.Key.ToString();
                    var v = item.Value.ToString();
                    s = Regex.Replace(s, "\\{" + k + "\\}", v, RegexOptions.IgnoreCase);
                }
            }
            else
            {
                foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
                {
                    var xx = p.Name;
                    var yy = p.GetValue(obj).ToString();
                    s = Regex.Replace(s, "\\{" + xx + "\\}", yy, RegexOptions.IgnoreCase);
                }
            }
            return s;
        }
    }
}
