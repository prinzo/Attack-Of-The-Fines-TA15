using System;
using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FineBot.DataAccess.BaseClasses;
using FineBot.Interfaces;
using MongoDB.Driver;

namespace FineBot.DataAccess.DI
{
    public class DataAccessContainerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            
            container.Register(
                Component.For<IMongoClient>().ImplementedBy<MongoClient>().LifestyleSingleton()
                         //.UsingFactoryMethod(() => new MongoClient("<ConnectionStringGoesHere>"))
            );

            if(Convert.ToBoolean(ConfigurationManager.AppSettings["PersistData"] ?? "false"))
            {
                container.Register(Component.For(typeof(IRepository<,>)).ImplementedBy(typeof(MongoRepository<,>)).LifestyleTransient());    
            }
            else
            {
                container.Register(Component.For(typeof(IRepository<,>)).ImplementedBy(typeof(MemoryRepository<,>)).LifestyleSingleton());    
            }
        }
    }
}