using System;
using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.MailViews;
using FluiTec.AppFx.Localization;
using Microsoft.Extensions.Localization;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Mail
{
    /// <summary>   A data Model for the error. This class cannot be inherited. </summary>
    public sealed class ErrorMailModel : AuthMailModel
    {
        /// <summary>   Constructor. </summary>
        /// <param name="localizerFactory"> The localizer factory. </param>
        /// <param name="appOptions">       Options for controlling the application. </param>
        /// <param name="errorRoute">       The error route. </param>
        /// <param name="exception">        The exception. </param>
        public ErrorMailModel(IStringLocalizerFactory localizerFactory, ApplicationOptions appOptions, string errorRoute, Exception exception) : base(localizerFactory, appOptions)
        {
            var localizer = localizerFactory.Create(typeof(ErrorModelResource));

            Subject = localizer.GetString(() => ErrorModelResource.Subject);
            Header = localizer.GetString(() => ErrorModelResource.Header);

            ErrorRoute = errorRoute;
            ExceptionText = exception?.ToString();
        }

        /// <summary>	Gets or sets the exception text. </summary>
        /// <value>	The exception text. </value>
        public string ExceptionText { get; set; }

        /// <summary>	Gets or sets the error route. </summary>
        /// <value>	The error route. </value>
        public string ErrorRoute { get; set; }
    }
}