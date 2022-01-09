using Autofac;
using Guldan.Common.QuartzNet;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz.Impl;
using Quartz;

namespace Guldan.QuartzNet.Base
{
    public class DefaultJobHostService : IHostedService, IDisposable
    {
        public Task StartAsync(CancellationToken stoppingToken)
        {

            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "接口平台定时任务监控";

            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = QuartzModel.ThreadPool;
            properties["quartz.threadPool.threadPriority"] = QuartzModel.Priority;

            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = QuartzModel.Ip;
            properties["quartz.scheduler.exporter.bindName"] = QuartzModel.BindName;
            properties["quartz.scheduler.exporter.channelType"] = QuartzModel.ChannelType;



            //IContainer container = JobModules.ConfigureContainer(new ContainerBuilder()).Build();
            //var scheFactory = container.Resolve<ISchedulerFactory>() as StdSchedulerFactory; // 通过容器获取，用于依赖注入的实现
            //scheFactory.Initialize(properties);

            //GuldanScheduler.Scheduler = scheFactory.GetScheduler().Result;

            //var configInfo = _dbService.GetPushConfiguration();
            //if (configInfo != null && configInfo.Count > 0)
            //{
            //    IJobDetail[] jobs = new IJobDetail[configInfo.Count + 1];
            //    ITrigger[] triggers = new ITrigger[configInfo.Count + 1];
            //    int count = 0;
            //    foreach (var info in configInfo)
            //    {
            //        jobs[count] = JobBuilder.Create<InterfaceSystemJob>()
            //      .WithIdentity($"Job_{info.InterName}", $"")
            //      .Build();
            //        triggers[count] = TriggerBuilder.Create()
            //        .WithIdentity($"Trigger_{info.InterName}", $"")
            //        .StartNow()
            //        .WithCronSchedule($"{info.CronExpression}")
            //        .Build();

            //        jobs[count].JobDataMap.Add("key", info);

            //        GuldanScheduler.Scheduler.ScheduleJob(jobs[count], triggers[count]);

            //        count++;
            //    }

            //    {
            //        // 其他定时任务
            //        //jobs[count] = JobBuilder.Create<AutoDeleteHistoryLogsJob>()
            //        // .WithIdentity($"Job_AutoDeleteLogs", $"")
            //        // .Build();
            //        //triggers[count] = TriggerBuilder.Create()
            //        //.WithIdentity($"Trigger_AutoDeleteLogs", $"")
            //        //.StartNow()
            //        //.WithCronSchedule(""))
            //        //.Build();

            //        GuldanScheduler.Scheduler.ScheduleJob(jobs[count], triggers[count]);
            //    }


            //    GuldanScheduler.Scheduler.Start();
            //}



            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {

            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }
}
