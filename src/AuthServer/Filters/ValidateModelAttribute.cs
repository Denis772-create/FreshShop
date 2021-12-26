using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthServer.Filters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;
            context.Result = (IActionResult)new BadRequestObjectResult(context.ModelState);
        }
    }
}
