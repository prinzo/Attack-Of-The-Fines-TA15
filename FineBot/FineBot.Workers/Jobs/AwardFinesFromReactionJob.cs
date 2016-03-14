using System;
using Quartz;

namespace FineBot.Workers.Jobs
{
    public class AwardFinesFromReactionJob : IJob, IDisposable
    {
        public AwardFinesFromReactionJob()
        {
            
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Award Fines From Reaction running {0}", DateTime.Now);
        }

        public void Dispose()
        {
            Console.WriteLine("Award Fines From Reaction disposed {0}", DateTime.Now);
        }
    }
}
