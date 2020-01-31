using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Web.Api.Shared.Weather;
using Backend.Web.Controllers.WebApi.Weather.Interfaces;
using Backend.Web.Core.Backend.Interfaces;
using OpenWeatherMap;

namespace Backend.Web.Controllers.WebApi.Weather
{
    public class WeatherResponseFactory : IWeatherResponseFactory
    {
        public WeatherResponseFactory(IAwaitTaskCreator awaitTaskCreator)
        {
            AwaitTaskCreator = awaitTaskCreator;
        }

        public async Task<GetWeatherResponse> CreateGetWeatherResponse(List<WeatherItem> weatherItems,
            List<TimeZoneResponseModel> timeZones) =>
            await AwaitTaskCreator.Create(weatherItems, timeZones, ResultGenerator);       

        GetWeatherResponse ResultGenerator(List<WeatherItem> weatherItems, List<TimeZoneResponseModel> timeZones)
        {
            var information = weatherItems.Join(timeZones, item => item.City.Id, model => model.CityId,
                (item, model) => new WeatherInformation(item.City.Name, model.TimeZoneId, model.TimeZoneName,
                    item.Temperature.Value)).ToList();
            return new GetWeatherResponse(information);
        }

        IAwaitTaskCreator AwaitTaskCreator { get; }
    }
}