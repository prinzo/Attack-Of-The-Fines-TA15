using System;
using FineBot.Workers.DI;

namespace FineBot.Workers
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = WorkerBootstrapper.Init();

            var workerManager = new WorkerManager(container);

            workerManager.StartProcessing();

            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
        }
    }
}
