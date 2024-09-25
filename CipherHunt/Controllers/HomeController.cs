using Repository.Common;
using Repository.Product;
using System.Collections.Generic;
using System.Web.Mvc;
using CipherHunt.Library;
using CipherHunt.Models;

namespace CipherHunt.Controllers
{
    [AllowAnonymous]
    public class HomeController : AppController
    {
        CalorieCalculator cal = new CalorieCalculator();
        private readonly IProductRepository _ipr;
        public HomeController(IProductRepository ipr)
        {
            _ipr = ipr;
        }
        [HttpGet]
        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.ComboMeals = _ipr.GetComboMeal();
            model.HomeMenu = _ipr.GetHomeMenu();
            return View(model);
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult HowItWorks()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CalorieCalculate()
        {
            CalorieModel cal = new CalorieModel();
            BindDropDown(cal);
            return View(cal);
        }

        private void BindDropDown(CalorieModel model)
        {
            Dictionary<string, string> acl = new Dictionary<string, string>();
            //acl.Add("1", "Basal Metabolic Rate (BMR)");
            acl.Add("1.2", "Sedentary: little or no exercise");
            acl.Add("1.375", "Light: exercise 1-3 times/week");
            acl.Add("1.465", "Moderate: exercise 4-5 times/week");
            acl.Add("1.55", "Active: daily exercise or intense exercise 3-4 times/week");
            acl.Add("1.725", "Very Active: intense exercise 6-7 times/week");
            //acl.Add("1.9", "Extra Active: very intense exercise daily, or physical job");
            ViewData["ActivityFactor"] = StaticData.SetDDLValue(acl, model.ActivityFactor, "Select Activity Factor");

            Dictionary<string, string> ht_ft = new Dictionary<string, string>();
            ht_ft.Add("4", "4");
            ht_ft.Add("5", "5");
            ht_ft.Add("6", "6");
            ht_ft.Add("7", "7");
            ViewData["Height_Feet"] = StaticData.SetDDLValue(ht_ft, model.Height_Feet, "Feet");

            Dictionary<string, string> ht_in = new Dictionary<string, string>();
            ht_in.Add("0", "0");
            ht_in.Add("1", "1");
            ht_in.Add("2", "2");
            ht_in.Add("3", "3");
            ht_in.Add("4", "4");
            ht_in.Add("5", "5");
            ht_in.Add("6", "6");
            ht_in.Add("7", "7");
            ht_in.Add("8", "8");
            ht_in.Add("9", "9");
            ht_in.Add("10", "10");
            ht_in.Add("11", "11");
            ht_in.Add("12", "12");
            ViewData["Height_Inch"] = StaticData.SetDDLValue(ht_in, model.Height_Inch, "Inch");
        }
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";
        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";
        //    return View();
        //}
    }
}
