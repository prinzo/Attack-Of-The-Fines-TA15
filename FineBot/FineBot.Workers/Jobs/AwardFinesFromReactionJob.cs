﻿using System;
using System.Threading.Tasks;
using FineBot.API.FinesApi;
using FineBot.API.SupportApi;
using FineBot.Enums;
using Quartz;

namespace FineBot.Workers.Jobs
{
    public class AwardFinesFromReactionJob : BaseJob, IAwardFinesFromReactionJob
    {
        private readonly IFineApi fineApi;
        private readonly ISupportApi supportApi;

        public AwardFinesFromReactionJob(IFineApi fineApi, ISupportApi supportApi) : base(supportApi)
        {
            this.fineApi = fineApi;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Award Fines From Reaction running {0}", DateTime.Now);

            try
            {
                var startTime = DateTime.Today;
                fineApi.IssueFinesFromReactions(startTime, ChatRoomType.Channel);
                fineApi.IssueFinesFromReactions(startTime, ChatRoomType.Group);
            }
            catch (Exception exception)
            {
                var message = this.LogException(exception);
                Console.WriteLine(message + ": " + exception.Message);
            }
            return null;
        }

        public void Dispose()
        {
            Console.WriteLine("Award Fines From Reaction disposed {0}", DateTime.Now);
        }
    }
}
