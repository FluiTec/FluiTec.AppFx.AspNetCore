using System;
using DbLocalizationProvider.Abstractions;
using FluiTec.AppFx.Localization;
using Microsoft.Extensions.Localization;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.Controllers
{
    /// <summary>A profile resource.</summary>
    [LocalizedResource]
    public class ProfileResource
    {
        /// <summary>The empty.</summary>
        private static readonly LocalizedString Empty = new LocalizedString(string.Empty, string.Empty);

        /// <summary>Gets or sets the change password success.</summary>
        /// <value>The change password success.</value>
        [TranslationForCulture("Ihr Passwort wurde geändert", "de")]
        public static string ChangePasswordSuccess => "Your password has been changed";

        /// <summary>Gets or sets the set password success.</summary>
        /// <value>The set password success.</value>
        [TranslationForCulture("Ihr Passwort wurde gesetzt", "de")]
        public static string SetPasswordSuccess => "You password has been set";

        /// <summary>Gets or sets the set two factor success.</summary>
        /// <value>The set two factor success.</value>
        [TranslationForCulture("Der 2-Faktor-Authentifizierungsprovider wurde gesetzt", "de")]
        public static string SetTwoFactorSuccess => "Your two-factor authentication provider has been set";

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        [TranslationForCulture("Es ist ein Fehler aufgetreten", "de")]
        public static string Error => "We've encountered an error";

        /// <summary>Gets or sets the add phone success.</summary>
        /// <value>The add phone success.</value>
        [TranslationForCulture("Ihre Telefonnummer wurde gesetzt", "de")]
        public static string AddPhoneSuccess => "Your phone number has been set";

        /// <summary>Gets or sets the remove phone success.</summary>
        /// <value>The remove phone success.</value>
        [TranslationForCulture("Ihre Telefonnummer wurde entfernt", "de")]
        public static string RemovePhoneSuccess => "Your phone number has been removed";

        /// <summary>Gets or sets the add login success.</summary>
        /// <value>The add login success.</value>
        [TranslationForCulture("Der Provider wurde hinzugefügt", "de")]
        public static string AddLoginSuccess => "The provider has been added";

        /// <summary>Gets or sets the remove login success.</summary>
        /// <value>The remove login success.</value>
        [TranslationForCulture("Der Provider wurde entfernt", "de")]
        public static string RemoveLoginSuccess => "The provider has been removed";

        /// <summary>Initializes this object from the given from manage message.</summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside
        ///     the required range.
        /// </exception>
        /// <param name="localizer">    The localizer. </param>
        /// <param name="message">      (Optional) The message. </param>
        /// <returns>A string.</returns>
        public static LocalizedString FromManageMessage(IStringLocalizer<ProfileResource> localizer,
            ManageMessageId? message = null)
        {
            switch (message)
            {
                case ManageMessageId.AddPhoneSuccess:
                    return localizer.GetString(() => AddPhoneSuccess);
                case ManageMessageId.AddLoginSuccess:
                    return localizer.GetString(() => AddLoginSuccess);
                case ManageMessageId.ChangePasswordSuccess:
                    return localizer.GetString(() => ChangePasswordSuccess);
                case ManageMessageId.SetTwoFactorSuccess:
                    return localizer.GetString(() => SetTwoFactorSuccess);
                case ManageMessageId.SetPasswordSuccess:
                    return localizer.GetString(() => SetPasswordSuccess);
                case ManageMessageId.RemoveLoginSuccess:
                    return localizer.GetString(() => RemoveLoginSuccess);
                case ManageMessageId.RemovePhoneSuccess:
                    return localizer.GetString(() => RemovePhoneSuccess);
                case ManageMessageId.Error:
                    return localizer.GetString(() => Error);
                case null:
                    return Empty;
                default:
                    return Empty;
            }
        }
    }
}