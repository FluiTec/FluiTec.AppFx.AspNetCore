using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.MailViews;
using FluiTec.AppFx.Localization;
using Microsoft.Extensions.Localization;

// ReSharper disable VirtualMemberCallInConstructor

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Mail
{
    /// <summary>   A data Model for the recover password mail. </summary>
    public class RecoverPasswordMailModel : AuthMailModel
    {
        /// <summary>   Constructor. </summary>
        /// <param name="localizerFactory"> The localizer factory. </param>
        /// <param name="appOptions">       Options for controlling the application. </param>
        /// <param name="validationUrl">    The validation URL. </param>
        public RecoverPasswordMailModel(IStringLocalizerFactory localizerFactory, ApplicationOptions appOptions, string validationUrl) : base(localizerFactory, appOptions)
        {
            var localizer = localizerFactory.Create(typeof(RecoverPasswordResource));
            ValidationUrl = validationUrl;

            Subject = localizer.GetString(() => RecoverPasswordResource.Subject);
            Header = localizer.GetString(() => RecoverPasswordResource.Header);
        }

        /// <summary>	Gets or sets URL of the validation. </summary>
        /// <value>	The validation URL. </value>
        public string ValidationUrl { get; set; }
    }
}