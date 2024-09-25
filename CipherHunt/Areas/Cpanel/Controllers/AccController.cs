using Microsoft.AspNet.Identity;
using Repository.Common;
using Repository.CPanel;
using Repository.HelperFunction;
using System;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using CipherHunt.Areas.Cpanel.Models;
using CipherHunt.Authentication;

namespace CipherHunt.Areas.Cpanel.Controllers
{
    [AllowAnonymous]
    public class AccController : Controller
    {
        IUtilityHelper _func;
        ICpanelUserRepository _iusr;
        ISMTPService _ismtp;
        public AccController(IUtilityHelper func, ICpanelUserRepository iusr, ISMTPService ismtp)
        {
            _func = func;
            _iusr = iusr;
            _ismtp = ismtp;
        }
        // GET: Cpanel/Account
        [HttpGet]
        public ActionResult Login()
        {
            CpanelLoginModel model = new CpanelLoginModel();
            if (Request.Cookies[_func.GetAppsettingValue("LoginCookie")] != null)
            {
                var msg = Request.Cookies[_func.GetAppsettingValue("LoginCookie")].Value;
                ViewBag.SuccessMessage = msg;
            }
            Response.Cookies[_func.GetAppsettingValue("LoginCookie")].Expires = DateTime.Now.AddDays(-1);
            _ismtp.PushEmail();
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(CpanelLoginModel model)
        {
            var ret = new CPanelDetail();
            if (ModelState.IsValid)
            {
                ret = _iusr.LoginUser(model.UserName, model.Password);
                if (ret.CODE == "0")
                {
                    var user = new CPanelAppUser()
                    {
                        ID = ret.UNIQUEID,
                        Username = ret.UserName,
                        Email = ret.Email,
                        Token = ret.Token
                    };
                    SetAuthCookie(user);
                    if (ret.Enable_Pwd_Change == true)
                    {
                        return RedirectToAction("ResetPassword", "Authentication");
                    }
                    if (ret.Auth_Required || !String.IsNullOrEmpty(ret.Auth_DateTime))
                    {
                        return RedirectToAction("Index", "Authentication");
                    }
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.Message = ret.MESSAGE;
                }
            }
            return View(model);
        }
        public void SetAuthCookie(CPanelAppUser user)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier,user.ID),
                    new Claim(ClaimTypes.Role,"Admin"),
                    new Claim(ClaimTypes.Sid, user.ID),
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Hash, user.Token),
                }, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            string enc_session = _func.EncryptSession(user.Token);
            Session["encryptedSession"] = enc_session;
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = claimsPrincipal;
        }
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}