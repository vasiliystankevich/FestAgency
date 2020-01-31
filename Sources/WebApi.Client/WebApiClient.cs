using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using Backend.Web.Api.Shared.Base;
using Backend.Web.Api.Shared.Weather;
using Newtonsoft.Json;
using WebApi.Client.Interfaces;

namespace WebApi.Client
{
    public class WebApiClient: IWebApiClient
    {
        public WebApiClient(Uri baseUri)
        {
            var webApiActionWeatherGet = WebConfigurationManager.AppSettings["api:1.0:weather:getdata"];
            var webApiClientTimeout = Convert.ToInt32(WebConfigurationManager.AppSettings["http:client:timeout"]);

            UriActions = new Dictionary<string, Uri>
            {
                {nameof(WeatherGet).ToLowerInvariant(), new Uri(baseUri, webApiActionWeatherGet)}
            };

            Client = new HttpClient {Timeout = new TimeSpan(0, 0, 0, webApiClientTimeout) };
        }

        public List<WeatherInformation> WeatherGet(string zipCode, bool metricSystem)
        {
            var request = new GetWeatherRequest(zipCode, metricSystem);
            var response = ExecuteRequest<GetWeatherRequest, GetWeatherResponse>(request, nameof(WeatherGet));
            return response.Information;
        }

        public TResponse ExecuteRequest<TRequest, TResponse>(TRequest request, string nameAction)
            where TRequest : BaseRequest
            where TResponse : BaseResponse
        {
            var expectedTask = Task<TResponse>.Factory.StartNew(() =>
            {
                var innerTask = ExecuteRequestAsync<TRequest, TResponse>(request, nameAction);
                innerTask.Wait();
                return innerTask.Result;
            });
            expectedTask.Wait();
            return expectedTask.Result;
        }

        public async Task<TResponse> ExecuteRequestAsync<TRequest, TResponse>(TRequest request, string nameAction)
            where TRequest : BaseRequest
            where TResponse : BaseResponse
        {
            HttpResponseMessage response = null;
            var content = string.Empty;
            try
            {
                response = await Client.PostAsJsonAsync(UriActions[nameAction.ToLowerInvariant()], request);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                throw new WebApiException(BaseResponseStatus.Create(Convert.ToInt32(response.StatusCode), content));
            }
            catch (Exception exception)
            {
                throw new WebApiException(BaseResponseStatus.Create(500, exception.Message));
            }
            var message = JsonConvert.DeserializeObject<BaseResponse>(content);
            if (message.Status.Code != 200) throw new WebApiException(message.Status);
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public void Dispose()
        {
            Client?.Dispose();
        }

        private HttpClient Client { get; }
        private Dictionary<string, Uri> UriActions { get; }
    }
}
