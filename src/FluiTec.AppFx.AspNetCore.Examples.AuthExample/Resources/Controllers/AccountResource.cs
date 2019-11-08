using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.Controllers
{
    /// <summary>   An account resource. </summary>
    [LocalizedResource]
    public class AccountResource
    {
        /// <summary>   Gets the email not confirmed. </summary>
        /// <value> The email not confirmed. </value>
        [TranslationForCulture("Ihre E-Mail-Adresse wurde noch nicht bestätigt.", "de")]
        public string EmailNotConfirmed => "Your mail was not yet verified.";

        /// <summary>   Gets the external provider error. </summary>
        /// <value> The external provider error. </value>
        [TranslationForCulture("Fehler des Anmeledienstes:", "de")]
        public string ExternalProviderError => "Error of the authentication-provider:";

        /// <summary>   Gets the invalid code. </summary>
        /// <value> The invalid code. </value>
        [TranslationForCulture("Ungültiger Code.", "de")]
        public string InvalidCode => "Invalid Code.";

        /// <summary>   Gets the invalid login attempt. </summary>
        /// <value> The invalid login attempt. </value>
        [TranslationForCulture("Ungültiger Anmeldeversuch.", "de")]
        public string InvalidLoginAttempt => "Invalig login attempt.";

        /// <summary>   Gets the security code is. </summary>
        /// <value> The security code is. </value>
        [TranslationForCulture("Dein Code ist:", "de")]
        public string SecurityCodeIs => "Your code is:";
    }
}