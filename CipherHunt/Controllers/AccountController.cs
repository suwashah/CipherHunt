using Microsoft.AspNet.Identity;
using Repository.Common;
using Repository.Customer;
using Repository.HelperFunction;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CipherHunt.Authentication;
using CipherHunt.Models;

namespace CipherHunt.Controllers
{
    public class AccountController : Controller
    {
        IApplicationConfig _app;
        ICustomerRepository _cust;
        IUtilityHelper _func;
        ISMTPService _ismtp;
        public AccountController(IApplicationConfig app, ICustomerRepository cust, IUtilityHelper func, ISMTPService ismtp)
        {
            _app = app;
            _cust = cust;
            _func = func;
            _ismtp = ismtp;
        }
        // GET: Account
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            LoginModel model = new LoginModel();
            model.ReturnUrl = returnUrl;
            if (Request.Cookies[_func.GetAppsettingValue("LoginCookie")] != null)
            {
                var msg = Request.Cookies[_func.GetAppsettingValue("LoginCookie")].Value;
                ViewBag.SuccessMessage = msg;
            }
            if (Request.Cookies["UserName"] != null)
            {
                model.RememberMe = true;
                model.UserName = Request.Cookies["UserName"].Value;
            }
            else
            {
                model.RememberMe = false;
            }
            Response.Cookies[_func.GetAppsettingValue("LoginCookie")].Expires = DateTime.Now.AddDays(-1);
            //_ismtp.PushEmail();
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            var ret = new CustomerDetail();
            if (ModelState.IsValid)
            {
                ret = _cust.CustomerLogin(model.UserName, model.Password);
                if (ret.CODE == "0")
                {
                    var user = new AppUser()
                    {
                        ID = ret.UNIQUEID,
                        Username = ret.FULLNAME,
                        Email = ret.EMAIL,
                        Token = ret.TOKEN
                    };
                    SetAuthCookie(user);
                    //if (ret.ENABLEPASSWORDCHANGE == true)
                    //{
                    //    return RedirectToAction("ResetPassword", "Authentication");
                    //}
                    RememberMe(model.RememberMe, model.UserName);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = ret.MESSAGE;
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            var ap = _app.AppConfig();
            RegisterModel model = new RegisterModel();
            model.ISO2 = ap["ISO2"];
            model.TeleCode = "+" + ap["TELECODE"];
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> SignUp(RegisterModel model)
        {
            CommonData obj = new CommonData();
            if (ModelState.IsValid)
            {
                CustomerDetail uc = new CustomerDetail
                {
                    EMAIL = model.EmailAddress,
                    PASSWORD = model.Password,
                    FIRSTNAME = model.FirstName,
                    MIDDLENAME = model.Middlename,
                    LASTNAME = model.LastName,
                    PHONE = model.Mobile,
                    INVITE_CODE = model.InviteCode,
                    GENDER = model.Gender
                };
                var u = await _cust.asyncRegisterUser(uc);
                if (u.CODE == "0")
                {
                    Session["Email"] = model.EmailAddress;
                    Session["USERNAME"] = u.UNIQUEID;
                    _ismtp.PushEmail();
                    Session["SuccessMessage"] = _func.isNull(u.MESSAGE, "Your account has been Successfully Registered");
                    return Json(u);
                }
                else
                {
                    u.MESSAGE = _func.isNull(u.MESSAGE, "Something went wrong");
                    ViewBag.Message = u.MESSAGE;
                    ViewBag.JavaScriptFunction = string.Format("toastr.error('" + u.MESSAGE + "');");
                    return Json(u);
                }
            }
            else
            {
                obj.CODE = "4001";
                obj.MESSAGE = "Please fill out all the field.";
            }
            return Json(obj);
        }
        public ActionResult RegSuccess()
        {
            _ismtp.PushEmail();
            SuccessModel model = new SuccessModel()
            {
                Title = "Thank You!",
                Message = _func.CheckSession("SuccessMessage"),
                UserName = _func.CheckSession("USERNAME"),
                EmailAddress = _func.CheckSession("Email")
            };
            return View(model);
        }
        public void SetAuthCookie(AppUser user)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier,user.ID),
                    new Claim(ClaimTypes.Role,"User"),
                    new Claim(ClaimTypes.Sid, user.ID),
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Hash, user.Token),
                }, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            string enc_session = _func.EncryptSession(user.Token);
            Session["encryptedUserSession"] = enc_session;
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = claimsPrincipal;
        }
        private void RememberMe(bool Remember, string username)
        {
            if (Remember == true)
            {
                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                Response.Cookies["UserName"].Value = username;
            }
            else
            {
                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["UserName"].Value = null;
            }

        }
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}