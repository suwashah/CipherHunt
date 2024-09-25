using System.Security.Claims;
using System.Web.Mvc;
using CipherHunt.Authentication;

namespace CipherHunt.Areas.Cpanel.Controllers
{
    public abstract class CAppController : Controller
    {
        public CPanelAppUserPrincipal CurrentUser
        {
            get
            {
                return new CPanelAppUserPrincipal(base.User as ClaimsPrincipal);
            }
        }
    }
}