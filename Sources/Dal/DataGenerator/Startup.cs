using System;
using DataGenerator.Interfaces;
using Project.Kernel;

namespace DataGenerator
{
    public class Startup: IStartup
    {
        public Startup(IExecutor executor, IDataLoader dataLoader)
        {
            Executor = executor;
            DataLoader = dataLoader;
        }

        public void Execute()
        {
            Executor.ExecuteAction(DataLoader.Seed);
            Console.WriteLine("Press any key for exit...");
            Console.ReadKey(true);
        }

        IExecutor Executor { get; }
        IDataLoader DataLoader { get; }
    }
}
