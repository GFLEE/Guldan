using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Guldan.Common
{
    public class FileHelper : IDisposable
    {
        private bool _alreadyDispose = false;

        public FileHelper()
        {
        }

        ~FileHelper()
        {
            Dispose();
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            _alreadyDispose = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



        public static List<string> GetAllFiles(List<string> extensions, string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                throw new Exception($"{dirPath}不是合法目录！");
            }

            var myFiles = Directory
               .EnumerateFiles(dirPath, "*.*", SearchOption.TopDirectoryOnly)
               .Where(s => extensions.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant())).ToList();
            if (!myFiles.Any())
            {
                return new List<string>();
            }

            return myFiles;
        }

        public static string ReadJsonFile(string filePath)
        {
            string result = string.Empty;
            if (!File.Exists(filePath)
                || !(Path.GetExtension(filePath).TrimStart('.').ToLowerInvariant().Equals("json")))
            {
                throw new Exception($"{filePath}不是合法文件路径或类型！");
            }
            else
            {
                StreamReader streamReader = new StreamReader(filePath);
                result = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
            }

            return result;

        }

        public static T GetConfig<T>(string jsonStr)
        {
            return jsonStr.DeSerialize<T>();
        }

        public static object GetConfig(string jsonStr, Type type)
        {
            return jsonStr.DeSerialize(type);
        }
        public static string ReadFile(string Path)
        {
            string s;
            if (!File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader streamReader = new StreamReader(Path);
                s = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
            }

            return s;
        }


        public static string ReadFile(string Path, Encoding encode)
        {
            string s;
            if (!File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader streamReader = new StreamReader(Path, encode);
                s = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
            }

            return s;
        }

        public static void WriteStringToFile(string Path, string Strings)
        {
            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }
            StreamWriter streamWriter = new StreamWriter(Path, false);
            streamWriter.Write(Strings);
            streamWriter.Close();
            streamWriter.Dispose();
        }


        public static void WriteWithCoding(string Path, string Strings, Encoding encode)
        {
            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }
            StreamWriter streamWriter = new StreamWriter(Path, false, encode);
            streamWriter.Write(Strings);
            streamWriter.Close();
            streamWriter.Dispose();
        }

    }
}
