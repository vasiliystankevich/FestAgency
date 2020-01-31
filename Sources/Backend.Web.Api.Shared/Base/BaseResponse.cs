namespace Backend.Web.Api.Shared.Base
{
    public class BaseResponse
    {
        public BaseResponse() { }
        protected BaseResponse(int code, string message)
        {
            Status = BaseResponseStatus.Create(code, message);
            Version = GetVersion();
        }

        public string GetVersion() => "1.0.0.0";

        public static BaseResponse Create(int code, string message)
        {
            return new BaseResponse(code, message);
        }

        public static BaseResponse Ok()
        {
            return new BaseResponse(200, "Ok");
        }


        public BaseResponseStatus Status { get; set; }
        public string Version { get; set; }
    }

    public class OkResponse : BaseResponse
    {
        public OkResponse() : base(200, "Ok")
        {
        }
        public OkResponse(string message) : base(200, message)
        {
        }
    }
}
