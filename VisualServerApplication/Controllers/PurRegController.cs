using System.Threading.Tasks;
using System.Web.Mvc;
using VisualServerApplication.Models;

namespace VisualServerApplication.Controllers
{
    public class PurRegController : Controller
    {
        // GET: PurReg
        public ActionResult Index()
        {
            if(HttpContext.Session.Count == 0)
            { 
               return RedirectToAction("Login", "Account");
            }
            return View();
        }


        public ActionResult Situation()
        {
            if (HttpContext.Session.Count == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }


        public ActionResult Equipment()
        {
            if (HttpContext.Session.Count == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Equipment(RegisterEqViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //
            if (string.IsNullOrEmpty(model.PROC_LOT_NO))
            {
                ModelState.AddModelError("", "[PROC LOT NO] 올바르지 않습니다.");
                return View(model);
            }


            return View();

        }


        public ActionResult MaterialInput()
        {
            if (HttpContext.Session.Count == 0)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }


        //// GET: PurReg/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: PurReg/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: PurReg/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: PurReg/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: PurReg/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: PurReg/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: PurReg/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
