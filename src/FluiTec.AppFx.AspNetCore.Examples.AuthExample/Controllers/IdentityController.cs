using System.Threading.Tasks;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Identity;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Services;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Controllers
{
    /// <summary>   A controller for handling identities. </summary>
    public class IdentityController : Controller
    {
        /// <summary>   The consent service. </summary>
        private readonly ConsentService _consentService;

        /// <summary>   Constructor. </summary>
        /// <param name="interactionService">   The interaction service. </param>
        /// <param name="clientStore">          The client store. </param>
        /// <param name="resourceStore">        The resource store. </param>
        /// <param name="grantStore">           The grant store. </param>
        /// <param name="localizer">            The localizer. </param>
        public IdentityController(IIdentityServerInteractionService interactionService, IClientStore clientStore,
            IResourceStore resourceStore, IPersistedGrantStore grantStore, IStringLocalizer<ConsentOptions> localizer)
        {
            _consentService = new ConsentService(interactionService, clientStore, resourceStore, grantStore, localizer);
        }

        /// <summary>	Consents. </summary>
        /// <param name="returnUrl">	URL of the return. </param>
        /// <returns>	An IActionResult. </returns>
        public async Task<IActionResult> Consent(string returnUrl)
        {
            // user consented already
            if (await _consentService.DidUserConsentAlreadyAsync(returnUrl, HttpContext)) return Redirect(returnUrl);

            // user did not consent yet
            var vm = await _consentService.BuildViewModelAsync(returnUrl);
            return vm != null ? View(vm) : View("Error");
        }

        /// <summary>	Consents the given model. </summary>
        /// <param name="urn">	The model. </param>
        /// <returns>	A Task&lt;IActionResult&gt; </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Consent([FromForm] ConsentInputModel urn)
        {
            var result = await _consentService.ProcessConsent(urn);

            if (result.IsRedirect)
                return Redirect(result.RedirectUri);

            if (result.HasValidationError)
                ModelState.AddModelError(string.Empty, result.ValidationError);

            return result.ShowView ? View(result.ViewModel) : View("Error");
        }
    }
}