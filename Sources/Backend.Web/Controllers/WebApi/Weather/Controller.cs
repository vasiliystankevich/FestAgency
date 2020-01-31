using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Backend.Web.Api.Shared.Weather;
using Backend.Web.Controllers.WebApi.Weather.Interfaces;
using Backend.Web.Core.Backend.Interfaces;
using Backend.Web.Core.Backend.WebApi;
using log4net;
using Project.Kernel;

namespace Backend.Web.Controllers.WebApi.Weather
{
    [RoutePrefix("weather")]
    public class WeatherApiController : BaseApiController<IWeatherApiRepository>, IWeatherApiController
    {
        public WeatherApiController(IWeatherApiRepository repository, IVersionRepository versionRepository, IWrapper<ILog> logger) : base(repository, versionRepository, logger)
        {
        }

        [Route("getdata")]
        [HttpPost]
        public async Task<HttpResponseMessage> GetData([FromBody] GetWeatherRequest request)
        {
            return await ExecuteAction(request, Repository.GetWeather);
        }
    }
}