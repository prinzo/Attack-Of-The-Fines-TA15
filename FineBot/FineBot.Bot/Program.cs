using Castle.Windsor;
using FineBot.DI;

namespace FineBot.Bot
{
    class Program
    {
        private static IWindsorContainer container;

        static void Main(string[] args)
        {
            container = Bootstrapper.BootstrapContainer();

            var mainBot = new MargieBot.Bot();

            mainBot.
        }
    }
}
