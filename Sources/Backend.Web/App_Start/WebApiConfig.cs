using System.Net.Http.Extensions.Compression.Core.Compressors;
using System.Web.Http;
using Microsoft.AspNet.WebApi.Extensions.Compression.Server;
using Project.Kernel;
using Unity.WebApi;

namespace Backend.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/1.0/{controller}/{action}/{request}",
                new { request = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
            var container = UnityConfig.GetConfiguredContainer();
            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
