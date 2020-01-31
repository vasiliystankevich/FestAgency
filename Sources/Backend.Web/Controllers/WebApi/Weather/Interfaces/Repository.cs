using System.Threading.Tasks;
using Backend.Web.Api.Shared.Weather;

namespace Backend.Web.Controllers.WebApi.Weather.Interfaces
{
    public interface IWeatherApiRepository
    {
        Task<GetWeatherResponse> GetWeather(GetWeatherRequest request);
    }
}