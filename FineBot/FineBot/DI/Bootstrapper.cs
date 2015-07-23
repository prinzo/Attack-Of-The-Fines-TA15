using Castle.Windsor;
using Castle.Windsor.Installer;

namespace FineBot.DI
{
    public static class Bootstrapper
    {
        public static IWindsorContainer BootstrapContainer()
        {
            return new WindsorContainer()
               .Install(FromAssembly.This()
               );
        } 
    }
}