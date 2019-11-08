using System.Linq;
using System.Threading.Tasks;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.Controllers;
using FluiTec.AppFx.Identity.Entities;
using FluiTec.AppFx.Identity.Models.ManageViewModels;
using FluiTec.AppFx.Mail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Controllers
{
    /// <summary>A controller for handling profiles.</summary>
    [Authorize]
    public class ProfileController : Controller
    {
        #region Constructors

        /// <summary>Constructor.</summary>
        /// <param name="userManager">      Manager for user. </param>
        /// <param name="signInManager">    Manager for sign in. </param>
        /// <param name="emailSender">      The email sender. </param>
        /// <param name="loggerFactory">    The logger factory. </param>
        /// <param name="localizer">        The localizer. </param>
        public ProfileController(
            UserManager<IdentityUserEntity> userManager,
            SignInManager<IdentityUserEntity> signInManager,
            ITemplatingMailService emailSender,
            ILoggerFactory loggerFactory,
            IStringLocalizer<ProfileResource> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ProfileController>();
            _localizer = localizer;
        }

        #endregion

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var model = new IndexViewModel
            {
                StatusMessage = ProfileResource.FromManageMessage(_localizer, message),
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };
            return View(model);
        }

        #endregion

        #region Fields

        /// <summary>Manager for user.</summary>
        private readonly UserManager<IdentityUserEntity> _userManager;

        /// <summary>Manager for sign in.</summary>
        private readonly SignInManager<IdentityUserEntity> _signInManager;

        /// <summary>The logger.</summary>
        private readonly ILogger _logger;

        /// <summary>The localizer.</summary>
        private readonly IStringLocalizer<ProfileResource> _localizer;

        #endregion

        #region DeleteAccount

        [HttpGet]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountNow()
        {
            var user = await GetCurrentUserAsync();

            await _signInManager.SignOutAsync();
            await _userManager.DeleteAsync(user);

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Logins

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            ManageMessageId? message = ManageMessageId.Error;
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction(nameof(ManageLogins), new {Message = message});
            var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
            if (!result.Succeeded) return RedirectToAction(nameof(ManageLogins), new {Message = message});
            await _signInManager.SignInAsync(user, false);
            message = ManageMessageId.RemoveLoginSuccess;
            return RedirectToAction(nameof(ManageLogins), new {Message = message});
        }

        [HttpGet]
        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var userLogins = await _userManager.GetLoginsAsync(user);
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = schemes.Where(auth => userLogins.All(ul => auth.Name != ul.ProviderDisplayName)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                StatusMessage = ProfileResource.FromManageMessage(_localizer, message),
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback), "Profile");
            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl,
                    _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return RedirectToAction(nameof(ManageLogins), new {Message = ManageMessageId.Error});
            }

            var result = await _userManager.AddLoginAsync(user, info);
            var message = ManageMessageId.Error;
            if (result.Succeeded)
            {
                message = ManageMessageId.AddLoginSuccess;
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }

            return RedirectToAction(nameof(ManageLogins), new {Message = message});
        }

        #endregion

        #region Phone

        [HttpGet]
        public async Task<IActionResult> AddPhone()
        {
            var user = await GetCurrentUserAsync();
            return View(new AddPhoneViewModel {Phone = user.Phone});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhone(AddPhoneViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.Phone);
            await _userManager.ChangePhoneNumberAsync(user, model.Phone, token);
            return RedirectToAction(nameof(Index), new {Message = ManageMessageId.AddPhoneSuccess});
        }

        #endregion

        #region Password

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return RedirectToAction(nameof(Index), new {Message = ManageMessageId.ChangePasswordSuccess});
                }

                AddErrors(result);
                return View(model);
            }

            return RedirectToAction(nameof(Index), new {Message = ManageMessageId.Error});
        }

        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction(nameof(Index), new {Message = ManageMessageId.SetPasswordSuccess});
                }

                AddErrors(result);
                return View(model);
            }

            return RedirectToAction(nameof(Index), new {Message = ManageMessageId.Error});
        }

        #endregion

        #region Helpers

        /// <summary>Adds the errors.</summary>
        /// <param name="result">   The result. </param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        /// <summary>Gets current user asynchronous.</summary>
        /// <returns>The asynchronous result that yields the current user asynchronous.</returns>
        private Task<IdentityUserEntity> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}