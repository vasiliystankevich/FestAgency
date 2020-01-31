using System.IO;
using System.Web;
using Backend.Web.Controllers;
using Backend.Web.Core.Backend.Interfaces;
using Backend.Web.Core.Backend.Repositories;
using log4net;
using log4net.Config;
using Project.Kernel;
using Unity;
using Unity.Lifetime;

namespace Backend.Web.Core.Backend
{
    public class TypeFabric:BaseTypeFabric 
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterFactory<IWrapper<ILog>>(unityContainer =>
            {
                var logFileConfigPath = HttpContext.Current.Server.MapPath("~/log4net.config");
                XmlConfigurator.Configure(new FileInfo(logFileConfigPath));
                return new Wrapper<ILog>(LogManager.GetLogger(typeof(Wrapper<ILog>)));
            });

            container.RegisterFactory<IConfiguration>(unityContainer =>
            {
                var configuration = new Configuration();
                return configuration;
            }, new SingletonLifetimeManager());

            container.RegisterFactory<IAwaitTaskCreator>(unityContainer => new AwaitTaskCreator(), new SingletonLifetimeManager());
            container.RegisterType<IVersionRepository, VersionRepository>();

            container.RegisterType<IHomeRepository, HomeRepository>();
            container.RegisterType<IHomeController, HomeController>();
            BaseTypeFabric.RegisterTypes<Controllers.WebApi.TypeFabric>(container);
        }
    }
}
