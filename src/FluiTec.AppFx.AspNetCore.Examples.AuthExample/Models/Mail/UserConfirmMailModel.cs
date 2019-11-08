// ReSharper disable VirtualMemberCallInConstructor

using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.MailViews;
using FluiTec.AppFx.Localization;
using Microsoft.Extensions.Localization;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Mail
{
    /// <summary>   A data Model for the user confirm mail. </summary>
    public class UserConfirmMailModel : AuthMailModel
    {
        /// <summary>   Constructor. </summary>
        /// <param name="localizerFactory"> The localizer factory. </param>
        /// <param name="appOptions">       Options for controlling the application. </param>
        /// <param name="email">            The email. </param>
        /// <param name="validationUrl">    URL of the validation. </param>
        public UserConfirmMailModel(IStringLocalizerFactory localizerFactory, ApplicationOptions appOptions, string email, string validationUrl) : base(localizerFactory, appOptions)
        {
            var localizer = localizerFactory.Create(typeof(UserConfirmMailResource));
            ValidationUrl = validationUrl;
            Email = email;

            Subject = localizer.GetString(() => UserConfirmMailResource.Subject);
            Header = localizer.GetString(() => UserConfirmMailResource.Header);
        }

        /// <summary>	Gets or sets URL of the validation. </summary>
        /// <value>	The validation URL. </value>
        public string ValidationUrl { get; set; }

        /// <summary>	Gets or sets the email. </summary>
        /// <value>	The email. </value>
        public string Email { get; set; }
    }
}