using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Common.QuartzNet
{
    public class QuartzModel
    { 
        public static string Port { get; set; } = "9966"; 
        public static string Ip { get; set; } = "127.0.0.1"; 
        public static string ThreadPool { get; set; } = "100"; 
        public static string Priority { get; set; } = "Normal"; 
        public static string BindName { get; set; } = "QuartzScheduler";
        public static string ChannelType { get; set; } = "tcp";


    }
}
