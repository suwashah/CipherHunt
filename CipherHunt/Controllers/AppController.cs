using System.Security.Claims;
using System.Web.Mvc;
using CipherHunt.Authentication;

namespace CipherHunt.Controllers
{
    public abstract class AppController : Controller
    {
        // GET: App
        public AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(base.User as ClaimsPrincipal);
            }
        }
    }
}