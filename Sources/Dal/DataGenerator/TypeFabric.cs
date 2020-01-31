using System.IO;
using DataGenerator.Interfaces;
using log4net;
using log4net.Config;
using Logs;
using Project.Kernel;
using Unity;

namespace DataGenerator
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterFactory<IWrapper<ILog>>(unityContainer =>
            {
                var logFileConfigPath = "log4net.config";
                XmlConfigurator.Configure(new FileInfo(logFileConfigPath));
                return new Wrapper<ILog>(LogManager.GetLogger(typeof(Wrapper<ILog>)));
            });
            container.RegisterFactory<ISaContext>(unityContainer => new SaContext());
            container.RegisterFactory<ILogsContext>(unityContainer => new LogsContext());
            container.RegisterType<IDataLoader, DataLoader>();
            container.RegisterType<IExecutor, Executor>();
            container.RegisterType<IStartup, Startup>();
        }
    }
}
