using System;
using FineBot.Workers.Jobs;
using Quartz;
using Quartz.Impl;

namespace FineBot.Workers
{
    public class WorkerManager
    {
        private readonly IScheduler scheduler;

        public WorkerManager()
        {
            this.scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public void StartProcessing()
        {
            ScheduleAllJobs();
            scheduler.Start();
        }

        protected void ScheduleAllJobs()
        {
            ScheduleAwardFinesFromReactionJob();
            ScheduleGreetingJob();
        }

        private void ScheduleGreetingJob()
        {
            var key = new JobKey("GreetingJob");

            var jobDetail = JobBuilder.Create<GreetingJob>()
                .WithIdentity(key)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("GreetingTrigger")
                .StartAt(new DateTimeOffset(new DateTime(2016, 03, 13, 15, 56, 0)))
                .ForJob(jobDetail)
                .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        private void ScheduleAwardFinesFromReactionJob()
        {
            var key = new JobKey("AwardFinesFromReactionJob");

            var jobDetail = JobBuilder.Create<AwardFinesFromReactionJob>()
                .WithIdentity(key)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("AwardFinesFromReactionTrigger")
                .StartAt(DateBuilder.TomorrowAt(0, 0, 0))
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
