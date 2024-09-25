using Repository.Common;
using Repository.HelperFunction;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CipherHunt.Areas.Cpanel.Models;
using CipherHunt.Filters;
using CipherHunt.Library;

namespace CipherHunt.Areas.Cpanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UtilitiesController : CAppController
    {
        // GET: Cpanel/Utilities
        private ICommonRepository _icr;
        private IUtilityHelper _func;
        public UtilitiesController(ICommonRepository icr, IUtilityHelper func)
        {
            _icr = icr;
            _func = func;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StaticDataList()
        {
            var model = _icr.GetStaticDataTypes();
            return View(model);
        }
        public ActionResult StaticDatas(string route)
        {
            SDViewModel model = new SDViewModel();
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string ID = qry["id"];
                model.TYPE_ID = ID;
                model.TYPE_Name = _icr.GetStaticDataTypeDetail(ID).TYPE_NAME;
                Session["TypeName"] = model.TYPE_Name;
                model.StaticDatas = _icr.GetStaticDatas(ID);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult SaveStaticData(string route)
        {
            StaticDataModel model = new StaticDataModel();
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string TypeID = qry["TypeId"];
                string ID = qry["id"];
                if (!String.IsNullOrEmpty(ID))
                {
                    var item = _icr.GetStaticData(ID);
                    model = StaticData.ModelToCommon(item, new StaticDataModel());
                    model.Flag = "u";
                }
                else
                {
                    model.Flag = "i";
                }
                model.TYPE_ID = TypeID;
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.TypeName = _func.CheckSession("TypeName");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveStaticData(StaticDataModel model)
        {
            var req = StaticData.ModelToCommon(model, new Static_Data());
            var ret = _icr.SaveStaticData(req);
            if (ret.CODE == "0")
            {
                return RedirectToAction("StaticDatas", new { route = StaticData.Encrypt("id=" + model.TYPE_ID) });
            }
            else
            {
                ret.MESSAGE = _func.isNull(ret.MESSAGE, "Something went wrong");
                ViewBag.Message = ret.MESSAGE;
            }
            ViewBag.TypeName = _func.CheckSession("TypeName");
            return View(model);
        }
        public JsonResult DeleteStaticData(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var del = _icr.DeleteStaticData(id);
                return Json(del);
            }
            else
            {
                return Json(new { CODE = "4001", MESSAGE = "No data found to delete" });
            }
        }
        public ActionResult AllNotifications()
        {
            var model = _icr.ViewAllNotification(CurrentUser.ID);
            return View(model);
        }
        public ActionResult SearchData(string SearchType, string SearchValue)
        {
            return View();
        }
    }

}