using System;
using FineBot.API.SupportApi;
using FineBot.Enums;

namespace FineBot.Workers.Jobs
{
    public class BaseJob
    {
        protected readonly ISupportApi supportApi;

        public BaseJob(ISupportApi supportApi)
        {
            this.supportApi = supportApi;
        }

        protected string LogException(Exception exception)
        {
            this.supportApi.CreateSupportTicketOnTrello(new SupportTicketModel
            {
                Message = exception.ToString(),
                Subject = "Exception in Bot Worker",
                Status = (int) Status.Open,
                Type = (int) SupportType.Bug
            });

            return string.Format("{0} - Exception in Bot Worker", DateTime.Now.ToString("F"));
        }
    }
}
