using Repository.Common;
using System.Web.Mvc;

namespace CipherHunt.Filters
{
    public class CustomHandleErrorAttribute:HandleErrorAttribute
    {
        CommonRepository icr;
        public CustomHandleErrorAttribute()
        {
            icr = new CommonRepository();       
        }
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                var exception = filterContext.Exception;
                var se=icr.SaveError(exception.Message, exception.Source);
                filterContext.ExceptionHandled = true;
                filterContext.Result = new RedirectResult("~/Error/GeneralError?error_id="+se.UNIQUEID);
            }
        }
    }
}