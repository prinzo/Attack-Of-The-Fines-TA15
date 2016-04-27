using System;
using Quartz;

namespace FineBot.Workers.Jobs
{
    public class AwardFinesFromReactionJob : IAwardFinesFromReactionJob
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
