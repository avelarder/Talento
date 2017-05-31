using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Talento.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Talento.Entities;
using System.Web.Security;
using Talento.EmailManager;
using Talento.Core;
using System.Drawing;

namespace Talento.Controllers
{
    [HandleError]
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IMessenger _emailManager;
        ICustomUser UserHelper;

        public AccountController()
        {
            
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IMessenger emailManager, ICustomUser userhelper)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _emailManager = emailManager;
            UserHelper = userhelper;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult RequestToken()
        {
            return View();
        }

        //


        //// POST: /Account/Login 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RequestToken(string Email)
        {
            try
            {
                MailAddress email = new MailAddress(Email);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "The Email is not a valid email address.");
                return View("RequestToken");
            }
            var user = UserManager.FindByEmail(Email);
            string code = UserManager.GenerateEmailConfirmationToken(user.Id);
            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //_emailManager.SendConfirmationEmail(Email, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            //ModelState.AddModelError("", "Your token has been sent to the email address.");
            if (user != null)
            {
                string iduser = user.Id;
                string token = UserManager.GenerateEmailConfirmationToken(iduser);
                return RegistrationFileDownload(iduser, token);
                //return View("Login");
            }
            else
            {
                ModelState.AddModelError("", "An email has been sent with the url for the account activation.");
                return View("Login");
            }
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (HttpContext.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        //// POST: /Account/Login 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    if (user.EmailConfirmed == true)
                    {
                        await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your email is registered but the Account has not been activated.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Error in login action");
            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            List<string> rolesName = new List<string>();

            foreach (string rol in Roles.GetAllRoles())
            {
                if (rol != "Admin")
                {
                    rolesName.Add(rol);
                }
            }

            ViewBag.Roles = rolesName;
            return View();
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var roles = Roles.GetAllRoles();
            List<string> rolesName = new List<string>();

            foreach (var rol in roles)
            {
                if (rol != "Admin")
                {
                    rolesName.Add(rol);
                }
            }
            ViewBag.Roles = rolesName;

            if (ModelState.IsValid)
            {
                //Check if Role exists
                var role = roles.SingleOrDefault(x => x == model.UserType);
                if (role == null || role == "Admin")
                {
                    ModelState.AddModelError(string.Empty, "Invalid role.");
                    return View(model);
                }



                Random rnd = new Random();
                int dice = rnd.Next(1, 5);


                MemoryStream ms = new MemoryStream();
                Image img = Image.FromFile(Server.MapPath("/Content/Images/alien" + dice + ".png"));
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, ImageProfile = ms.ToArray(), CreatedDate = DateTime.Now };
                var result = await UserManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    var rolesForUser = UserManager.GetRoles(user.Id);

                    if (!rolesForUser.Contains(model.UserType))
                    {
                        result = UserManager.AddToRole(user.Id, model.UserType);
                    }
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //_emailManager.SendConfirmationEmail(model.Email, "Confirm your account", "Talento account confirmation", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //ViewData["RegisterCode"] = HttpUtility.UrlEncode(code);
                    //ViewData["UserId"] = user.Id;

                    return View("Login");

                    //ModelState.AddModelError("", "A link has been sent to your registered mail address. Check for it in order to activate the account before being able to login.");
                    //return View("Login");

                    //return RedirectToAction("Login", "Account");
                }
                AddErrors(result);
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult RegistrationFileDownload(string userId, string code)
        {
            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream);
            tw.WriteLine("[InternetShortcut]\n" + "URL=http://" + HttpContext.Request.Url.Host +
                ":" + HttpContext.Request.Url.Port +
                "/Account/ConfirmEmail/?userId=" + userId + "&code=" + HttpUtility.UrlEncode(code));
            tw.Flush();
            tw.Close();
            return File(memoryStream.GetBuffer(), "application/octet-stream", "ConfirmMyAccount.url");

        }

      
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "Error");
                    // Don't reveal that the user does not exist or is not confirmed
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await _emailManager.SendConfirmationEmail(model.Email, "Talento Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(string code, string mail)
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewData["mail"] = mail;
            ViewData["code"] = code;
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code, string mail)
        {
            ViewData["mail"] = mail;
            ViewData["code"] = code;
            return code == null ? View("Error") : View("ForgotPasswordConfirmation");
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            string mail = string.Empty;
            byte[] decriptedmail = Convert.FromBase64String(model.Email);
            mail = System.Text.Encoding.Unicode.GetString(decriptedmail);
            model.Email = mail;
            ModelState.Clear();
            TryValidateModel(model);
            var user = await UserManager.FindByNameAsync(model.Email);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "An error occurred when trying to reset the password");
                return View("Login");
            }
            if (user == null)
            {
                // Don't reveal that the user does not exist
                ModelState.AddModelError("", "An error occurred when trying to reset the password");
                return View("Login");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                ModelState.AddModelError("", "Your Password has been changed successfully");
                return View("Login");
            }
            AddErrors(result);
            return View("Login");
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/MailSent
        [AllowAnonymous]
        public ActionResult MailSent(string code, string mail)
        {
            ViewData["mail"] = mail;
            ViewData["code"] = code;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SendmeanEmail(ForgotPasswordViewModel model)
        {
            string code = "";
            var callbackUrl = "";
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The Email has been sent, please check your inbox");
                    return View("Login");
                }
                code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                string result = string.Empty;
                byte[] encriptedmail = System.Text.Encoding.Unicode.GetBytes(model.Email);
                result = Convert.ToBase64String(encriptedmail);
                ViewData["mail"] = result;
                ViewData["link"] = callbackUrl;
                ViewData["code"] = code;
                return View("MailSent");

                //string body = "Reset Password. Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";
                // _emailManager.SendConfirmationEmail(model.Email, "talento forgot password confirmation email", body);

            }
            ModelState.Clear();
            ModelState.AddModelError("", "The email does not have valid format of email address.");
            return View("ForgotPassword");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Dashboard");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }



#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
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
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
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