using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.MailViews
{
    /// <summary>   An account confirmed resource. </summary>
    [LocalizedResource]
    public class AccountConfirmedResource
    {
        /// <summary>Gets or sets the subject.</summary>
        /// <value>The subject.</value>
        [TranslationForCulture("Account bestätigt", "de")]
        public static string Subject => "Account confirmed";

        /// <summary>Gets or sets the header.</summary>
        /// <value>The header.</value>
        [TranslationForCulture("Account bestätigt", "de")]
        public static string Header => "Account confirmed";
    }
}