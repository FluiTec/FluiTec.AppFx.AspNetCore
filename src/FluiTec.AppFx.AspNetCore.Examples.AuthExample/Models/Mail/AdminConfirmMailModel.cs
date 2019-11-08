using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.MailViews;
using FluiTec.AppFx.Localization;
using Microsoft.Extensions.Localization;

// ReSharper disable VirtualMemberCallInConstructor

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Mail
{
    /// <summary>   A data Model for the admin confirm mail. </summary>
    public class AdminConfirmMailModel : AuthMailModel
    {
        /// <summary>Constructor.</summary>
        /// <param name="localizerFactory"> The localizer factory. </param>
        /// <param name="appOptions">       Options for controlling the application. </param>
        /// <param name="validationUrl">    URL of the validation. </param>
        /// <param name="email">            The email. </param>
        /// <param name="userEditUrl">      The user edit URL. </param>
        public AdminConfirmMailModel(IStringLocalizerFactory localizerFactory, ApplicationOptions appOptions, string validationUrl, string email, string userEditUrl) : base(localizerFactory, appOptions)
        {
            var localizer = localizerFactory.Create(typeof(UserConfirmMailResource));
            ValidationUrl = validationUrl;
            UserEditUrl = userEditUrl;
            Email = email;

            Subject = localizer.GetString(() => AdminConfirmMailResource.Subject);
            Header = localizer.GetString(() => AdminConfirmMailResource.Header);
        }

        /// <summary>	Gets or sets URL of the validation. </summary>
        /// <value>	The validation URL. </value>
        public string ValidationUrl { get; set; }

        /// <summary>Gets or sets URL of the user edit.</summary>
        /// <value>The user edit URL.</value>
        public string UserEditUrl { get; set; }

        /// <summary>	Gets or sets the email. </summary>
        /// <value>	The email. </value>
        public string Email { get; set; }
    }
}