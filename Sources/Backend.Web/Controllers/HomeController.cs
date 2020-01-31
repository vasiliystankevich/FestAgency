using System.Threading.Tasks;
using System.Web.Mvc;
using Backend.Web.Core.Backend;
using log4net;
using Project.Kernel;

namespace Backend.Web.Controllers
{
    public interface IHomeController
    {
        Task<ActionResult> Index();
    }

    public interface IHomeRepository
    {
    }

    public class HomeRepository : BaseRepository, IHomeRepository
    {
        public HomeRepository(IWrapper<ILog> logger) : base(logger)
        {
        }
    }

    [CompressContent]
    public class HomeController: BaseController<IHomeRepository>, IHomeController
    {
        public HomeController(IWrapper<ILog> log, IHomeRepository repository) : base(log, repository)
        {
        }

        public async Task<ActionResult> Index()
        {
            return await GeneratorActionResult("~/Views/Home/Index.cshtml");
        }
    }
}