using System;
using Castle.Windsor;
using FineBot.Workers.DI;
using Quartz;

namespace FineBot.Workers
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = WorkerBootstrapper.Init();

            var workerManager = container.Resolve<WorkerManager>();

            workerManager.StartProcessing();

            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
        }
    }
}
