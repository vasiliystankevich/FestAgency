using Google.Maps.TimeZone;

namespace Backend.Web.Controllers.WebApi.Weather.Interfaces
{
    public interface ITimeZoneResponseModelFactory
    {
        TimeZoneResponseModel Create(int cityId, TimeZoneResponse response);
    }
}
