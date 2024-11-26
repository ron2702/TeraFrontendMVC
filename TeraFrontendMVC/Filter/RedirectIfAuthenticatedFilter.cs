using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TeraFrontendMVC.Filter
{
    public class RedirectIfAuthenticatedFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var checkToken = context.HttpContext.Session.GetString("AuthToken");

            if (!string.IsNullOrEmpty(checkToken))
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
