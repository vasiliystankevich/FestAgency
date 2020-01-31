using System;
using System.Collections.Generic;
using Backend.Web.Api.Shared.Weather;

namespace WebApi.Client.Interfaces
{
    public interface IWebApiClient : IDisposable
    {
        List<WeatherInformation> WeatherGet(string zipCode, bool metricSystem);
    }
}
