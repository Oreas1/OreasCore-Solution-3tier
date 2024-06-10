using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OreasCore
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                context.Result = new ContentResult
                {
                    Content = "This is not a valid Ajax Request", StatusCode=500
             
                }; 
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
