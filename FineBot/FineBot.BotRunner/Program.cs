using System;
using System.Configuration;
using Castle.Windsor;
using FineBot.BotRunner.DI;
using MargieBot;
using MargieBot.Responders;

namespace FineBot.BotRunner
{
    public class Program
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
            bot.CreateResponder(x => !x.BotHasResponded, rc => "My responses are limited, you must ask the right question...");

            var task = bot.Connect(ConfigurationManager.AppSettings["BotKey"]);

            Console.WriteLine(String.Format("{0}: Bot is runnning, type 'die' to make it die", DateTime.Now));
            
            var seconderbot = new Bot();
            var seconderTask = seconderbot.Connect(ConfigurationManager.AppSettings["SeconderBotKey"]);

            Console.WriteLine(String.Format("{0}: Finebot's second cousin is also also running. Some say he can't die.", DateTime.Now));

            while(Console.ReadLine() != "die")
            {
                
            }

            container.Dispose();
        }

    }
}
