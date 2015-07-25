using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using FineBot.API.DI;
using FineBot.API.UsersApi;
using FineBot.DataAccess.BaseClasses;
using FineBot.Entities;
using FineBot.Interfaces;
using FineBot.WepApi.DI;

namespace FineBot.WepApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication
    {
        private static IWindsorContainer container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ConfigureWindsor(GlobalConfiguration.Configuration);
            WebApiConfig.Register(GlobalConfiguration.Configuration,container);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        protected void Application_End()
        {
            container.Dispose();
            base.Dispose();
        }


        private static void BootstrapContainer()
        {
            container = Bootstrapper.Init();
            container.Install(FromAssembly.This());

            var controllerFactory = new WindsorControllerFactory(container.Kernel);

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            var dependencyResolver = new WindsorDependencyResolver(container);
           
        }

        public static void ConfigureWindsor(HttpConfiguration configuration)
        {
            container =Bootstrapper.Init();

            container.Install(FromAssembly.This());
    

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, true));
            var dependencyResolver = new WindsorDependencyResolver(container);
            configuration.DependencyResolver = dependencyResolver;
        }
    }
}