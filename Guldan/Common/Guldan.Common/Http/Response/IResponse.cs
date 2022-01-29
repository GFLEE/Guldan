using System.Text.Json.Serialization;

namespace Guldan.Common
{
   
    public interface IResponse
    { 
        [JsonIgnore]
        bool isSucceed { get; } 
          
        public string msg { get; }
    }

 
    public interface IResponse<T> : IResponse
    { 
        T Data { get; }
    }
}