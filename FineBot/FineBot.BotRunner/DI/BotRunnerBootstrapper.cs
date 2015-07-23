using Castle.Windsor;
using Castle.Windsor.Installer;
using FineBot.API.DI;

namespace FineBot.BotRunner.DI
{
    public static class BotRunnerBootstrapper
    {
        public static IWindsorContainer Init()
        {
            var container = API.DI.Bootstrapper.Init();

            container.Install(FromAssembly.Containing<ApiInstaller>());

            container.Install(FromAssembly.This());

            return container;
        }
    }
}