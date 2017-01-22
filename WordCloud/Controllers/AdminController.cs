using System.Web.Mvc;
using WordCloud.Services;

namespace WordCloud.Controllers
{
    public class AdminController : Controller
    {
        private IAdminService _adminService;

        public AdminController()
        {
            //todo: inject
            _adminService = new AdminService();
        }

        [HttpGet]
        public ActionResult Index()
        {   
            //todo: should return all with paging
            return View(_adminService.GetTopWords());
        }
    }
}