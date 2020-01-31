using DataGenerator.Interfaces;
using Project.Kernel;
using Unity;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = UnityConfig.GetConfiguredContainer();
            BaseTypeFabric.RegisterTypes<TypeFabric>(container);
            var startup = container.Resolve<IStartup>();
            startup.Execute();
        }
    }
}
