using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Identity
{
    /// <summary>   A consent options. </summary>
    [LocalizedModel]
    public class ConsentOptions
    {
        /// <summary>   True to enable, false to disable the offline access. </summary>
        public static bool EnableOfflineAccess = true;

        /// <summary>   Gets the offline access header. </summary>
        /// <value> The offline access header. </value>
        [TranslationForCulture("Offline Zugriff", "de")]
        public string OfflineAccessHeader => "Offline access";

        /// <summary>   Gets information describing the offline access. </summary>
        /// <value> Information describing the offline access. </value>
        [TranslationForCulture("Zugriff auf ihre Anwendungsdaten, sogar wenn Sie offline sind.", "de")]
        public string OfflineAccessDescription => "Access to your applications and resources, even when you are offline.";

        /// <summary>   Gets a message describing the must choose one error. </summary>
        /// <value> A message describing the must choose one error. </value>
        [TranslationForCulture("Sie müssen mindestens eine Erlaubnis auswählen!", "de")]
        public string MustChooseOneErrorMessage => "You must pick at leat one permission!";

        /// <summary>   Gets a message describing the invalid selection error. </summary>
        /// <value> A message describing the invalid selection error. </value>
        [TranslationForCulture("Ungültige Auswahl!", "de")]
        public string InvalidSelectionErrorMessage => "Invalid selection!";

        /// <summary>   Gets the allow access text. </summary>
        /// <value> The allow access text. </value>
        [TranslationForCulture("Erlauben", "de")]
        public string AllowAccessText => "Allow";

        /// <summary>   Gets the deny access text. </summary>
        /// <value> The deny access text. </value>
        [TranslationForCulture("Verweigern", "de")]
        public string DenyAccessText => "Deny";
    }
}