using Repository.Common;
using Repository.HelperFunction;
using Repository.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using CipherHunt.Library;
using CipherHunt.Models;
using Repository.Challenge;

namespace CipherHunt.Areas.Cpanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ChallengeController : CAppController
    {
        // GET: Cpanel/Product
        private IChallengeRepository _ich;
        private IProductRepository _ipr;
        private IUtilityHelper _func;
        private ICommonRepository _icr;
        public ChallengeController(IProductRepository ipr, IChallengeRepository ich, IUtilityHelper func, ICommonRepository icr)
        {
            _ipr = ipr;
            _ich = ich;
            _func = func;
            _icr = icr;
        }
        public ActionResult Index()
        {
            var lst = _ich.GetAllChallenges("a");
            return View(lst);
        }
        public ActionResult PendingChallenges()
        {
            var lst = _ich.GetAllChallenges("uv");
            return View(lst);
        }
        public ActionResult Categories()
        {
            var lst = _ipr.GetAllCategories();
            return View(lst);
        }
        [HttpGet]
        public ActionResult SaveCategory(string route)
        {
            CategoryModel model = new CategoryModel();
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string ID = qry["id"];
                var cat = _ipr.GetCategoryDetail(ID);
                model.CAT_ID = cat.CAT_ID;
                model.CATEGORY_NAME = cat.CATEGORY_NAME;
                model.CATEGORY_DESCRIPTION = cat.CATEGORY_DESCRIPTION;
                model.IS_ENABLE = cat.IS_ENABLE;
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
        public ActionResult SaveCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var post = new Category()
                {
                    FLAG = model.FLAG,
                    CAT_ID = model.CAT_ID,
                    CATEGORY_NAME = model.CATEGORY_NAME,
                    CATEGORY_DESCRIPTION = model.CATEGORY_DESCRIPTION,
                    CREATE_BY = CurrentUser.Name,
                    CATEGORY_IMAGE = null,
                    IS_ENABLE = model.IS_ENABLE
                };
                var ret = _ipr.SaveCategory(post);
                if (ret.CODE == "0")
                {
                    return RedirectToAction("Categories");
                }
                else
                {
                    ret.MESSAGE = _func.isNull(ret.MESSAGE, "Something went wrong");
                    ViewBag.Message = ret.MESSAGE;
                }
            }
            return View(model);
        }

        public JsonResult DeleteCategory(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var del = _ipr.DeleteCategory(id);
                return Json(del);
            }
            else
            {
                return Json(new { CODE = "4001", MESSAGE = "No data found to delete" });
            }
        }

        [HttpGet]
        public ActionResult SaveChallenge(string route)
        {
            ChallengeModel model = new ChallengeModel();
            if (!String.IsNullOrEmpty(route))
            {
                var qry = StaticData.GetQueryParameters(route);
                string ID = qry["id"];
                var ch = _ich.GetChallengeDetail(ID);
                model.CHALLENGE_ID = ch.CHALLENGE_ID;
                model.CAT_ID = ch.CAT_ID;
                model.NAME = ch.NAME;
                model.DESCRIPTION = ch.DESCRIPTION;
                model.CREATE_TS = ch.CREATE_TS;
                model.CREATE_BY = ch.CREATE_BY;
                model.UPDATE_BY = ch.UPDATE_BY;
                model.UPDATE_TS = ch.UPDATE_TS;
                model.IS_ENABLE = ch.IS_ENABLE;
                model.POINTS = ch.POINTS;
                model.DIFFICULTY_LEVEL= ch.DIFFICULTY_LEVEL;
                model.ImageSrc = ch.CHALLENGE_URL;
                model.STATUS = ch.STATUS;
                model.IS_ENABLE = ch.IS_ENABLE;
                model.IS_VERIFIED = ch.IS_VERIFIED;
                model.VERIFIED_BY = ch.VERIFIED_BY;
                model.FLAG = "u";
                model.CTF_FLAG = ch.CTF_FLAG;
                model.IMAGENAME = ch.IMAGENAME;
                model.HINT_1=ch.HINT_1;
                model.HINT_2 = ch.HINT_2;
                model.HINT_3 = ch.HINT_3;
                model.INTENDED_LEARNING = ch.INTENDED_LEARNING;
                model.CHALLENGE_SOLUTION = ch.CHALLENGE_SOLUTION;
            }
            else
            {
                model.FLAG = "i";
            }
            BindDropDown(model);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveChallenge(ChallengeModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] bytes = null;
                string ImageName = "";
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    if (_func.GetAppsettingValue("SaveImageBytes") == "n")
                    {
                        string folderPath = Server.MapPath(_func.GetAppsettingValue("ChallengePath"));
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        Guid gid = Guid.NewGuid();
                        string fileName = "Challenge_" + _func.changeFilename(gid.ToString()) + "_" + model.CAT_ID + "_" + _func.changeFilename(DateTime.Now.ToBinary().ToString()) + Path.GetExtension(model.ImageFile.FileName);
                        string path = Path.Combine(Server.MapPath(_func.GetAppsettingValue("ChallengePath")), Path.GetFileName(fileName));
                        model.ImageFile.SaveAs(path);
                        ImageName = fileName;
                        //DocModel.DOCIMG = fileName;
                    }
                    else
                    {
                        byte[] fileInBytes = new byte[model.ImageFile.ContentLength];
                        using (BinaryReader theReader = new BinaryReader(model.ImageFile.InputStream))
                        {
                            fileInBytes = theReader.ReadBytes(model.ImageFile.ContentLength);
                        }
                        string fileAsString = Convert.ToBase64String(fileInBytes);
                        bytes = System.Convert.FromBase64String(fileAsString);
                    }                  
                    if (!String.IsNullOrEmpty(model.IMAGENAME) && model.FLAG == "u")
                    {
                        DeleteFile(model.IMAGENAME);
                    }
                }
                var post = new TblChallenge()
                {
                    FLAG = model.FLAG,
                    CHALLENGE_ID = model.CHALLENGE_ID,
                    CAT_ID = model.CAT_ID,
                    NAME = model.NAME,
                    DESCRIPTION = model.DESCRIPTION,
                    DIFFICULTY_LEVEL=model.DIFFICULTY_LEVEL,
                    POINTS = model.POINTS,
                    CTF_FLAG=model.CTF_FLAG,
                    CREATE_BY = CurrentUser.Name,
                    UPDATE_BY = CurrentUser.Name,
                    IMAGE = bytes,
                    IMAGENAME = ImageName,
                    IS_ENABLE = model.IS_ENABLE,
                    HINT_1 = model.HINT_1,
                    HINT_2 = model.HINT_2,
                    HINT_3 = model.HINT_3,
                    INTENDED_LEARNING = model.INTENDED_LEARNING,
                    CHALLENGE_SOLUTION = model.CHALLENGE_SOLUTION
                };
                var ret = _ich.SaveChallenge(post);
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
            BindDropDown(model);
            return View(model);
        }
        public JsonResult VerifyChallenge(string Id, string UserName)
        {
            if (!String.IsNullOrEmpty(Id))
            {
                var verify = _ich.VerifyChallenge(Id, UserName);
                return Json(verify);
            }
            else
            {
                return Json(new { CODE = "4001", MESSAGE = "No data found to delete" });
            }
        }
        private void BindDropDown(ChallengeModel model)
        {
            Dictionary<string, string> Cat_List = _icr.GetCategoryList();
            ViewData["CAT_ID"] = StaticData.SetDDLValue(Cat_List, model.CAT_ID, "Select Category");
        }
        public void DeleteFile(string FileNmae)
        {
            if (!string.IsNullOrEmpty(FileNmae))
            {
                string folderPath = Server.MapPath(_func.GetAppsettingValue("ChallengePath") + "/" + FileNmae);
                try
                {
                    if (System.IO.File.Exists(folderPath))
                    {
                        System.IO.File.Delete(folderPath);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}