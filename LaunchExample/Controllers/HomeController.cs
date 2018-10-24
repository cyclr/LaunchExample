using System.Web.Mvc;

namespace Cyclr.LaunchExample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Organisation");
        }
    }
}