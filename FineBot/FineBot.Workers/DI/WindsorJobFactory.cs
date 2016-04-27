using System;
using Castle.Windsor;
using Quartz;
using Quartz.Spi;

namespace FineBot.Workers.DI
{
    public class WindsorJobFactory : IJobFactory
    {
        private readonly IWindsorContainer container;

        public WindsorJobFactory(IWindsorContainer container)
        {
            this.container = container;
        }

       public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
       {
           IJob newJob;
           try
           {
               newJob = (IJob)container.Resolve(bundle.JobDetail.JobType);
           }
           catch (Exception ex)
           {
                Console.WriteLine(ex.Message);   
               throw;
           }
           return newJob;
       }

        public void ReturnJob(IJob job)
        {
            this.container.Release(job);
        }
    }
}