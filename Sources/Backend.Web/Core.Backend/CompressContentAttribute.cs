using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace Backend.Web.Core.Backend
{
    public class CompressContentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = HttpContext.Current.Response;
            var acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
            var isGZipSupported = !string.IsNullOrEmpty(acceptEncoding) &&
                                  (acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate"));
            if (isGZipSupported)
            {
                if (acceptEncoding.Contains("gzip"))
                    CreateGZipCompressFilter(response);
                else
                    CreateDeflateCompressFilter(response);
            }
            response.AppendHeader("Vary", "Content-Encoding");
        }

        private static void CreateDeflateCompressFilter(HttpResponse response)
        {
            response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            response.Headers.Remove("Content-Encoding");
            response.AppendHeader("Content-Encoding", "deflate");
        }

        private static void CreateGZipCompressFilter(HttpResponse response)
        {
            response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            response.Headers.Remove("Content-Encoding");
            response.AppendHeader("Content-Encoding", "gzip");
        }
    }
}