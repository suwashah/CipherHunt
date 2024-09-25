using Repository.Common;
using Repository.CPanel;
using Repository.HelperFunction;
using System;
using System.Web.Mvc;
using CipherHunt.Areas.Cpanel.Models;
using CipherHunt.Library;

namespace CipherHunt.Areas.Cpanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CPanelUserController : CAppController
    {
        // GET: Cpanel/CPanelUser
        IUtilityHelper _func;
        ICpanelUserRepository _iusr;
        ISMTPService _ismtp;
        public CPanelUserController(IUtilityHelper func, ICpanelUserRepository iusr, ISMTPService ismtp)
        {
            _func = func;
            _iusr = iusr;
            _ismtp = ismtp;
        }
        public ActionResult Index()
        {
            var lst = _iusr.GetALLCpanelUsers(CurrentUser.ID);
            return View(lst);
        }
        [HttpGet]
        public ActionResult EditUser(string route)
        {
            CPanelUserModel model = new CPanelUserModel();
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string ID = qry["id"];
                var obj = _iusr.GetCPanelUserDetails(ID);
                model.ID = ID;
                if (String.IsNullOrEmpty(obj.UserName))
                {
                    return RedirectToAction("Index");
                }
                model.UserName = obj.UserName;
                model.StaffName = obj.StaffName;
                model.EmailAddress = obj.Email;
                model.CreateBy = obj.Create_By;
                model.Create_TS = obj.Create_TS;
                model.Update_By = obj.Update_By;
                model.Updte_TS = obj.Updte_TS;
                model.Lock_Status = obj.Lock_Status;
                model.Lock_By = obj.Lock_By;
                model.LoginAttempt = obj.LoginAttempt;
                model.Lock_Reason = obj.Lock_Reason;
                model.LastLoginDate = obj.LastLoginDate;
                model.Enable_Pwd_Change = obj.Enable_Pwd_Change;
                model.Auth_Approved_By = obj.Auth_Approved_By;
                model.Auth_Approved_Ts = obj.Auth_Approved_Ts;
                model.Auth_DateTime = obj.Auth_DateTime;
                model.Auth_Required = obj.Auth_Required;
                model.Auth_key = obj.Auth_key;
                model.isEnable = obj.isEnable;
                model.UserRemarks = _iusr.GetCpanelUserRemark(ID);
            }
            else
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(CPanelUserModel model)
        {
            if (ModelState.IsValid)
            {
                var post = new CPanelDetail()
                {
                    StaffName = model.StaffName,
                    Email = model.EmailAddress,
                    Create_By = CurrentUser.Name,
                    isEnable = model.isEnable,
                    //Lock_Status = model.Lock_Status,
                    //Auth_Required = model.Auth_Required,
                    UNIQUEID = model.ID
                };
                var ret = _iusr.UpdateCpanelUser(post);
                if (ret.CODE == "0")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ret.MESSAGE = _func.isNull(ret.MESSAGE, "Something went wrong");
                    ViewBag.Message = ret.MESSAGE;
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(CPanelUserModel model)
        {
            if (ModelState.IsValid)
            {
                var post = new CPanelDetail()
                {
                    UserName = model.UserName,
                    StaffName = model.StaffName,
                    Email = model.EmailAddress,
                    Create_By = CurrentUser.Name
                };
                var ret = _iusr.AddCpanelUser(post);
                if (ret.CODE == "0")
                {
                    _ismtp.PushEmail();
                    return RedirectToAction("Index");
                }
                else
                {
                    ret.MESSAGE = _func.isNull(ret.MESSAGE, "Something went wrong");
                    ViewBag.Message = ret.MESSAGE;
                }
            }
            return View(model);
        }
        public JsonResult ClearAuthentication(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var obj = _iusr.UpdateAuthentication("clauth", id, CurrentUser.Name);
                return Json(obj);
            }
            else
            {
                return Json(new { CODE = "4001", MESSAGE = "No data found to clear" });
            }
        }
        public JsonResult ApproveAuthentication(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var obj = _iusr.UpdateAuthentication("apauth", id, CurrentUser.Name);
                return Json(obj);
            }
            else
            {
                return Json(new { CODE = "4001", MESSAGE = "No data found to approve" });
            }
        }
    }

}