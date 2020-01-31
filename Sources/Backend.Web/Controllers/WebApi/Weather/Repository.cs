using System.Threading.Tasks;
using Backend.Web.Api.Shared.Weather;
using Backend.Web.Controllers.WebApi.Weather.Interfaces;

namespace Backend.Web.Controllers.WebApi.Weather
{
    public class WeatherApiRepository : IWeatherApiRepository
    {
        public WeatherApiRepository(IOpenWeatherMapWebClient openWeatherMapWebClient,
            IGoogleTimeZonesWebClient googleTimeZonesWebClient, IWeatherResponseFactory responseFactory)
        {
            OpenWeatherMapWebClient = openWeatherMapWebClient;
            GoogleTimeZonesWebClient = googleTimeZonesWebClient;
            ResponseFactory = responseFactory;
        }

        public async Task<GetWeatherResponse> GetWeather(GetWeatherRequest request)
        {
            var weatherItems = await OpenWeatherMapWebClient.GetWeatherItems(request);
            var googleTimeZones = await GoogleTimeZonesWebClient.GetTimeZones(weatherItems);
            return await ResponseFactory.CreateGetWeatherResponse(weatherItems, googleTimeZones);
        }

        IOpenWeatherMapWebClient OpenWeatherMapWebClient { get; }
        IGoogleTimeZonesWebClient GoogleTimeZonesWebClient { get; }
        IWeatherResponseFactory ResponseFactory { get; }
    }
}