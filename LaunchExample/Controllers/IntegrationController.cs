using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cyclr.LaunchExample.Controllers
{
    [Authorize]
    public class IntegrationController : Controller
    {
        public ActionResult ApiUser()
        {
            return View(Tuple.Create(ConfigurationManager.AppSettings["ApiUsername"], ConfigurationManager.AppSettings["ApiPassword"]));
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Add()
        {
            return Redirect(await Helpers.ApiHelper.GetLaunchUrl());
        }

        public async Task<ActionResult> Manage()
        {
            return Redirect(await Helpers.ApiHelper.GetOrbitUrl());
        }
    }
}