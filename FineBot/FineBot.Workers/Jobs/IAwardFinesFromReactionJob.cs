using System;
using Quartz;

namespace FineBot.Workers.Jobs
{
    public interface IAwardFinesFromReactionJob : IJob, IDisposable
    {
    }
}