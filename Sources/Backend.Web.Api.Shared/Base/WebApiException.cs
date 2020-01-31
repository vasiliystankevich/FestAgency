using System;

namespace Backend.Web.Api.Shared.Base
{
    public class WebApiException : Exception
    {
        public WebApiException(BaseResponseStatus status) : base(status.Message)
        {
            Status = status;
        }
        public BaseResponseStatus Status { get; set; }
    }
}