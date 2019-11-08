using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.MailViews
{
    /// <summary>   A user confirm mail resource. </summary>
    [LocalizedResource]
    public class UserConfirmMailResource
    {
        /// <summary>   Gets the subject. </summary>
        /// <value> The subject. </value>
        [TranslationForCulture("Bestätigen Sie Ihre E-Mail-Adresse", "de")]
        public static string Subject => "Confirm your mail-address";

        /// <summary>   Gets the header. </summary>
        /// <value> The header. </value>
        [TranslationForCulture("Account-Management", "de")]
        public static string Header => "Account-Management";
    }
}