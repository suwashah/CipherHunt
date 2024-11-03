using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CipherHunt.Controllers
{
    public class ErrorController : Controller
    {
        ICommonRepository _icr;
        public ErrorController(ICommonRepository icr)
        {
            _icr=icr;
        }
        public ActionResult GeneralError(string error_id)
        {
            var error_Log = _icr.GetErrorDetail(error_id);
            return View(error_Log);
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            _icr.SaveError("500 Internal Server error occurred.", "Server Error");
            return View();
        }       
    }
}