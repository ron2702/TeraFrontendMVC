using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TeraFrontendMVC.Filter
{
    public class RedirectIfNotAuthenticatedFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var checkToken = context.HttpContext.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(checkToken))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No es necesario implementar nada aquí para este caso
        }
    }
}
