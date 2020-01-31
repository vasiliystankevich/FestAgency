using System.Collections.Generic;
using Backend.Web.Api.Shared.Base;

namespace Backend.Web.Api.Shared.Weather
{
    public class GetWeatherResponse : OkResponse
    {
        public GetWeatherResponse(List<WeatherInformation> information)
        {
            Information = information;
        }

        public List<WeatherInformation> Information { get; }
    }
}