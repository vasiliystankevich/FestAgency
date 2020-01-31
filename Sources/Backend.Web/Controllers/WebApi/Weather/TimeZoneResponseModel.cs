using Google.Maps.TimeZone;

namespace Backend.Web.Controllers.WebApi.Weather
{
    public class TimeZoneResponseModel
    {
        public TimeZoneResponseModel(int cityId, TimeZoneResponse response)
        {
            CityId = cityId;
            TimeZoneId = response.TimeZoneID;
            TimeZoneName = response.TimeZoneName;
        }

        public int CityId { get; }
        public string TimeZoneId { get; }
        public string TimeZoneName { get; }
    }
}