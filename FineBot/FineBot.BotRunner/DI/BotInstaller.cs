using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FineBot.BotRunner.Responders;
using FineBot.BotRunner.Responders.Interfaces;
using MargieBot.Responders;

namespace FineBot.BotRunner.DI
{
    public class BotInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(
            //    Classes.FromThisAssembly().Pick()
            //        .WithService.DefaultInterfaces()
            //    );

            container.Register(Component.For<IFineBotResponder>().ImplementedBy<GiveFineResponder>());
            container.Register(Component.For<IFineBotResponder>().ImplementedBy<HelpResponder>());
            container.Register(Component.For<IFineBotResponder>().ImplementedBy<LeaderBoardResponder>());
            container.Register(Component.For<IFineBotResponder>().ImplementedBy<PayFineResponder>());
            container.Register(Component.For<IFineBotResponder>().ImplementedBy<SeconderResponder>());
            container.Register(Component.For<IFineBotResponder>().ImplementedBy<ShowResponder>());
            container.Register(Component.For<IFineBotResponder>().ImplementedBy<CommonReplyResponder>());
            container.Register(Component.For<IFineBotResponder>().ImplementedBy<YouTubeResponder>());
            container.Register(Component.For<ISecondCousinResponder>().ImplementedBy<SecondAutofineResponder>());
            container.Register(Component.For<IFineBotResponder>().ImplementedBy<FineCountResponder>());
        }
    }
}