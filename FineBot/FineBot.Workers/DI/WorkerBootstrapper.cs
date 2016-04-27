using Castle.Windsor;
using Castle.Windsor.Installer;
using FineBot.API.DI;

namespace FineBot.Workers.DI
{
    public class WorkerBootstrapper
    {
        public static IWindsorContainer Init()
        {
            var container = Bootstrapper.Init();

            container.Install(FromAssembly.This());

            return container;
        }
    }
}
