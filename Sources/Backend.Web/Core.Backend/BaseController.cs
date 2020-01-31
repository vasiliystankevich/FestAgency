using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Backend.Web.Core.Backend.Interfaces;
using log4net;
using Project.Kernel;

namespace Backend.Web.Core.Backend
{
    public abstract class BaseController<TRepository>:Controller, IRepository<TRepository>
    {
        protected BaseController(IWrapper<ILog> log, TRepository repository)
        {
            Log = log;
            Repository = repository;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            Thread.CurrentThread.CurrentCulture =
                            Thread.CurrentThread.CurrentUICulture =
                                new CultureInfo("en-US");
            base.Initialize(requestContext);
        }

        protected Task<T> GetAsyncResult<T>(T result) where T:ActionResult
        {
            return Task<T>.Factory.StartNew(() => result);
        }

        public Task<ActionResult> GeneratorActionResult(string viewName)
        {
            return Task<ActionResult>.Factory.StartNew(() => View(viewName));
        }

        public Task<ActionResult> GeneratorActionResult<T>(string viewName, T model)
        {
            return Task<ActionResult>.Factory.StartNew(() => View(viewName, model));
        }

        public TRepository Repository { get; }
        IWrapper<ILog> Log { get; set; }
    }
}