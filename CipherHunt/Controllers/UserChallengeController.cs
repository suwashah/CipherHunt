﻿using Antlr.Runtime.Misc;
using CipherHunt.API;
using CipherHunt.Library;
using CipherHunt.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Challenge;
using Repository.Common;
using Repository.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CipherHunt.Controllers
{
    [Authorize(Roles = "User")]
    public class UserChallengeController : Controller
    {
        private IChallengeRepository _ich;
        private IProductRepository _ipr;

        private readonly APIService _apiService;
        public UserChallengeController(IChallengeRepository ich, IProductRepository ipr, APIService apiService)
        {
            _ich = ich;    
            _ipr = ipr;
            _apiService = apiService;
        }
        [HttpGet]
        public ActionResult Index(string route)
        {
            List<string> categories_filter = _ipr.GetAllCategories().Select(e => e.CATEGORY_NAME).ToList();            
            var Difficulty_filter = new List<string>()
            {
                "All Difficulties",
                "Easy",
                "Medium",
                "Hard"
            };
            var lst = _ich.GetAllChallenges("a").Where(i=>i.IS_VERIFIED&&i.IS_ENABLE);
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string difficulty = qry["difficulty"];
                string category=qry["category"];
                if(!String.IsNullOrEmpty(difficulty))
                {
                    if(difficulty != "All Difficulties")
                        lst = lst.Where(i => i.DIFFICULTY_LEVEL.Equals(difficulty));
                    ViewBag.Selected_difficulty = difficulty;
                }               
                if (!String.IsNullOrEmpty(category))
                {
                    lst = lst.Where(i => i.CATEGORY_NAME.Equals(category));
                    ViewBag.Selected_category = category;
                }

            }
            else
            {
                ViewBag.Selected_difficulty = "All Difficulties";
            }
            ViewBag.Difficulty_filter=Difficulty_filter;
            ViewBag.Category_filter = categories_filter;
            return View(lst);
        }
        [HttpGet]
        public ActionResult History()
        {
            var lst = _ich.GetAllChallenges("yc",User.Identity.GetUserId());
            return View(lst);
        }

        public JsonResult SubmitFlag(SubmitFlagModel model)
        {
            var post = new SubmitUserFlag()
            {
                USER_FLAG = model.USER_FLAG,
                CHALLENGE_ID = model.CHALLENGE_ID,
                USER_ID = User.Identity.GetUserId()
            };
            return Json(_ich.SubmitFlag(post), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> StartDockerInstance(RunDockerModel model)
        {
            if (ModelState.IsValid)
            {
                var apiResponse = await _apiService.PostDataAsync(model);
                if (apiResponse != null && apiResponse.code=="0")
                {
                    return Json(new {code=apiResponse.code, success = true, message = apiResponse?.message ?? "Data sent successfully!", data = apiResponse });
                }
                else
                {
                    return Json(new { code = apiResponse.code, success = false, message = apiResponse?.message ?? "Error sending data." });
                }
            }
            return Json(new { code = "500", success = false, message = "Invalid data.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost]
        public async Task<JsonResult> StopDockerInstance(RunDockerModel model)
        {
            if (ModelState.IsValid)
            {
                var apiResponse = await _apiService.PostDataAsync(model,false);
                if (apiResponse != null && apiResponse.code == "0")
                {
                    return Json(new { code = apiResponse.code, success = true, message = apiResponse?.message ?? "Data sent successfully!", data = apiResponse });
                }
                else
                {
                    return Json(new { code = apiResponse.code, success = false, message = apiResponse?.message ?? "Error sending data." });
                }
            }
            return Json(new { code = "500", success = false, message = "Invalid data.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpGet]
        public ActionResult ChallengeDetail(string route)
        {
            ChallengeModel model = new ChallengeModel();
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string ID = qry["id"];
                var ch = _ich.GetChallengeDetail(ID);
                model.CHALLENGE_ID = ch.CHALLENGE_ID;
                model.CAT_ID = ch.CAT_ID;
                model.CATEGORY_NAME = ch.CATEGORY_NAME;
                model.NAME = ch.NAME;
                model.DESCRIPTION = ch.DESCRIPTION;
                model.CREATE_TS = ch.CREATE_TS;
                model.CREATE_BY = ch.CREATE_BY;
                model.UPDATE_BY = ch.UPDATE_BY;
                model.UPDATE_TS = ch.UPDATE_TS;
                model.IS_ENABLE = ch.IS_ENABLE;
                model.POINTS = ch.POINTS;
                model.DIFFICULTY_LEVEL = ch.DIFFICULTY_LEVEL;
                model.ImageSrc = ch.IMAGE_URL;
                model.STATUS = ch.STATUS;
                model.IS_ENABLE = ch.IS_ENABLE;
                model.IS_VERIFIED = ch.IS_VERIFIED;
                model.VERIFIED_BY = ch.VERIFIED_BY;
                model.CTF_FLAG = ch.CTF_FLAG;
                model.IMAGENAME = ch.IMAGENAME;
                model.HINT_1 = ch.HINT_1;
                model.HINT_2 = ch.HINT_2;
                model.HINT_3 = ch.HINT_3;
                model.INTENDED_LEARNING = ch.INTENDED_LEARNING;
                model.CHALLENGE_SOLUTION = ch.CHALLENGE_SOLUTION;
                model.CHALLENGE_URL = ch.CHALLENGE_URL;
                model.CHALLENGE_FOLDER = ch.CHALLENGE_FOLDER;
                model.FILE_PATH = ch.FILE_PATH;
            }           
            return View(model);
        }        
    }
}