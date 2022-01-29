using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Common
{
    public class HostHelper
    {
        public static IWebHostEnvironment host { get; set; }

        public static bool IsDev()
        {
            return host.IsDevelopment();
        }
    }
}
