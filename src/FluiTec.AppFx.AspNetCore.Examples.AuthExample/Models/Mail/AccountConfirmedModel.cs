using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.MailViews;
using FluiTec.AppFx.Localization;
using Microsoft.Extensions.Localization;

// ReSharper disable VirtualMemberCallInConstructor

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Mail
{
    /// <summary>   A data Model for the account confirmed. </summary>
    public class AccountConfirmedModel : AuthMailModel
    {
        /// <summary>   Constructor. </summary>
        /// <param name="localizerFactory"> The localizer factory. </param>
        /// <param name="appOptions">       Options for controlling the application. </param>
        /// <param name="loginLink">        The login link. </param>
        public AccountConfirmedModel(IStringLocalizerFactory localizerFactory, ApplicationOptions appOptions, string loginLink) : base(localizerFactory, appOptions)
        {
            var localizer = localizerFactory.Create(typeof(AccountConfirmedResource));

            Subject = localizer.GetString(() => AccountConfirmedResource.Subject);
            Header = localizer.GetString(() => AccountConfirmedResource.Header);
        }
    }
}