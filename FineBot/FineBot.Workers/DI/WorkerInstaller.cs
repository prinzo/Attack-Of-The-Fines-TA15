using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace FineBot.Workers.DI
{
    public class WorkerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().Pick().WithService.DefaultInterfaces().LifestyleTransient());
        }
    }
}
