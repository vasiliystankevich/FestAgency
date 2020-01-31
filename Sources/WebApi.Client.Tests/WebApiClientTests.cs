using System;
using System.Collections.Generic;
using System.Configuration;
using Backend.Web.Api.Shared.Weather;
using Newtonsoft.Json;
using Tests.AAAPattern.xUnit.Attributes;
using Xunit;

namespace WebApi.Client.Tests
{
    public class WebApiClientTests
    {
        [Theory]
        [MoqInlineAutoData("api:base:uri", "61000", true, "[{\"City\":\"Le Mans\",\"TimeZoneId\":\"Europe/Paris\",\"TimeZoneName\":\"GMT+00:09:21\"},{\"City\":\"Chilaw\",\"TimeZoneId\":\"Asia/Colombo\",\"TimeZoneName\":\"GMT+05:19:24\"},{\"City\":\"San Juan del Rio\",\"TimeZoneId\":\"America/Mexico_City\",\"TimeZoneName\":\"GMT-06:36:36\"}]")]
        [MoqInlineAutoData("api:base:uri", "61000", false, "[{\"City\":\"Le Mans\",\"TimeZoneId\":\"Europe/Paris\",\"TimeZoneName\":\"GMT+00:09:21\"},{\"City\":\"Chilaw\",\"TimeZoneId\":\"Asia/Colombo\",\"TimeZoneName\":\"GMT+05:19:24\"},{\"City\":\"San Juan del Rio\",\"TimeZoneId\":\"America/Mexico_City\",\"TimeZoneName\":\"GMT-06:36:36\"}]")]
        public void WeatherGetAvailabilityTest(string baseUriKey, string zipCode, bool metricSystem, string jsonActual)
        {
            //arrange
            var actual = JsonConvert.DeserializeObject<List<WeatherInformation>>(jsonActual);
            var baseUriValue = ConfigurationManager.AppSettings[baseUriKey];
            var baseUri = new Uri(baseUriValue);
            var sut = new WebApiClient(baseUri);
            
            //act
            var expected = sut.WeatherGet(zipCode, metricSystem);

            //assert
            for (var index = 0; index < expected.Count; index++)
                WeatherItemsCompare(expected[index], actual[index]);
        }

        void WeatherItemsCompare(WeatherInformation expected, WeatherInformation actual)
        {
            Assert.Equal(expected.City, actual.City);
            Assert.Equal(expected.TimeZoneId, actual.TimeZoneId);
            Assert.Equal(expected.TimeZoneName, actual.TimeZoneName);
        }
    }
}