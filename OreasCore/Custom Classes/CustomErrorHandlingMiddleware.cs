using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace OreasCore
{

    public class CustomErrorHandlingMiddleware : IMiddleware
    {
       

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (SqlException sqlE)
            {
                // await this.result(context, "There is some thing went wrong in DB, Please contact Technical Team to troubleshoot");
                await this.result(context, sqlE.InnerException != null ? sqlE.InnerException.Message : sqlE.Message ?? "There is some technical difficulty found, Please Contact Technical Team to troubleshoot");
            }
            catch (Exception e)
            {
                await this.result(context, e.InnerException !=null ? e.InnerException.Message : e.Message ?? "There is some technical difficulty found, Please Contact Technical Team to troubleshoot");
                
            }

        }

        private async Task result(HttpContext context, string ExceptioMessage="")
        {
            //500=HttpStatusCode.InternalServerError
            context.Response.StatusCode = 500;

            bool isAjaxCall = context.Request.Headers["x-requested-with"] == "XMLHttpRequest";

            if (!isAjaxCall)
            {

                //await context.Response.WriteAsync("sss");

                context.Response.Redirect("/Home/Error?Msg=" + ExceptioMessage);

            }
            else
            {
                await context.Response.WriteAsync(ExceptioMessage);
            }
        }
    }



}
