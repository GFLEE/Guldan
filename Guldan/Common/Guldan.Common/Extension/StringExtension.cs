using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guldan.Common
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        public static bool IsIn(this string str, params string[] data)
        {
            foreach (var item in data)
            {
                if (str == item)
                {
                    return true;
                }
            }
            return false;
        }

        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }


        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }


        public static string Right(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        public static string GetCamelCaseFirstWord(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 1)
            {
                return str;
            }

            var res = Regex.Split(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})");

            if (res.Length < 1)
            {
                return str;
            }
            else
            {
                return res[0];
            }
        }

        public static string GetPascalCaseFirstWord(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 1)
            {
                return str;
            }

            var res = Regex.Split(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})");

            if (res.Length < 2)
            {
                return str;
            }
            else
            {
                return res[1];
            }
        }

        public static string GetPascalOrCamelCaseFirstWord(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length <= 1)
            {
                return str;
            }

            if (str[0] >= 65 && str[0] <= 90)
            {
                return GetPascalCaseFirstWord(str);
            }
            else
            {
                return GetCamelCaseFirstWord(str);
            }
        }


        /// <summary>
        /// 用法如 Oracle like
        /// </summary>
        /// <param name="toSearch"></param>
        /// <param name="toFind"></param>
        /// <returns></returns>
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
