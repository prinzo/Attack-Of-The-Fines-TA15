﻿using System.Web.Http;
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
                 .LifestyleScoped()
             );
        }
    }
}