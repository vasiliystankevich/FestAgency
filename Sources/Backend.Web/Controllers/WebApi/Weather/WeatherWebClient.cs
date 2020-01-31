using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Web.Api.Shared.Weather;
using Backend.Web.Controllers.WebApi.Weather.Interfaces;
using Backend.Web.Core.Backend.Interfaces;
using OpenWeatherMap;
using Project.Kernel;

namespace Backend.Web.Controllers.WebApi.Weather
{
    public class OpenWeatherMapWebClient: IOpenWeatherMapWebClient
    {
        public OpenWeatherMapWebClient(IConfiguration configuration, IWrapper<OpenWeatherMapClient> internalClient, IAwaitTaskCreator awaitTaskCreator)
        {
            HttpClientTimeout = configuration.GeTimeSpan("httpClient:Timeout");
            InternalClient = internalClient;
            AwaitTaskCreator = awaitTaskCreator;
        }
        public async Task<List<WeatherItem>> GetWeatherItems(GetWeatherRequest request) => await AwaitTaskCreator.Create(request, ResultGenerator);

        List<WeatherItem> ResultGenerator(GetWeatherRequest request)
        {
            var metricSystemValue = request.MetricSystem ? MetricSystem.Imperial : MetricSystem.Metric;
            var searchResponse = InternalClient.Instance.Search.GetByName(request.ZipCode, metricSystemValue);
            searchResponse.Wait(HttpClientTimeout);
            return searchResponse.Result.List.GroupBy(item => item.City.Id)
                .Select(groupByElement => groupByElement.First()).ToList();
        }

        TimeSpan HttpClientTimeout { get; }
        IWrapper<OpenWeatherMapClient> InternalClient { get; }
        IAwaitTaskCreator AwaitTaskCreator { get; }
    }
}