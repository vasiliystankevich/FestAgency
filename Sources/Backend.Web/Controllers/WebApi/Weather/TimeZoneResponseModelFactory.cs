using Backend.Web.Controllers.WebApi.Weather.Interfaces;
using Google.Maps.TimeZone;

namespace Backend.Web.Controllers.WebApi.Weather
{
    public class TimeZoneResponseModelFactory: ITimeZoneResponseModelFactory
    {
        public TimeZoneResponseModel Create(int cityId, TimeZoneResponse response)
        {
            return new TimeZoneResponseModel(cityId, response);
        }
    }
}