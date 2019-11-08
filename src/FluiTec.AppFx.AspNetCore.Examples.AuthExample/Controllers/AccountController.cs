using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Mail;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.Controllers;
using FluiTec.AppFx.Identity;
using FluiTec.AppFx.Identity.Entities;
using FluiTec.AppFx.Identity.Models.AccountViewModels;
using FluiTec.AppFx.Localization;
using FluiTec.AppFx.Mail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Bcpg;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Controllers
{
    /// <summary>A controller for handling accounts.</summary>
    [Authorize]
    public class AccountController : Controller
    {
        #region Constructors

        /// <summary>   Constructor. </summary>
        /// <param name="userManager">          Manager for user. </param>
        /// <param name="signInManager">        Manager for sign in. </param>
        /// <param name="emailSender">          The email sender. </param>
        /// <param name="loggerFactory">        The logger factory. </param>
        /// <param name="dataService">          The data service. </param>
        /// <param name="adminOptions">         Options for controlling the admin. </param>
        /// <param name="localizer">            The localizer. </param>
        /// <param name="localizerFactory">     The localizer factory. </param>
        /// <param name="identityDataService">  The identity data service. </param>
        /// <param name="applicationOptions">   Options for controlling the application. </param>
        public AccountController(
            UserManager<IdentityUserEntity> userManager,
            SignInManager<IdentityUserEntity> signInManager,
            ITemplatingMailService emailSender,
            ILoggerFactory loggerFactory,
            IIdentityDataService dataService,
            AdminOptions adminOptions,
            IStringLocalizer<AccountResource> localizer,
            IStringLocalizerFactory localizerFactory, IIdentityDataService identityDataService, ApplicationOptions applicationOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _adminOptions = adminOptions;
            _localizer = localizer;
            _localizerFactory = localizerFactory;
            _identityDataService = identityDataService;
            _applicationOptions = applicationOptions;
        }

        #endregion

        #region Logout

        /// <summary>   Logout. </summary>
        /// <returns>   An asynchronous result that yields an IActionResult. </returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion

        #region Fields

        /// <summary>   Manager for user. </summary>
        private readonly UserManager<IdentityUserEntity> _userManager;

        /// <summary>   Manager for sign in. </summary>
        private readonly SignInManager<IdentityUserEntity> _signInManager;

        /// <summary>   The email sender. </summary>
        private readonly ITemplatingMailService _emailSender;

        /// <summary>   The logger. </summary>
        private readonly ILogger _logger;

        /// <summary>   Options for controlling the admin. </summary>
        private readonly AdminOptions _adminOptions;

        /// <summary>   The localizer. </summary>
        private readonly IStringLocalizer<AccountResource> _localizer;

        /// <summary>   The localizer factory. </summary>
        private readonly IStringLocalizerFactory _localizerFactory;

        /// <summary>   The identity data service. </summary>
        private readonly IIdentityDataService _identityDataService;

        /// <summary>   Options for controlling the application. </summary>
        private readonly ApplicationOptions _applicationOptions;

        #endregion

        #region Login

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var tUser = await _userManager.FindByEmailAsync(model.Email);
                if (tUser != null)
                {
                    var result =
                        await _signInManager.PasswordSignInAsync(tUser, model.Password, model.RememberMe, true);

                    var myUser = User;
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(1, "User logged in.");
                        return RedirectToLocal(returnUrl);
                    }

                    if (result.RequiresTwoFactor)
                    {
                        throw new NotImplementedException("TwoFactor-Authentication is not implemented!");
                    }

                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning(2, "User account locked out.");
                        // ReSharper disable once PossibleInvalidOperationException
                        return View("Lockout", tUser.LockedOutTill.Value);
                    }
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, _localizer.GetString(r => r.EmailNotConfirmed));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _localizer.GetString(r => r.InvalidLoginAttempt));
                }

                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new {ReturnUrl = returnUrl});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty,
                    $"{_localizer.GetString(r => r.ExternalProviderError)} {remoteError}");
                return View(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }

            if (result.RequiresTwoFactor)
            {
                throw new NotImplementedException("TwoFactor-Authentication not implemented!");
            }

            if (result.IsLockedOut)
            {
                return View("Lockout");
            }

            // If the user does not have an account, then ask the user to create an account.
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LoginProvider"] = info.LoginProvider;
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            return View(viewName: "ExternalLoginConfirmation",
                model: new ExternalLoginConfirmationViewModel {Email = email});
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new IdentityUserEntity {Name = model.Name, Email = model.Email};
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await SendConfirmationMail(user);

                        // disabled to force the the user to confirm his mail address
                        // await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToAction(nameof(ConfirmEmailNotification));
                    }
                }

                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        #endregion

        #region Register

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateRecaptcha]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                using (var uow = _identityDataService.StartUnitOfWork())
                {
                    if (uow.UserRepository.Count() == 0)
                    {
                        var admin = await RegisterFirstUserAdminWithoutConfirmation(uow, model, returnUrl);
                        if (admin == null)
                            return View(model);
                        await _signInManager.SignInAsync(admin, false);
                        return RedirectToLocal(returnUrl);
                    }
                }

                var user = new IdentityUserEntity { Name = model.Name, Email = model.Email, Phone = model.Phone };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SendConfirmationMail(user);

                    // sign the user in
                    // disabled to force the the user to confirm his mail address
                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToAction(nameof(ConfirmEmailNotification));
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region ConfirmEmail

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmailNotification()
        {
            return View(_adminOptions);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded || _adminOptions.ConfirmationRecipient != MailAddressConfirmationRecipient.Admin)
                return View(result.Succeeded ? "ConfirmEmail" : "Error");

            var mailModel = new AccountConfirmedModel(_localizerFactory, _applicationOptions, Url.Action(nameof(Login)));
            await _emailSender.SendEmailAsync(user.Email, mailModel);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmailAgain()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateRecaptcha]
        public async Task<IActionResult> ConfirmEmailAgain(ConfirmEmailAgainViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is confirmed
                return View("ConfirmEmailAgainConfirmation");
            }

            await SendConfirmationMail(user);

            _logger.LogInformation($"Send emai-confirmation (again) for users {user.Email}.");

            return View("ConfirmEmailAgainConfirmation");
        }

        #endregion

        #region ForgotPassword

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateRecaptcha]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is already confirmed
                return View("ForgotPasswordConfirmation");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new {userId = user.Identifier, code},
                HttpContext.Request.Scheme);
            var mailModel = new RecoverPasswordMailModel(_localizerFactory, _applicationOptions, callbackUrl);
            await _emailSender.SendEmailAsync(model.Email, mailModel);
            _logger.LogInformation($"Send password-recovery mail for user {user.Email}.");
            return View("ForgotPasswordConfirmation");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateRecaptcha]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }

            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region AccessDenied

        /// <summary>Access denied.</summary>
        /// <returns>An IActionResult.</returns>
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion

        #region Helpers

        /// <summary>Registers the first user admin without confirmation.</summary>
        /// <param name="uow">          The uow. </param>
        /// <param name="model">        The model. </param>
        /// <param name="returnUrl">    URL of the return. </param>
        /// <returns>The asynchronous result that yields an IdentityUserEntity.</returns>
        private async Task<IdentityUserEntity> RegisterFirstUserAdminWithoutConfirmation(IIdentityUnitOfWork uow, RegisterViewModel model, string returnUrl)
        {
            var admin = uow.RoleRepository.FindByLoweredName("ADMINISTRATOR") ?? uow.RoleRepository.Add(
                            new IdentityRoleEntity
                            {
                                ApplicationId = 0,
                                Description = "Administrator-Role",
                                Identifier = Guid.NewGuid(),
                                NormalizedName = "ADMINISTRATOR",
                                Name = "Administrator"
                            });
            uow.Commit();

            var user = new IdentityUserEntity { Name = model.Name, Email = model.Email, Phone = model.Phone };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, admin.Name);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, code);
            }

            return user;
        }

        /// <summary>Sends a confirmation mail.</summary>
        /// <param name="user"> The user. </param>
        private async Task SendConfirmationMail(IdentityUserEntity user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new {userId = user.Identifier, code},
                HttpContext.Request.Scheme);

            if (_adminOptions.ConfirmationRecipient == MailAddressConfirmationRecipient.User)
                await _emailSender.SendEmailAsync(user.Email,
                    new UserConfirmMailModel(_localizerFactory, _applicationOptions, user.Email, callbackUrl));
            else
                await _emailSender.SendEmailAsync(_adminOptions.AdminConfirmationRecipient,
                    new AdminConfirmMailModel(_localizerFactory, _applicationOptions, callbackUrl, _adminOptions.AdminConfirmationRecipient, Url.Action(nameof(AdminController.ManageUser), "Admin", new { userId = user.Id}, HttpContext.Request.Scheme)));
        }

        /// <summary>   Adds the errors. </summary>
        /// <param name="result">   The result. </param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        /// <summary>   Redirect to local. </summary>
        /// <param name="returnUrl">    URL of the return. </param>
        /// <returns>   An IActionResult. </returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion
    }
}