using System;
using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FineBot.DataAccess.BaseClasses;
using FineBot.DataAccess.DataModels;
using FineBot.DataAccess.Mappers;
using FineBot.DataAccess.Mappers.Interfaces;
using FineBot.DataAccess.Mappings;
using FineBot.Entities;
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

            container.Register(Component.For(typeof(IDataModelMapper<UserDataModel,User>)).ImplementedBy(typeof(UserDataModelMapper)).LifestyleTransient());
            container.Register(Component.For(typeof(IDataModelMapper<FineDataModel,Fine>)).ImplementedBy(typeof(FineDataModelMapper)).LifestyleTransient());
            container.Register(Component.For(typeof(IDataModelMapper<SupportTicketDataModel, SupportTicket>)).ImplementedBy(typeof(SupportTicketDataModelMapper)).LifestyleTransient());


            if (Convert.ToBoolean(ConfigurationManager.AppSettings["PersistData"] ?? "false"))
            {
                container.Register(Component.For(typeof(IRepository<,,>)).ImplementedBy(typeof(MongoRepository<,,>)).LifestyleTransient());    
                MongoMappings.SetupMappings();
            }
            else
            {
                container.Register(Component.For(typeof(IRepository<,,>)).ImplementedBy(typeof(MemoryRepository<,,>)).LifestyleSingleton());    
            }
        }
    }
}