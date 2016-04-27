using System;
using FineBot.API.ChatApi;
using Quartz;
using System.Configuration;

namespace FineBot.Workers.Jobs
{
    public class GreetingJob : IGreetingJob
    {
        private readonly IChatApi chatApi;

        public GreetingJob(IChatApi chatApi)
        {
            this.chatApi = chatApi;
        }

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Greeting job running {0}", DateTime.Now);
            chatApi.PostMessage(ConfigurationManager.AppSettings["BotKey"], "#random", "Ain't this a might a fine day!");
        }

        public void Dispose()
        {
            Console.WriteLine("Greeting job disposed {0}", DateTime.Now);
        }
    }
}
