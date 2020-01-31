using System.Collections.Generic;
using System.Threading.Tasks;
using OpenWeatherMap;

namespace Backend.Web.Controllers.WebApi.Weather.Interfaces
{
    public interface IGoogleTimeZonesWebClient
    {
        Task<List<TimeZoneResponseModel>> GetTimeZones(List<WeatherItem> weatherItems);
    }
}
