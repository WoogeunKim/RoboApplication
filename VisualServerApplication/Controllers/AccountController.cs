using ModelsLibrary.Auth;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using VisualServerApplication.Config;
using VisualServerApplication.Models;

namespace VisualServerApplication.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //private ApplicationSignInManager _signInManager;
        //private ApplicationUserManager _userManager;

        //public AccountController()
        //{
        //}

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set 
        //    { 
        //        _signInManager = value; 
        //    }
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            System.Security.Cryptography.SHA256Managed sha256Managed = new System.Security.Cryptography.SHA256Managed();
            //로그인 체크
            GroupUserVo result = Properties.EntityMapper.QueryForObject<GroupUserVo>("S136SelectUserList", new GroupUserVo() { CHNL_CD = model.CHNL_CD, USR_ID = model.USR_ID, USR_PWD = Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.USR_PWD))) });
            if (result == null)
            {
                ModelState.AddModelError("", "잘못된 로그인 시도입니다.");
                return View(model);
            }

            //쿠키 설정
            //HttpCookie userCookie = new HttpCookie("UserInfo");
            //userCookie["Name"] = result.USR_N1ST_NM;
            //userCookie["Id"] = result.USR_ID;
            //userCookie["Company"] = result.CHNL_NM;
            //userCookie.Expires.AddDays(50); // cookie will expire after 50 days
            //Response.Cookies.Add(userCookie);
            //HttpContext context = HttpContext.Current;
            //Session["Name"] = model.UserID;
            //Session["Id"] = result.USR_ID;
            //Session["Company"] = result.CHNL_NM;
            //Session["CHNL_CD"] = result.CHNL_CD;

            //HttpContext.Session.Add("Name", result.USR_N1ST_NM);

            HttpContext.Session.Add("CHNL_NM", result.CHNL_NM);

            HttpContext.Session.Add("USR_ID", result.USR_ID);
            HttpContext.Session.Add("CHNL_CD", result.CHNL_CD);
            HttpContext.Session.Timeout = 600;

            FormsAuthentication.SetAuthCookie(model.CHNL_CD, model.RememberMe);

            return RedirectToLocal(returnUrl);

            //// 계정이 잠기는 로그인 실패로 간주되지 않습니다.
            //// 암호 오류 시 계정 잠금을 트리거하도록 설정하려면 shouldLockout: true로 변경하십시오.
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //ModelState.AddModelError("", "잘못된 로그인 시도입니다.");
            //return View(model);
            //}
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //HttpContext.Session.Remove("");
            FormsAuthentication.SignOut();
            HttpContext.Session.Clear();
            //System.Web.Security.FormsAuthentication.SignOut();
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
       
    }
}