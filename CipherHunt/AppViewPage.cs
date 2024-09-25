using Repository.Common;
using Repository.HelperFunction;
using System.Security.Claims;
using System.Web.Mvc;
using CipherHunt.Authentication;
using Unity;

namespace CipherHunt
{
    public abstract class AppViewPage<TModel> : WebViewPage<TModel>
    {
        protected AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(this.User as ClaimsPrincipal);
            }
        }
        [Dependency]
        public IUtilityHelper _func { get; set; }
        [Dependency]
        public IApplicationConfig _app { get; set; }

    }
    public abstract class AppViewPage : AppViewPage<dynamic>
    {
    }

    public abstract class CAppViewPage<TModel> : WebViewPage<TModel>
    {
        protected CPanelAppUserPrincipal CurrentUser
        {
            get
            {
                return new CPanelAppUserPrincipal(this.User as ClaimsPrincipal);
            }
        }
        [Dependency]
        public IUtilityHelper _func { get; set; }
        [Dependency]
        public IApplicationConfig _app { get; set; }

    }
    public abstract class CAppViewPage : CAppViewPage<dynamic>
    {
    }
}