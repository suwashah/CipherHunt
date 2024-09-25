using Google.Authenticator;
using Repository.CPanel;
using Repository.HelperFunction;
using System;
using System.Web.Mvc;
using CipherHunt.Areas.Cpanel.Models;
using CipherHunt.Models;

namespace CipherHunt.Areas.Cpanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthenticationController : CAppController
    {
        IUtilityHelper _func;
        ICpanelUserRepository _iusr;
        ISMTPService _ismtp;
        public AuthenticationController(IUtilityHelper func, ICpanelUserRepository iusr, ISMTPService ismtp)
        {
            _func = func;
            _iusr = iusr;
            _ismtp = ismtp;
        }
        [HttpGet]
        public ActionResult Index()
        {
            string session = _func.CheckSession("encryptedSession", true);
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            AuthenticatorModel model = new AuthenticatorModel();
            var CP_User = _iusr.GetCPanelUserDetails(CurrentUser.ID);
            if (String.IsNullOrEmpty(CP_User.Token) || CP_User.Auth_Required == false)
            {
                return RedirectToAction("LogOff", "Acc");
            }
            if (!String.IsNullOrEmpty(CP_User.Auth_Approved_By) && !String.IsNullOrEmpty(CP_User.Auth_key))
            {
                return RedirectToAction("OTPAuthenticate");
            }
            string key = CP_User.Token.Replace("-", "");
            string UserUniqueKey = CP_User.UserName + key; //as Its a demo, I have done this way. But you should use any encrypted value here which will be unique value per user 
            var setupInfo = tfa.GenerateSetupCode(_func.GetAppsettingValue("company_name"), CP_User.UserName, UserUniqueKey, false, 6);
            model.UserName = CP_User.StaffName;
            model.UniqueKey = UserUniqueKey;
            model.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
            model.SetupCode = setupInfo.ManualEntryKey;
            model.AuthKey = CP_User.Auth_key;
            model.AuthDateTime = CP_User.Auth_DateTime;
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(AuthenticatorModel model)
        {
            if (ModelState.IsValid)
            {
                var token = model.OTPCode;
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                string UserUniqueKey = model.UniqueKey;
                bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
                if (isValid)
                {
                    Session["IsValid2FA"] = true;
                    //////////////////Save AuthKey//////////////////////
                    var req = _iusr.RequestAuthorization(CurrentUser.ID, UserUniqueKey);
                    if (req.CODE == "0")
                    {
                        _ismtp.PushEmail();
                        Session["Title"] = "Authentication Success and Requested for approval";
                        return RedirectToAction("RequestSuccess");
                    }
                    else
                    {
                        ViewBag.Message = req.MESSAGE;
                    }
                }
                else
                {
                    ViewBag.Message = "Authentication failed.";
                }
            }
            return View(model);
        }
        public ActionResult RequestSuccess()
        {
            SuccessModel model = new SuccessModel();
            model.Title = _func.CheckSession("Title");
            return View(model);
        }
        [HttpGet]
        public ActionResult OTPAuthenticate()
        {
            string session = _func.CheckSession("encryptedSession", true);
            AuthenticatorModel model = new AuthenticatorModel();
            var CP_User = _iusr.GetCPanelUserDetails(CurrentUser.ID);
            if (String.IsNullOrEmpty(CP_User.Token) || CP_User.Auth_Required == false)
            {
                return RedirectToAction("LogOff", "Acc");
            }
            if (String.IsNullOrEmpty(CP_User.Auth_Approved_By) && String.IsNullOrEmpty(CP_User.Auth_key))
            {
                return RedirectToAction("Index");
            }
            model.UserName = CP_User.StaffName;
            return View(model);
        }
        [HttpPost]
        public ActionResult OTPAuthenticate(AuthenticatorModel model)
        {
            var token = model.OTPCode;
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var CP_User = _iusr.GetCPanelUserDetails(CurrentUser.ID);
            string UserUniqueKey = CP_User.Auth_key;
            bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
            if (isValid)
            {
                Session["IsValid2FA"] = true;
                return RedirectToAction("Index", "DashBoard");
            }
            else
            {
                ViewBag.Message = "Authentication failed.";
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var ret = _iusr.ChangePassword(model.OldPassword, model.NewPassword, CurrentUser.ID);
                if (ret.CODE == "0")
                {
                    Response.Cookies[_func.GetAppsettingValue("LoginCookie")].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies[_func.GetAppsettingValue("LoginCookie")].Value = ret.MESSAGE;
                    return RedirectToAction("Login", "Acc");
                }
                else
                {
                    ViewBag.JavaScriptFunction = string.Format("toastrMessage('error', '" + ret.MESSAGE + "', 'Message');");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var ret = _iusr.ChangePassword(model.OldPassword, model.NewPassword, CurrentUser.ID);
                if (ret.CODE == "0")
                {
                    Response.Cookies[_func.GetAppsettingValue("LoginCookie")].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies[_func.GetAppsettingValue("LoginCookie")].Value = ret.MESSAGE;
                    return RedirectToAction("Login", "Acc");
                }
                else
                {
                    ViewBag.JavaScriptFunction = string.Format("toastrMessage('error', '" + ret.MESSAGE + "', 'Message');");
                }
            }
            return View(model);
        }
    }
}