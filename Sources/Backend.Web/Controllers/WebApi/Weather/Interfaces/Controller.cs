using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Backend.Web.Api.Shared.Weather;

namespace Backend.Web.Controllers.WebApi.Weather.Interfaces
{
    public interface IWeatherApiController
    {
        Task<HttpResponseMessage> GetData([FromBody] GetWeatherRequest request);
    }
}