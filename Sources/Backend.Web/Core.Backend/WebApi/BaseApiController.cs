using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Backend.Web.Api.Shared.Base;
using Backend.Web.Core.Backend.Interfaces;
using log4net;
using Newtonsoft.Json;
using Project.Kernel;
using Unity.Interception.Utilities;

namespace Backend.Web.Core.Backend.WebApi
{
    public class BaseApiController<TRepository>:ApiController, IRepository<TRepository>
    {
        public BaseApiController(TRepository repository, IVersionRepository versionRepository, IWrapper<ILog> logger)
        {
            Repository = repository;
            Logger = logger;
            VersionRepository = versionRepository;
            IsVisibleExceptionRemoute = Convert.ToBoolean(ConfigurationManager.AppSettings["IsVisibleExceptionRemoute"]);
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            Thread.CurrentThread.CurrentCulture =
                Thread.CurrentThread.CurrentUICulture =
                    new CultureInfo("en-US");
            base.Initialize(controllerContext);
        }

        public async Task<HttpResponseMessage> ContentGenerator<T>(T model)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            var content = JsonConvert.SerializeObject(model);
            response.Content = new StringContent(content, Encoding.UTF8, "application/json");
            return await Task<HttpResponseMessage>.Factory.StartNew(() => response);
        }

        public async Task<HttpResponseMessage> ExecuteAction<TRequest, TResponse>(TRequest request, Func<TRequest, Task<TResponse>> action) where TRequest:BaseRequest
        {
            Task<HttpResponseMessage> result;
            try
            {
                if (string.Compare(request.Version, VersionRepository.Version,
                        StringComparison.OrdinalIgnoreCase) != 0)
                    throw new ValidationException("invalid version of the request");
                Validate(request);
                if (!ModelState.IsValid) throw new ValidationException("not valid model");
                var model = await action(request);
                result = ContentGenerator(model);
            }
            catch (AuthenticationException exception)
            {
                LogException(request, exception);
                var model = GenerateExceptionResponse(401, exception);
                result = ContentGenerator(model);
            }
            catch (ValidationException exception)
            {
                LogException(request, exception);
                var model = GenerateValidationExceptionResponse();
                result = ContentGenerator(model);
            }
            catch (Exception exception)
            {
                LogException(request, exception);
                var model = GenerateExceptionResponse(500, exception);
                result = ContentGenerator(model);
            }
            return await result;
        }

        protected void LogException<TRequest>(TRequest request, Exception exception)
            where TRequest : BaseRequest
        {
            var badApiRequest = JsonConvert.SerializeObject(request);
            Logger.Instance.Error($"ip for bad request: {IP.GetIPAddress()}");
            Logger.Instance.Error($"bad api request: {badApiRequest}");
            Logger.Instance.Error(exception);
        }

        protected BaseResponse GenerateExceptionResponse(int httpCode, Exception exception)
        {
            var exceptionMessage = IsVisibleExceptionRemoute ? exception.Message : "bad request";
            return BaseResponse.Create(httpCode, exceptionMessage);
        }

        protected BaseResponse GenerateValidationExceptionResponse()
        {
            var index = 1;
            var exceptionMessageBuilder=new StringBuilder();
            ModelState.Keys.Where(key => key.StartsWith("request")).ForEach(key =>
                exceptionMessageBuilder.Append($"{index++}) {ModelState[key].Errors.First().ErrorMessage} "));
            var exceptionMessage = IsVisibleExceptionRemoute ? exceptionMessageBuilder.ToString() : "bad request";
            return BaseResponse.Create(500, exceptionMessage);
        }

        protected bool IsVisibleExceptionRemoute { get; }
        public TRepository Repository { get; set; }
        protected IWrapper<ILog> Logger { get; set; }
        protected IVersionRepository VersionRepository { get; set; }
    }
}
