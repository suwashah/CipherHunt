using Repository.Common;
using Repository.Customer;
using Repository.HelperFunction;
using System;
using System.Web.Mvc;
using CipherHunt.Library;
using CipherHunt.Models;

namespace CipherHunt.Areas.Cpanel.Controllers
{

    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly IUtilityHelper _func;
        private readonly ICommonRepository _icr;
        private readonly ICustomerRepository _icust;
        public CustomerController(IUtilityHelper func, ICommonRepository icr, ICustomerRepository icust)
        {
            _func = func;
            _icr = icr;
            _icust = icust;
        }
        // GET: Cpanel/Customer
        public ActionResult Index()
        {
            var lst = _icust.AllCustomerList();
            return View(lst);
        }
        public ActionResult Detail(string route)
        {
            var model = new CustomerDetailModel();
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string ID = qry["id"];
                var customer = _icust.GetCustomerDetail(ID, "DISP");
                model = StaticData.ModelToCommon(customer, new CustomerDetailModel());
            }
            return View(model);
        }
    }
}