using Repository.Common;
using Repository.CPanel;
using System;
using System.Web.Mvc;
using CipherHunt.Models;

namespace CipherHunt.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]/[action]")]

    [Authorize]
    //
    public class RestController : AppController
    {
        private readonly ICommonRepository _icr;
        private readonly IApplicationConfig _app;
        private readonly ICalorieCalculator _ical;
        private readonly IGraphRepository _igr;
        public RestController(
            ICommonRepository icr,
            IApplicationConfig app,
            ICalorieCalculator ical,
            IGraphRepository igr)
        {
            _icr = icr;
            _app = app;
            _ical = ical;
            _igr = igr;
        }
        [AllowAnonymous]
        public JsonResult AppConfig()
        {
            return Json(_app.WebAppConfig(), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult ApplicationConfig()
        {
            return Json(_app.AppConfig(), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult CalorieCalculate(CalorieModel model)
        {
            double a = Convert.ToDouble(model.Height_Feet);
            double b = Convert.ToDouble(model.Height_Inch);
            var c = ((a * 12) + b) * 2.54;// Height in cm
            CalorieInput inp = new CalorieInput();
            inp.Height = c;
            inp.Age = model.Age;
            inp.Weight = model.Weight;
            inp.ActivityFactor = Convert.ToDouble(model.ActivityFactor);
            inp.Gender = model.Gender;
            return Json(_ical.Calculate(inp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult NotificationList()
        {
            var lst = _icr.GetNotification();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NotificationCount()
        {
            var lst = _icr.NotificationCount();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DrawWeeklyGraph(string type = "line")
        {
            var lst = _igr.DrawWeeklyCustomerGraph("myChart1", type);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}