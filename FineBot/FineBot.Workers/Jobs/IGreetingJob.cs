using System;
using Quartz;

namespace FineBot.Workers.Jobs
{
    public interface IGreetingJob : IJob, IDisposable
    {
    }
}
