using System;
using FineBot.API.SupportApi;
using Quartz;

namespace FineBot.Workers.Jobs
{
    public class AwardFinesFromReactionJob : BaseJob, IAwardFinesFromReactionJob
    {
        private readonly ISupportApi supportApi;

        public AwardFinesFromReactionJob(ISupportApi supportApi) : base(supportApi)
        {
            
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Award Fines From Reaction running {0}", DateTime.Now);

            try
            {

            }
            catch (Exception exception)
            {
                this.LogException(exception);
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Award Fines From Reaction disposed {0}", DateTime.Now);
        }
    }
}
