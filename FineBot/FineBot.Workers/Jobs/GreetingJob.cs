using System;
using Quartz;

namespace FineBot.Workers.Jobs
{
    public class GreetingJob : IJob, IDisposable
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Greeting job running {0}", DateTime.Now);
        }

        public void Dispose()
        {
            Console.WriteLine("Greeting job disposed {0}", DateTime.Now);
        }
    }
}
