using System;
using FineBot.API.ChatApi;
using Quartz;
using System.Configuration;
using FineBot.API.SupportApi;

namespace FineBot.Workers.Jobs
{
    public class GreetingJob : BaseJob, IGreetingJob
    {
        private readonly IChatApi chatApi;
        private readonly ISupportApi supportApi;

        public GreetingJob(IChatApi chatApi, ISupportApi supportApi) : base(supportApi)
        {
            this.chatApi = chatApi;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Greeting job running {0}", DateTime.Now);

            try
            {
                chatApi.PostMessage(ConfigurationManager.AppSettings["BotKey"], "#random","Well, isn't this a  :fine:  day :dancing-monkey:");
            }
            catch (Exception exception)
            {
                var message = this.LogException(exception);
                Console.WriteLine(message + ": " + exception.Message);
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Greeting job disposed {0}", DateTime.Now);
        }
    }
}
