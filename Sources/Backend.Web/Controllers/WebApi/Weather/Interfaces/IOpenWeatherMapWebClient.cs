using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Web.Api.Shared.Weather;
using OpenWeatherMap;

namespace Backend.Web.Controllers.WebApi.Weather.Interfaces
{
    public interface IOpenWeatherMapWebClient
    {
        Task<List<WeatherItem>> GetWeatherItems(GetWeatherRequest request);
    }
}
