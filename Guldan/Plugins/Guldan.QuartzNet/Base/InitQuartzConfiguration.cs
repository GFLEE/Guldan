using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Guldan.Common.QuartzNet;
using Microsoft.Extensions.Hosting;


namespace Guldan.QuartzNet.Base
{
    public class InitQuartzConfiguration : IHostedService, IDisposable
    {

        public Task StartAsync(CancellationToken cancellationToken)
        {
            QuartzModel.Ip = "127.0.0.1";
            QuartzModel.Port = "9966";
            QuartzModel.ThreadPool = "100";
            QuartzModel.Priority = "Normal";
            QuartzModel.BindName = "QuartzScheduler";
            QuartzModel.ChannelType = "tcp";

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

    }
}
