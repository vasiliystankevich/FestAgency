using Backend.Web.Api.Shared.Base;

namespace Backend.Web.Api.Shared.Weather
{
    public class GetWeatherRequest : BaseRequest
    {
        public GetWeatherRequest(string zipCode, bool metricSystem)
        {
            ZipCode = zipCode;
            MetricSystem = metricSystem;
        }

        public string ZipCode { get; }
        public bool MetricSystem { get; }
    }
}