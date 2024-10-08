﻿using Repository.Common;
using Repository.CPanel;
using Repository.Customer;
using Repository.Product;
using System;
using System.Web.Mvc;
using CipherHunt.Areas.Cpanel.Models;
using CipherHunt.Filters;
using CipherHunt.Library;
using Repository.Challenge;

namespace CipherHunt.Areas.Cpanel.Controllers
{
    //[SessionExpiryFilter]
    [Authorize(Roles = "Admin")]
    public class DashboardController : CAppController
    {
        // GET: Cpanel/Dashboard
        private IProductRepository _ipr;
        private IChallengeRepository _ich;
        private ICommonRepository _icr;
        private ICustomerRepository _icust;
        private ICpanelUserRepository _iusr;
        public DashboardController(IProductRepository ipr,IChallengeRepository ich, ICommonRepository icr, ICustomerRepository icust, ICpanelUserRepository iusr)
        {
            _ipr = ipr;
            _ich = ich;
            _icr = icr;
            _icust = icust;
            _iusr = iusr;
        }
        public ActionResult Index()
        {
            ApplicationViewModel model = new ApplicationViewModel();
            model.TotalChallenges = _ich.GetAllChallenges("a").Count.ToString();
            model.TotalUnverifiedChallenges = _ich.GetAllChallenges("uv").Count.ToString();
            model.TotalCustomers = _icust.AllCustomerList().Count.ToString();
            model.TotalCategories = _ipr.GetAllCategories().Count.ToString();
            model.CpanelUsers = _iusr.GetALLCpanelUsers(CurrentUser.ID);
            model.TopThreeNotification = _icr.GetTopThreeNotification();
            return View(model);
        }
        public JsonResult GetNotifications()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetMessage(notificationRegisterTime);
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}