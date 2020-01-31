using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Web.Api.Shared.Weather;
using OpenWeatherMap;

namespace Backend.Web.Controllers.WebApi.Weather.Interfaces
{
    public interface IWeatherResponseFactory
    {
        Task<GetWeatherResponse> CreateGetWeatherResponse(List<WeatherItem> weatherItems, List<TimeZoneResponseModel> timeZones);
    }
}