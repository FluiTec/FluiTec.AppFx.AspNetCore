using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.MailViews
{
    /// <summary>   An admin confirm mail resource. </summary>
    [LocalizedResource]
    public class AdminConfirmMailResource
    {
        /// <summary>   Gets the subject. </summary>
        /// <value> The subject. </value>
        [TranslationForCulture("Bestätigen Sie diese E-Mail-Adresse", "de")]
        public static string Subject => "Confirm this mail-address";

        /// <summary>   Gets the header. </summary>
        /// <value> The header. </value>
        [TranslationForCulture("Account-Management", "de")]
        public static string Header => "Account-Management";
    }
}