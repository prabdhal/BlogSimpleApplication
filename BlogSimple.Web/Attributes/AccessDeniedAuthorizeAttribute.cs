using System.Web.Mvc;

namespace BlogSimple.Web.Attributes;

public class AccessDeniedAuthorizeAttribute : AuthorizeAttribute
{
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        base.OnAuthorization(filterContext);

        if (filterContext.Result is HttpUnauthorizedResult)
        {
            filterContext.Result = new RedirectResult("~/AccessDenied");
        }
    }
}
