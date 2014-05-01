using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using IkoulaACDF.Models;
using IkoulaACDF.Helpers;
using System.IO;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Net.Mail;

namespace IkoulaACDF.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        // New Index Method:
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var Db = new ApplicationDbContext();
            var users = Db.Users.OrderBy(u => u.RegistrationDate);
            var model = new List<EditUserViewModel>();
            foreach (var user in users)
            {
                var u = new EditUserViewModel(user);
                model.Add(u);
            }
            ViewBag.CountMembers = model.Count();
            return View(model);
        }

        public ActionResult RestrictedUsersList()
        {
            var Db = new ApplicationDbContext();
            var users = Db.Users.Where(u => u.ConfirmedEmail == true);
            var model = new List<RestrictedUsersListViewModel>();
            foreach (var user in users)
            {
                var u = new RestrictedUsersListViewModel(user);
                model.Add(u);
            }
            ViewBag.CountMembers = model.Count();
            return View(model);

        }
        // New Edit Method:
        //[Authorize(Roles = "User")]
        public ActionResult Edit(string id, EditMessageID? message = null)
        {
            ViewBag.StatusMessage =
                    message == EditMessageID.ModifSuccess ? "Vos modifications ont été prises en compte."
                    : message == EditMessageID.Error ? "Une erreur s'est produite."
                    : "";

            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            // Dont let user edit another user if not Admin
            if((user.UserName != User.Identity.Name) && (! User.IsInRole("Admin")))
            {
                ModelState.AddModelError("", "You are not" + user.UserName);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
            }
            var model = new EditUserViewModel(user);
            return View(model);
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "User")]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == model.UserName);

            if (ModelState.IsValid)
            {
                if ((!user.PhotoUrl.Contains("BlankPhoto.jpg")) && model.IsNoPhotoChecked)
                {
                    string fileToDelete = Path.GetFileName(user.PhotoUrl);
                    var path = Path.Combine(Server.MapPath("~/uploads"), fileToDelete);
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                        fi.Delete();
                }
                model.PhotoUrl = Utils.SavePhotoFileToDisk(model.Photo, this, user.PhotoUrl, model.IsNoPhotoChecked);
                user.PhotoUrl = model.PhotoUrl;
                // Dont delete BlankPhoto.jpg

                // Update the user data:
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.BirthDate = model.BirthDate;
                user.FirstYearSchool = model.FirstYearSchool;
                user.LastYearSchool = model.LastYearSchool;
                user.LastClass = model.LastClass;
                user.ActualCity = model.ActualCity;
                user.ActualCountry = model.ActualCountry;
                Db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Edit", new { Message = EditMessageID.ModifSuccess });
            }
            model.PhotoUrl = user.PhotoUrl;
            //// Generate Years
            List<System.Web.Mvc.SelectListItem> years = new List<System.Web.Mvc.SelectListItem>();
            for (int y = 1900; y <= DateTime.Now.Year; y++)
            {
                years.Add(new System.Web.Mvc.SelectListItem { Value = y.ToString(), Text = y.ToString() });
            };
            model.YearFirst = (IEnumerable<System.Web.Mvc.SelectListItem>)years;
            model.YearLast = (IEnumerable<System.Web.Mvc.SelectListItem>)years;

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // New Delete Methods:
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id = null)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new EditUserViewModel(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            // Dont delete admin or dev
            if ((id != "admin") && (id != "dev"))
            {
                var Db = new ApplicationDbContext();
                var user = Db.Users.First(u => u.UserName == id);
                // Dont delete BlankPhoto.jpg
                if (!user.PhotoUrl.Contains("BlankPhoto.jpg"))
                {
                    string fileToDelete = Path.GetFileName(user.PhotoUrl);
                    var path = Path.Combine(Server.MapPath("~/uploads"), fileToDelete);
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                        fi.Delete();
                }
                Db.Users.Remove(user);
                Db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // New UserRoles Method(s):
        [Authorize(Roles = "Admin")]
        public ActionResult UserRoles(string id)
        {
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.UserName == id);
            var model = new SelectUserRolesViewModel(user);
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult UserRoles(SelectUserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var idManager = new IdentityManager();
                var Db = new ApplicationDbContext();
                var user = Db.Users.First(u => u.UserName == model.UserName);
                idManager.ClearUserRoles(user.Id);
                foreach (var role in model.Roles)
                {
                    if (role.Selected)
                    {
                        idManager.AddUserToRole(user.Id, role.RoleName);
                    }
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    //await SignInAsync(user, model.RememberMe);
                    //return RedirectToLocal(returnUrl);
                    if (user.ConfirmedEmail == true)
                    {
                        await SignInAsync(user, model.RememberMe); return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Confirm Email Address.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // Si nous sommes arrivés là, un échec s’est produit. Réafficher le formulaire
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            model.PhotoUrl = System.IO.Path.Combine(HttpRuntime.AppDomainAppVirtualPath, "uploads", "BlankPhoto.jpg");
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                // Save file to disk and retreive calculated file name or null if handled exception occure
                // if user don't provide photo then he don't want photo
                model.PhotoUrl = Utils.SavePhotoFileToDisk(model.Photo, this, null, model.Photo == null ? true : false);

                // Ajout
                var user = model.GetUser();
                user.FirstName = user.FirstName.ToLower();
                user.LastName = user.LastName.ToUpper();
                user.ActualCity = user.ActualCity.ToUpper();
                user.ActualCountry = user.ActualCountry.ToUpper();
                user.RegistrationDate = DateTime.Now;
                if (User.IsInRole("Admin"))
                {
                    user.ConfirmedEmail = true;
                }
                else
                {
                    user.ConfirmedEmail = false;
                }
                //var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var idManager = new IdentityManager();
                    idManager.AddUserToRole(user.Id, "User");
                    idManager.AddUserToRole(user.Id, "CanEdit");
                    if(!User.IsInRole("Admin"))
                    {
                        //await SignInAsync(user, isPersistent: false);

                        // Send email to new user
                        //Helpers.Utils.SendMail("", "", user.UserName, model.Password, model.Email);

                        //return RedirectToAction("Create", "GuessBook");

                        using (var client = new System.Net.Mail.SmtpClient())
                        {
                            MailDefinition md = new MailDefinition();
                            md.IsBodyHtml = true;
                            md.Subject = "ACDF mail confirmation";

                            ListDictionary replacements = new ListDictionary();
                            replacements.Add("<%Name%>", user.UserName);
                            replacements.Add("<%Link%>", Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));

                            string body = "Hello <%Name%> <BR/> Thank you for your registration, please click on the below link to complete your registration:. <BR/> <%Link%>";

                            MailMessage msg = md.CreateMailMessage(user.Email, replacements, body, new System.Web.UI.Control());
                            client.Send(msg);

                            System.Diagnostics.Debug.WriteLine(msg);
                        }
                        return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Account");
                    }
                }
                else
                {
                    AddErrors(result);
                }
            }

            // Si nous sommes arrivés là, un échec s’est produit. Réafficher le formulaire
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            ApplicationUser user = this.UserManager.FindById(Token);
            if (user != null)
            {
                if (user.ConfirmedEmail == false)
                {
                    if (user.Email == Email)
                    {
                        user.ConfirmedEmail = true;
                        await UserManager.UpdateAsync(user);
                        await SignInAsync(user, isPersistent: false);
                        // Send mail to track
                        Helpers.Utils.SendMail("", "", user.FirstName, user.LastName, user.Email);
                        return RedirectToAction("Create", "GuessBook");
                    }
                    else
                    {
                        return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                    }
                    
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { Email = "" });
            }

        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        [Authorize(Roles = "User")]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Votre mot de passe a été modifié."
                : message == ManageMessageId.SetPasswordSuccess ? "Votre mot de passe a été défini."
                : message == ManageMessageId.RemoveLoginSuccess ? "La connexion externe a été supprimée."
                : message == ManageMessageId.Error ? "Une erreur s'est produite."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // Si nous sommes arrivés là, un échec s’est produit. Réafficher le formulaire
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Demandez une redirection vers le fournisseur de connexions externe
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Obtenez des informations sur l’utilisateur auprès du fournisseur de connexions externe
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region MyApplications auxiliaires
        public enum EditMessageID
        {
            ModifSuccess,
            Error,
        }
        #endregion

        #region Applications auxiliaires
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}