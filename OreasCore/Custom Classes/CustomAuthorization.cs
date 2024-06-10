using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OreasCore.Custom_Classes
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.EntityFrameworkCore;
    using OreasServices;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    namespace OreasCore.Custom_Classes
    {

        public class MyAuthorizationAttribute : AuthorizeAttribute
        {            
            public string FormName = "";
            public string Operation = "";
        }
        public class CustomAuthorizationRequirement : IAuthorizationRequirement
        {
   
        }
        public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
        {
            private readonly OreasDbContext db;

            public CustomAuthorizationHandler([FromServices] OreasDbContext db)
            {                
                this.db = db;                
            }           

            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
            {
                //-----------------get operation and form name attributes for net5.0----------------//
                if (context.Resource is Microsoft.AspNetCore.Http.DefaultHttpContext httpContext)
                {
                    //---------------------------if not log in------------------//
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        if (httpContext.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                        {
                            httpContext.Response?.OnStarting(async () =>
                            {
                                httpContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                                httpContext.HttpContext.Response.Clear();
                                await httpContext.HttpContext.Response.WriteAsync("Not Authenticated, Please Sign-In into the application ");
                                await httpContext.HttpContext.Response.CompleteAsync();
                            });                            
                        }
                        else
                        {
                            httpContext.Response?.OnStarting(async () =>
                            {
                                httpContext.HttpContext.Response.Redirect("/Home/Index"+ "?ReturnUrl=" + httpContext.Request.Path.Value);
                                await httpContext.HttpContext.Response.CompleteAsync();
                            }); 
                        }
                        context.Fail();
                    }
                    else if (httpContext.GetEndpoint() is Endpoint routeEndpoint)
                    {
                        string AreaName = httpContext.HttpContext?.Request?.RouteValues["Area"]?.ToString() ?? "";
                        string FormName = routeEndpoint.Metadata.OfType<MyAuthorizationAttribute>()?.FirstOrDefault()?.FormName ?? "";
                        //---if operation is CanPost then consider posting opertion from querystring i.e save new, save update, save delete------//
                        string OperationName = routeEndpoint.Metadata.OfType<MyAuthorizationAttribute>()?.FirstOrDefault()?.Operation ?? "";
                        if (OperationName == "CanPost")
                            OperationName = httpContext.HttpContext.Request.Query["operation"].ToString() ?? "";
                        
                        string UserName = httpContext.HttpContext.User.Identity.Name ?? "";
                        
                        if (string.IsNullOrEmpty(FormName))
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            var resultPara = new Microsoft.Data.SqlClient.SqlParameter("@result", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                            
                            db.Database.ExecuteSqlRaw("SELECT @result=[dbo].[UDSF_IsUserAuthorized] ({0},{1},{2},{3})", AreaName.ToLower(), UserName, OperationName.ToLower(), FormName.ToLower(), resultPara);
                            
                            if ((bool?)resultPara.Value ?? false)
                            {
                                context.Succeed(requirement);
                            }
                            else
                            {
                                if (httpContext.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
                                    {
                                        httpContext.Response?.OnStarting(async () =>
                                        {                                           
                                            httpContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                                            httpContext.HttpContext.Response.Clear();
                                            await httpContext.HttpContext.Response.WriteAsync("Access Denied on " + FormName + " for operation " + OperationName);
                                            await httpContext.HttpContext.Response.CompleteAsync();
                                        });                                       
                                    }
                                else
                                    {
                                        httpContext.Response?.OnStarting(async () =>
                                        {
                                            httpContext.HttpContext.Response.Redirect("/Home/AccessDenied");
                                            await httpContext.HttpContext.Response.CompleteAsync();
                                        }); 
                                    }
                                context.Fail();
                            }
                        }
                    }
                }    

                return Task.CompletedTask;
            }
        }
    }



}
