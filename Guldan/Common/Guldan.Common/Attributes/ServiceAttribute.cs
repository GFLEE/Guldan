using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public string ServiceName { get; }
        public ServiceAttribute()
        {
            ServiceName = string.Empty;
        }

        public ServiceAttribute(string serviceName)
        {
            ServiceName = serviceName;
        }

    }
}
