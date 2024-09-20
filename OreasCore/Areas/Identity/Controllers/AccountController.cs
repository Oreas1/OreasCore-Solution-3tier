using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Models;
using OreasModel;
using OreasServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OreasCore.Areas.Identity.Controllers
{
    [Area("Identity")]
    
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;


        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }
     
        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetUserAuthorizationOnOperationAsync([FromServices] IAuthorizationScheme db, string ForArea = "", string ForForm = "")
        {
            return Json(await db.GetUserAuthorizatedOnOperationAsync(ForArea, User.Identity.Name, ForForm), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #region LogIn-LogOut
        [AjaxOnly]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync([FromServices] SignInManager<ApplicationUser> signInManager, [FromServices] IAuthorizationScheme db,
            [FromBody] LoginModel loginModel = null)
        {
            try
            {
                loginModel.returnUrl = Url.IsLocalUrl(loginModel.returnUrl) && !string.IsNullOrEmpty(loginModel.returnUrl) ? loginModel.returnUrl : Url.Content("~/");

                if (ModelState.IsValid || User.Identity.IsAuthenticated)
                {

                    ApplicationUser user = await signInManager.UserManager.FindByNameAsync(loginModel.UserName);
                    if (user == null) 
                    {
                        return new ContentResult
                        {
                            Content = $"User Name is Invalid \nPlease Provide valid User Name",
                            StatusCode = StatusCodes.Status500InternalServerError
                        };
                    }

                    var result = await signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);
                  
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        var authorizedAreaList = await db.GetUserAuthorizatedAreaListAsync(loginModel.UserName);
                        //------------cookie approach--------------//
                        var AuthorizedWareHouses = await db.GetAspNetOreasAuthorizedStoreListAsync(loginModel.UserName);

                        string serializedStoreList = JsonConvert.SerializeObject(AuthorizedWareHouses);
                        HttpContext.Response.Cookies.Delete("AuthWareHouseList");

                        var cookieOptions = new CookieOptions
                        {
                            Expires = DateTime.MaxValue
                        };

                        HttpContext.Response.Cookies.Append("AuthWareHouseList", serializedStoreList, cookieOptions);
                        //--------------xxxxxxxx--------------------------//
                        return Json(new { redirectUrl = loginModel.returnUrl, AuthorizedAreaList = authorizedAreaList });
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = loginModel.returnUrl, RememberMe = loginModel.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        
                        return new ContentResult
                        {
                            Content = $"Password is Invalid \nPlease Provide valid Password",
                            StatusCode = StatusCodes.Status500InternalServerError
                        };
                    }
                }

                return View();
            }
            catch(SqlException sqlE)
            {
                string responseMessage = "";

                if(sqlE.Number == 4060)
                    responseMessage = "We are currently experiencing some technical difficulties accessing our database.\n Please try again later or contact support if the issue persists.";

                return new ContentResult
                {
                    Content = $"{responseMessage}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (Exception ex)
            {

                return new ContentResult
                {
                    Content = $"An error occurred: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


        }

        [AjaxOnly]
        public async Task<IActionResult> LogoutAsync([FromServices] SignInManager<ApplicationUser> signInManager)
        {
            await signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Delete("AuthWareHouseList");
            //_logger.LogInformation("User logged out.");

            return Json(new { redirectUrl = "/" });
        }

        #endregion

        #region user

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedUserAsync([FromServices] IAuthorizationScheme db, [FromServices] IUser dbUser)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "UserIndexCtlr",
                        WildCard = dbUser.GetWCLUser(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Identity", User.Identity.Name, "Users"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Users", Operation = "CanView")]
        public IActionResult UserIndex()
        {

            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Users", Operation = "CanView")]
        public async Task<IActionResult> UserLoad([FromServices] IUser db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Users", Operation = "CanPost")]
        public async Task<string> UserPostAsync([FromServices] IUser db, [FromBody] UserViewModel userViewModel, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostAsync(userViewModel, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Users", Operation = "CanView")]
        public async Task<IActionResult> UserGetAsync([FromServices] IUser db, string id)
        {
            return Json(await db.GetAsync(id), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion
        
        #region Area
        [MyAuthorization]
        public async Task<IActionResult> AreaListAsync([FromServices] IArea db, string FilterByText = null, string FilterValueByText = null)
        {
            return Json(await db.GetAreaListAsync(FilterByText, FilterValueByText), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization]
        public async Task<IActionResult> FormListAsync([FromServices] IArea db, string LoadByText = null, string LoadValueByText = null, string FilterByText = null, string FilterValueByText = null)
        {
            return Json(await db.GetFormsListAsync(LoadByText, LoadValueByText,FilterByText,FilterValueByText), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region Authorization Scheme Managment

        [MyAuthorization]
        public async Task<IActionResult> AuthorizationSchemeListAsync([FromServices] IAuthorizationScheme db, string FilterByText = null, string FilterValueByText = null)
        {
            return Json(await db.GetAuthorizationSchemeListAsync(FilterByText, FilterValueByText), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedSchemeAsync([FromServices] IAuthorizationScheme db, [FromServices] IArea dbArea, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AuthorizationSchemeCtlr",                        
                        WildCard = db.GetWCLAuthorizationScheme(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Identity", User.Identity.Name, "Authorization Scheme"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AuthorizationSchemeWHMCtlr",
                        WildCard=db.GetWCLAuthorizationSchemeWHM(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AuthorizationSchemeSectionCtlr",
                        WildCard=db.GetWCLAuthorizationSchemeSection(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { SectionList = await IList.GetSectionListAsync("General",null,null,0,null) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AuthorizationSchemeAreaCtlr",
                        WildCard=db.GetWCLAuthorizationSchemeArea(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { AreaList=await dbArea.GetAreaListAsync(null, null) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AuthorizationSchemeAreaFormCtlr",
                        WildCard=db.GetWCLAuthorizationSchemeAreaForm(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Scheme
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public IActionResult AuthorizationSchemeIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> AuthorizationSchemeLoad([FromServices] IAuthorizationScheme db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAuthorizationScheme(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);      

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanPost")]
        public async Task<string> AuthorizationSchemePostAsync([FromServices] IAuthorizationScheme db, [FromBody] AspNetOreasAuthorizationScheme aspNetOreasAuthorizationScheme, string operation = "")
        {

            if (ModelState.IsValid)
                return await db.PostAuthorizationSchemeAsync(aspNetOreasAuthorizationScheme, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);

        }

        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public async Task<IActionResult> AuthorizationSchemeGetAsync([FromServices] IAuthorizationScheme db, int ID)
        {
            return Json(await db.GetAuthorizationSchemeAsync(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region Scheme Sections

        [AjaxOnly]
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public async Task<IActionResult> AuthorizationSchemeSectionLoad([FromServices] IAuthorizationScheme db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAuthorizationSchemeSection(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanPost")]
        public async Task<string> AuthorizationSchemeSectionPostAsync([FromServices] IAuthorizationScheme db, [FromBody] AspNetOreasAuthorizationScheme_Section AspNetOreasAuthorizationScheme_Section, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostAuthorizationSchemeSectionAsync(AspNetOreasAuthorizationScheme_Section, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public async Task<IActionResult> AuthorizationSchemeSectionGetAsync([FromServices] IAuthorizationScheme db, int id)
        {
            return Json(await db.GetAuthorizationSchemeSectionAsync(id), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Scheme WareHouse

        [AjaxOnly]
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public async Task<IActionResult> AuthorizationSchemeWHMLoad([FromServices] IAuthorizationScheme db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAuthorizationSchemeWHM(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanPost")]
        public async Task<string> AuthorizationSchemeWHMPostAsync([FromServices] IAuthorizationScheme db, [FromBody] AspNetOreasAuthorizationScheme_WareHouse AspNetOreasAuthorizationScheme_WareHouse, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostAuthorizationSchemeWHMAsync(AspNetOreasAuthorizationScheme_WareHouse, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public async Task<IActionResult> AuthorizationSchemeWHMGetAsync([FromServices] IAuthorizationScheme db, int id)
        {
            return Json(await db.GetAuthorizationSchemeWHMAsync(id), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Scheme Area

        [AjaxOnly]
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public async Task<IActionResult> AuthorizationSchemeAreaLoad([FromServices] IAuthorizationScheme db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAuthorizationSchemeArea(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanPost")]
        public async Task<string> AuthorizationSchemeAreaPostAsync([FromServices] IAuthorizationScheme db, [FromBody] AspNetOreasAuthorizationScheme_Area aspNetOreasAuthorizationScheme_Area, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostAuthorizationSchemeAreaAsync(aspNetOreasAuthorizationScheme_Area, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public async Task<IActionResult> AuthorizationSchemeAreaGetAsync([FromServices] IAuthorizationScheme db, int id)
        {
            return Json(await db.GetAuthorizationSchemeAreaAsync(id), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Scheme Area Form

        [AjaxOnly]
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public async Task<IActionResult> AuthorizationSchemeAreaFormLoad([FromServices] IAuthorizationScheme db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAuthorizationSchemeAreaForm(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanPost")]
        public async Task<string> AuthorizationSchemeAreaFormPostAsync([FromServices] IAuthorizationScheme db, [FromBody] AspNetOreasAuthorizationScheme_Area_Form aspNetOreasAuthorizationScheme_Area_Form, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostAuthorizationSchemeAreaFormAsync(aspNetOreasAuthorizationScheme_Area_Form, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Authorization Scheme", Operation = "CanView")]
        public async Task<IActionResult> AuthorizationSchemeAreaFormGetAsync([FromServices] IAuthorizationScheme db, int id)
        {
            return Json(await db.GetAuthorizationSchemeAreaFormAsync(id), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #endregion

        #region General Settings

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedGeneralSettingsAsync([FromServices] IAuthorizationScheme db, [FromServices] IAspNetOreasGeneralSettings db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "GeneralSettingsIndexCtlr",
                        WildCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Identity", User.Identity.Name, "General Settings"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "GeneralSettingsProductTypeCtlr",
                        WildCard = db2.GetWCLProductType(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }


        [MyAuthorization(FormName = "General Settings", Operation = "CanView")]
        public IActionResult GeneralSettingsIndex()
        {

            if (User.Identity.Name.ToLower() == "ovais")
                return View();
            else
                return Redirect("/AccessDenied");
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "General Settings", Operation = "CanView")]
        public async Task<IActionResult> GeneralSettingsLoad([FromServices] IAspNetOreasGeneralSettings db)
        {
            return Json(await db.Load(), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "General Settings", Operation = "CanPost")]
        public async Task<string> GeneralSettingsPost([FromServices] IAspNetOreasGeneralSettings db, [FromBody] AspNetOreasGeneralSettings AspNetOreasGeneralSettings, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(AspNetOreasGeneralSettings, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #region Auto Rate / with out Purchase note & Order Note Policy

        [AjaxOnly]
        [MyAuthorization(FormName = "General Settings", Operation = "CanView")]
        public async Task<IActionResult> ProductTypeLoad([FromServices] IAspNetOreasGeneralSettings db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductType(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "General Settings", Operation = "CanPost")]
        public async Task<string> ProductTypePost([FromServices] IAspNetOreasGeneralSettings db, [FromBody] tbl_Inv_ProductType tbl_Inv_ProductType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostProductType(tbl_Inv_ProductType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "General Settings", Operation = "CanView")]
        public async Task<IActionResult> ProductTypeGet([FromServices] IAspNetOreasGeneralSettings db, int ID)
        {
            return Json(await db.GetProductType(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region Company Profile

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCompanyProfileAsync([FromServices] IAuthorizationScheme db)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompanyProfileIndexCtlr",
                        WildCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Identity", User.Identity.Name, "Company Profile"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }


        [MyAuthorization(FormName = "Company Profile", Operation = "CanView")]
        public IActionResult CompanyProfileIndex()
        {

            if (User.Identity.Name.ToLower() == "ovais")
                return View();
            else
                return Redirect("/AccessDenied");
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Company Profile", Operation = "CanView")]
        public async Task<IActionResult> CompanyProfileLoad([FromServices] IAspNetOreasCompanyProfile db)
        {
            return Json(await db.Load(), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Company Profile", Operation = "CanPost")]
        public async Task<string> CompanyProfilePost([FromServices] IAspNetOreasCompanyProfile db, AspNetOreasCompanyProfile AspNetOreasCompanyProfile, string operation = "", IFormFile LicenseByLogofile = null, IFormFile LicenseToLogofile = null)
        {

    
            if (LicenseByLogofile != null)
                using (var ms = new MemoryStream())
                {
                    LicenseByLogofile.CopyTo(ms);
                    AspNetOreasCompanyProfile.LicenseByLogo = ms.ToArray();
                }
            else
                AspNetOreasCompanyProfile.LicenseByLogo = new byte[] { };

            if (LicenseToLogofile != null)
                using (var ms = new MemoryStream())
                {
                    LicenseToLogofile.CopyTo(ms);
                    AspNetOreasCompanyProfile.LicenseToLogo = ms.ToArray();
                }
            else
                AspNetOreasCompanyProfile.LicenseToLogo = new byte[] { };

            ModelState.Remove("AspNetOreasCompanyProfile.LicenseByLogo");
            ModelState.Remove("AspNetOreasCompanyProfile.LicenseToLogo");

            if (ModelState.IsValid)
                return await db.Post(AspNetOreasCompanyProfile, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }
        #endregion

    }
}
