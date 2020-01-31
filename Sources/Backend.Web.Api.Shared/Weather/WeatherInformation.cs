using System;

namespace Backend.Web.Api.Shared.Weather
{
    public class WeatherInformation
    {
        public WeatherInformation(string city, string timeZoneId, string timeZoneName, double temperature)
        {
            City = city;
            TimeZoneId = timeZoneId;
            TimeZoneName = timeZoneName;
            Temperature = temperature;
        }

        public string City { get; }
        public string TimeZoneId { get; }
        public string TimeZoneName { get; }
        public double Temperature { get; }
    }
}
