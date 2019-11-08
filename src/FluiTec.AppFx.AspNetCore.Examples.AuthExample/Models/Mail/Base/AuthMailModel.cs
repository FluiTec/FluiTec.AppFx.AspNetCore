using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.AspNetCore.Models;
using FluiTec.AppFx.Mail;
using Microsoft.Extensions.Localization;
using FluiTec.AppFx.Localization;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Mail
{
    /// <summary>   A data Model for the authentication mail. </summary>
    public class AuthMailModel : MailModel
    {
        /// <summary>   Constructor. </summary>
        /// <param name="localizerFactory"> The localizer factory. </param>
        /// <param name="appOptions">       Options for controlling the application. </param>
        public AuthMailModel(IStringLocalizerFactory localizerFactory, ApplicationOptions appOptions)
        {
            var localizer = localizerFactory.Create(typeof(ApplicationResource));

            ApplicationName = localizer.GetString(() => ApplicationResource.Name);
            ApplicationUrl = appOptions?.ApplicationRoot;
            ApplicationUrlDisplay = appOptions?.ApplicationRootDisplay;
        }
    }
}