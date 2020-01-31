using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;
using Backend.Web.Core.Backend;
using Microsoft.Owin;
using Owin;
using Project.Kernel;

[assembly: OwinStartup(typeof(Backend.Web.Startup))]
namespace Backend.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var unityContainer = UnityConfig.GetConfiguredContainer();
            BaseTypeFabric.RegisterTypes<TypeFabric>(unityContainer);
            var unityControllerFactory = UnityControllerFactory.Create(unityContainer);
            ControllerBuilder.Current.SetControllerFactory(unityControllerFactory);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
                new UnityHttpControllerSelector(GlobalConfiguration.Configuration));
            CorsConfig.RegisterCors(app, GlobalConfiguration.Configuration);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}