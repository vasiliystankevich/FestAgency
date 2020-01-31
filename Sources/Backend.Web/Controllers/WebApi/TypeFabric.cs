using Backend.Web.Controllers.WebApi.Weather;
using Backend.Web.Controllers.WebApi.Weather.Interfaces;
using Backend.Web.Core.Backend.Interfaces;
using Google.Maps;
using OpenWeatherMap;
using Project.Kernel;
using Unity;

namespace Backend.Web.Controllers.WebApi
{
    public class TypeFabric: BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterFactory<IWrapper<OpenWeatherMapClient>>(unityContainer =>
            {
                var config = container.Resolve<IConfiguration>();
                var openWeatherMapAppId = config.GetGuid("openWeatherMap:AppId");
                var client = new OpenWeatherMapClient(openWeatherMapAppId.ToString("N"));
                return new Wrapper<OpenWeatherMapClient>(client);
            });

            container.RegisterType<IOpenWeatherMapWebClient, OpenWeatherMapWebClient>();
            container.RegisterType<IGoogleTimeZonesWebClient, GoogleTimeZonesWebClient>();
            container.RegisterType<ITimeZoneResponseModelFactory, TimeZoneResponseModelFactory>();
            container.RegisterType<IWeatherResponseFactory, WeatherResponseFactory>();
            container.RegisterType<IWeatherApiRepository, WeatherApiRepository>();
            container.RegisterType<IWeatherApiController, WeatherApiController>();
            SignedAllGoogleServices(container);
        }

        void SignedAllGoogleServices(IUnityContainer container)
        {
            var configuration = container.Resolve<IConfiguration>();
            var gooleApiKey = configuration.GetString("google:ApiKey");
            GoogleSigned.AssignAllServices(new GoogleSigned(gooleApiKey));
        }
    }
}