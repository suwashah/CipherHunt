using Repository.Email;
using Repository.HelperFunction;
using System;
using System.Web.Mvc;
using CipherHunt.Areas.Cpanel.Models;
using CipherHunt.Library;

namespace CipherHunt.Areas.Cpanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmailTemplateController : CAppController
    {
        // GET: Cpanel/EmailTemplate
        private readonly IEmailRepository _iem;
        private readonly IUtilityHelper _func;
        public EmailTemplateController(IEmailRepository iem, IUtilityHelper func)
        {
            _iem = iem;
            _func = func;
        }
        public ActionResult Index()
        {
            var lst = _iem.EmailTemplateList();
            return View(lst);
        }
        [HttpGet]
        public ActionResult SaveEmail(string route)
        {
            EmailtemplateModel model = new EmailtemplateModel();
            model.StaticDatas = _iem.GetEmailIDList();
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string ID = qry["id"];
                var obj = _iem.EditTemplate(ID);
                model.TEMP_ID = obj.TEMP_ID;
                model.TEMP_NAME = obj.TEMP_NAME;
                model.TEMP_EMAIL_SUBJECT = obj.TEMP_EMAIL_SUBJECT;
                model.TEMP_EMAIL_BODY = obj.TEMP_EMAIL_BODY;
                model.enable = obj.enable;
                model.Response = obj.Response;
                model.FLAG = "u";
            }
            else
            {
                model.FLAG = "i";
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEmail(EmailtemplateModel model)
        {
            if (ModelState.IsValid)
            {
                var ret = _iem.SaveTemplate(model.FLAG, model.TEMP_ID, model.TEMP_NAME, model.TEMP_EMAIL_SUBJECT, model.TEMP_EMAIL_BODY, model.enable, model.Response);
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
    }
}