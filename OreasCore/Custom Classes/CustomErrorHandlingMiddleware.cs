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
                if (sqlE.Number == 4060)
                {
                    await this.result(context, "We are currently experiencing some technical difficulties accessing our database.\nPlease try again later or contact support if the issue persists.");
                    return;
                }
                else
                {
                    await this.result(context, "There is some thing went wrong in DB, Please contact Technical Team to troubleshoot");
                    return;
                }
                
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

                ExceptioMessage = ExceptioMessage.Replace("\n", "<br/>"); /// ager line change hai tou ose html main line break karain
                ExceptioMessage = Uri.EscapeDataString(ExceptioMessage); /// phir wo url k through hoga error page k so ose uri format main kar dia

                context.Response.ContentType = "text/html";
                context.Response.Redirect("/Home/Error?Msg=" + ExceptioMessage);

            }
            else
            {
                //context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(ExceptioMessage);
            }
        }
    }



}
