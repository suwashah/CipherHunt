using Repository.Challenge;
using System;
using System.Collections.Generic;
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
        public ActionResult SubmitFlag()
        {
        }
    }
}