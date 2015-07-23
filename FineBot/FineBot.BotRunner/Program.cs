using System;
using System.Configuration;
using Castle.Windsor;
using FineBot.BotRunner.DI;
using MargieBot;
using MargieBot.Responders;

namespace FineBot.BotRunner
{
    class Program
    {
        private static IWindsorContainer container;

        static void Main(string[] args)
        {
            container = BotRunnerBootstrapper.Init();
            
            Bot bot = new Bot();

            foreach(IResponder responder in container.ResolveAll<IResponder>())
            {
                bot.Responders.Add(responder);    
            }

            bot.RespondsTo("hi").IfBotIsMentioned().With("Stop resisting citizen!");

            var task = bot.Connect(ConfigurationManager.AppSettings["SlackApiKey"]);

            Console.WriteLine(String.Format("{0}: Bot is runnning, type 'die' to make it die", DateTime.Now));

            while(Console.ReadLine() != "die")
            {
                
            }

            container.Dispose();
        }
    }
}
