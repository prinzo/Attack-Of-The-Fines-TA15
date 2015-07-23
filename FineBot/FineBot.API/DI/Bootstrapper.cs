using Castle.Windsor;
using Castle.Windsor.Installer;
using FineBot.DataAccess.DI;

namespace FineBot.API.DI
{
    public static class Bootstrapper
    {
        public static IWindsorContainer Init()
        {
            var container = FineBot.DI.Bootstrapper.BootstrapContainer();

            container.Install(FromAssembly.This());

            container.Install(FromAssembly.Containing<DataAccessContainerInstaller>());

            return container;
        }
    }
}