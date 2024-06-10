using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using OreasModel;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using System.IO;
using System;

namespace OreasCore.Areas.WPT.Controllers
{
    [Area("WPT")]
    public class GeneralController : Controller
    {

        #region Email

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedEmailAsync([FromServices] IAuthorizationScheme db, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "EmailIndexCtlr",
                        WildCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Email"),
                        Otherdata = new
                        {
                            SectionList = await IList.GetSectionListAsync("General",null,null,0,null),
                            DesignationList = await IList.GetDesignationListAsync(null,null)
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Email", Operation = "CanView")]
        public IActionResult EmailIndex()
        {
            return View();
        }

        [MyAuthorization(FormName = "Email", Operation = "CanView")]
        public async Task<IActionResult> GetGroupEmailList([FromServices] IWPTList db, int DesignationID = 0, int DepartmentID = 0)
        {
            return Json(await db.GetGroupEmailListAsync(DesignationID,DepartmentID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Email", Operation = "CanPost")]
        public async Task<string> SendMail([FromBody] VM_Email VM_Email, string operation = "")
        {
            string rtn = "Failed";
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Rpt_Shared.LicenseToEmailHostName, Rpt_Shared.LicenseToEmailPortNo, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(Rpt_Shared.LicenseToEmail, Rpt_Shared.LicenseToEmailPswd);

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(Rpt_Shared.LicenseTo, Rpt_Shared.LicenseToEmail));
                message.Subject = VM_Email.Subject;

                var builder = new BodyBuilder();

                VM_Email.MailBody = VM_Email.MailBody.Replace("\n", "<br>");


                builder.HtmlBody = VM_Email.MailBody + (VM_Email.WithFooter ?  "<hr>" + Rpt_Shared.LicenseToEmailFooter.Replace("@whatsapp", "Hi; ") : "");

                /// Now we just need to set the message body and we're done
                message.Body = builder.ToMessageBody();

                foreach (var item in VM_Email.VM_EmailAddresses)
                {
                    if (item.EmailType == "To")
                        message.To.Add(new MailboxAddress(item.EmailAddress, item.EmailAddress));
                    else if (item.EmailType == "Cc")
                        message.Cc.Add(new MailboxAddress(item.EmailAddress, item.EmailAddress));
                    else if (item.EmailType == "Bcc")
                        message.Bcc.Add(new MailboxAddress(item.EmailAddress, item.EmailAddress));
                }

                await client.SendAsync(message);
                rtn = "Successfully";
            }

                return rtn;
        }

        #endregion
    }
}
