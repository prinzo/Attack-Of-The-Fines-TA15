using System.Web.Http;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace FineBot.WepApi.DI
{
    public class WebApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
             Classes
                 .FromThisAssembly()
                 .BasedOn<ApiController>()
                 .LifestylePerWebRequest()
             );

            container.Register(
            Classes
                .FromThisAssembly()
                .BasedOn<Controller>()
                .LifestylePerWebRequest()
            );

            container.Register(
              Classes.FromThisAssembly().Pick()
                  .WithService.DefaultInterfaces()
              );
        }
    }
}