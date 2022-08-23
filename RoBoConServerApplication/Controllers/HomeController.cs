using System.Web.Mvc;

namespace VisualServerApplication.Controllers
{
    public class HomeController : Controller
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            if (HttpContext.Session.Count == 0)
            {
                ViewBag.Title = "Aquila ERP / MES";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.Title = "Aquila ERP / MES - " + HttpContext.Session["CHNL_NM"];
            }
            return View();
        }
    }
}
