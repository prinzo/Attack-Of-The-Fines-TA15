using System;
using System.Configuration;
using Castle.Windsor;
using FineBot.API.UsersApi;
using FineBot.BotRunner.DI;
using FineBot.BotRunner.Responders.Interfaces;
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

            var fineBot = new Bot();
            var fineBotResponders = container.ResolveAll<IFineBotResponder>();
            foreach (IFineBotResponder responder in fineBotResponders)
            {
                fineBot.Responders.Add(responder);
            }

            fineBot.RespondsTo("hi").IfBotIsMentioned().With("Stop resisting citizen!");
            fineBot.CreateResponder(x => !x.BotHasResponded, rc => "My responses are limited, you must ask the right question...");

            var task = fineBot.Connect(ConfigurationManager.AppSettings["BotKey"]);
            Console.WriteLine(String.Format("{0}: Bot is runnning, type 'die' to make it die", DateTime.Now));
            
            var secondCousinBot = new Bot();
            var secondCousinResponders = container.ResolveAll<ISecondCousinResponder>();
            foreach (ISecondCousinResponder responder in secondCousinResponders)
            {
                secondCousinBot.Responders.Add(responder);
            }
            var seconderTask = secondCousinBot.Connect(ConfigurationManager.AppSettings["SeconderBotKey"]);
            Console.WriteLine(String.Format("{0}: Finebot's second cousin is also also running. Some say he can't die.", DateTime.Now));

            while(Console.ReadLine() != "die")
            {
                
            }

            container.Dispose();
        }

    }
}
