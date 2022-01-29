using Newtonsoft.Json;

namespace Guldan.Common
{
 
    public class ResponseInfo<T> : IResponse<T>
    { 
        public ResponseInfo<T> Success(T data, string msg = null)
        {
            isSucceed = true;
            Data = data;
            this.msg = msg;

            return this;
        }
         
        public ResponseInfo<T> Failed(string msg = null, T data = default(T))
        {
            isSucceed = false;
            this.msg = msg;
            Data = data;

            return this;
        } 

       
        [JsonIgnore] 
        public bool isSucceed { get; private set; }
         
        public int code => isSucceed ? 1 : 0;
 
        public string msg { get; private set; }
         
        public T Data { get; private set; }

     
    }

 
    public static partial class ResponseInfo
    {
       
         public static IResponse Success<T>(T data = default(T), string msg = null)
        {
            return new ResponseInfo<T>().Success(data, msg);
        }
 
        public static IResponse Success()
        {
            return Success<string>();
        }
 
        public static IResponse Failed<T>(string msg = null, T data = default(T))
        {
            return new ResponseInfo<T>().Failed(msg, data);
        }

 
        public static IResponse Failed(string msg = null)
        {
            return new ResponseInfo<string>().Failed(msg);
        }

 
        public static IResponse Result<T>(bool success)
        {
            return success ? Success<T>() : Failed<T>();
        }
 
        public static IResponse Result(bool success)
        {
            return success ? Success() : Failed();
        }
    }
}