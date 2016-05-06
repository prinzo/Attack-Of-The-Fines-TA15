using System;
using Castle.Windsor;
using FineBot.Workers.DI;
using FineBot.Workers.Jobs;
using Quartz;
using Quartz.Impl;

namespace FineBot.Workers
{
    public class WorkerManager
    {
        private readonly IScheduler scheduler;

        public WorkerManager(IWindsorContainer container)
        {
            this.scheduler = StdSchedulerFactory.GetDefaultScheduler();
            this.scheduler.JobFactory = new WindsorJobFactory(container);
        }

        public void StartProcessing()
        {
            ScheduleAllJobs();
            scheduler.Start();
        }

        protected void ScheduleAllJobs()
        {
            ScheduleAwardFinesFromReactionJob();
            //ScheduleGreetingJob();
        }

        private void ScheduleGreetingJob()
        {
            var key = new JobKey("GreetingJob");

            var jobDetail = JobBuilder.Create<IGreetingJob>()
                .WithIdentity(key)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("GreetingTrigger")
                .StartAt(new DateTimeOffset(new DateTime(2016, 5, 5, 7, 45, 0)))
                .ForJob(jobDetail)
                .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        private void ScheduleAwardFinesFromReactionJob()
        {
            var key = new JobKey("AwardFinesFromReactionJob");

            var jobDetail = JobBuilder.Create<IAwardFinesFromReactionJob>()
                .WithIdentity(key)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("AwardFinesFromReactionTrigger")
                .StartAt(DateBuilder.TodayAt(20, 0, 0))
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
