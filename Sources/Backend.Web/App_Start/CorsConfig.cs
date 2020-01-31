using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Cors;
using Owin;

namespace Backend.Web
{
    public class CorsConfig
    {
        public static void RegisterCors(IAppBuilder app, HttpConfiguration config)
        {
            var appSettings = WebConfigurationManager.AppSettings;

            if (string.IsNullOrWhiteSpace(appSettings["cors:Origins"])) return;

            var corsPolicy = new EnableCorsAttribute(
                appSettings["cors:Origins"],
                appSettings["cors:Headers"],
                appSettings["cors:Methods"]);

            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = request =>
                        request.Path.Value == "/token" ?
                            corsPolicy.GetCorsPolicyAsync(null, CancellationToken.None) :
                            Task.FromResult<CorsPolicy>(null)
                }
            });

            config.EnableCors(corsPolicy);
        }
    }
}