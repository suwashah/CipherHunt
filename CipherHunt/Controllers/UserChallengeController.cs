using Antlr.Runtime.Misc;
using CipherHunt.Library;
using CipherHunt.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Challenge;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CipherHunt.Controllers
{
    [Authorize(Roles = "User")]
    public class UserChallengeController : Controller
    {
        private IChallengeRepository _ich;
        public UserChallengeController(IChallengeRepository ich)
        {
            _ich = ich;       
        }
        [HttpGet]
        public ActionResult Index()
        {
            var lst = _ich.GetAllChallenges("a");
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
                model.FLAG = "u";
                model.CTF_FLAG = ch.CTF_FLAG;
                model.IMAGENAME = ch.IMAGENAME;
                model.HINT_1 = ch.HINT_1;
                model.HINT_2 = ch.HINT_2;
                model.HINT_3 = ch.HINT_3;
                model.INTENDED_LEARNING = ch.INTENDED_LEARNING;
                model.CHALLENGE_SOLUTION = ch.CHALLENGE_SOLUTION;
            }
            else
            {
                model.FLAG = "i";
            }
            return View(model);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SaveChallenge(ChallengeModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
               
        //        var post = new TblChallenge()
        //        {
        //            FLAG = model.FLAG,
        //            CHALLENGE_ID = model.CHALLENGE_ID,
        //            CAT_ID = model.CAT_ID,
        //            NAME = model.NAME,
        //            DESCRIPTION = model.DESCRIPTION,
        //            DIFFICULTY_LEVEL = model.DIFFICULTY_LEVEL,
        //            POINTS = model.POINTS,
        //            CTF_FLAG = model.CTF_FLAG,
        //            CREATE_BY = CurrentUser.Name,
        //            UPDATE_BY = CurrentUser.Name,
        //            IMAGE = bytes,
        //            IMAGENAME = ImageName,
        //            IS_ENABLE = model.IS_ENABLE,
        //            HINT_1 = model.HINT_1,
        //            HINT_2 = model.HINT_2,
        //            HINT_3 = model.HINT_3,
        //            INTENDED_LEARNING = model.INTENDED_LEARNING,
        //            CHALLENGE_SOLUTION = model.CHALLENGE_SOLUTION
        //        };
        //        var ret = _ich.SaveChallenge(post);
        //        if (ret.CODE == "0")
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ret.MESSAGE = _func.isNull(ret.MESSAGE, "Something went wrong");
        //            ViewBag.Message = ret.MESSAGE;
        //        }
        //    }
        //    BindDropDown(model);
        //    return View(model);
        //}
    }
}