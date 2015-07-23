using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FineBot.DataAccess.BaseClasses;
using FineBot.Interfaces;

namespace FineBot.DataAccess.DI
{
    public class DataAccessContainerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IRepository<,>)).ImplementedBy(typeof(MemoryRepository<,>)).LifestyleSingleton()
                );
            //container.Register(
            //    Component.For(typeof(IRepository<,>)).ImplementedBy(typeof(Repository<,>)).LifestyleSingleton(),
            //    Component.For<IUnitOfWorkFactory>().ImplementedBy<UnitOfWorkFactory>()
            //             .UsingFactoryMethod(() => new UnitOfWorkFactory("<ConnectionStringGoesHere>")),
            //    Component.For<IUnitOfWork>().ImplementedBy<UnitOfWork>()
            //             .UsingFactoryMethod(k => k.Resolve<IUnitOfWorkFactory>().CreateUnitOfWork())
            //             .LifestylePerWebRequest()
            // );
            
        }
    }
}