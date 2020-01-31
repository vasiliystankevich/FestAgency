using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Web.Controllers.WebApi.Weather.Interfaces;
using Backend.Web.Core.Backend.Interfaces;
using Google.Maps;
using Google.Maps.TimeZone;
using OpenWeatherMap;

namespace Backend.Web.Controllers.WebApi.Weather
{
    public class GoogleTimeZonesWebClient : IGoogleTimeZonesWebClient
    {
        public GoogleTimeZonesWebClient(IConfiguration configuration, ITimeZoneResponseModelFactory timeZoneResponseModelFactory, IAwaitTaskCreator awaitTaskCreator)
        {
            HttpClientTimeout = configuration.GeTimeSpan("httpClient:Timeout");
            TimeZoneResponseModelFactory = timeZoneResponseModelFactory;
            AwaitTaskCreator = awaitTaskCreator;
        }

        public async Task<List<TimeZoneResponseModel>> GetTimeZones(List<WeatherItem> weatherItems) =>
            await AwaitTaskCreator.Create(weatherItems, ResultGenerator);

        List<TimeZoneResponseModel> ResultGenerator(List<WeatherItem> weatherItems)
        {
            var googleTimeZoneTasks = weatherItems.Select(item =>
            {
                return Task<TimeZoneResponseModel>.Factory.StartNew(() =>
                {
                    var req = new TimeZoneRequest
                    {
                        Location = new LatLng(item.City.Coordinates.Latitude, item.City.Coordinates.Longitude)
                    };
                    var response = new TimeZoneService().GetResponse(req);
                    return TimeZoneResponseModelFactory.Create(item.City.Id, response);
                });
            }).ToArray();
            Task.WaitAll(googleTimeZoneTasks, HttpClientTimeout);
            return googleTimeZoneTasks.Select(task => task.Result).ToList();
        }

        TimeSpan HttpClientTimeout { get; }
        ITimeZoneResponseModelFactory TimeZoneResponseModelFactory { get; }
        IAwaitTaskCreator AwaitTaskCreator { get; }
    }
}