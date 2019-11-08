using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.MailViews
{
    /// <summary>   A recover password resource. </summary>
    [LocalizedResource]
    public class RecoverPasswordResource
    {
        /// <summary>   Gets the subject. </summary>
        /// <value> The subject. </value>
        [TranslationForCulture("Passwort zurücksetzen", "de")]
        public static string Subject => "Recover password";

        /// <summary>   Gets the header. </summary>
        /// <value> The header. </value>
        [TranslationForCulture("Account-Management", "de")]
        public static string Header => "Account-Management";
    }
}