using System;
using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin
{
    /// <summary>   A data Model for the user lockout. </summary>
    [LocalizedModel]
    public class UserLockoutModel : UserDeleteModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin.UserLockoutModel";

        /// <summary>   Gets or sets the lockout time. </summary>
        /// <value> The lockout time. </value>
        [Display(Name = FullModelName + "LockoutTime", Description = "LockoutTime:")]
        [DisplayTranslationForCulture("LockoutTime", "Aussperrzeit:", "de")]
        [DataType(DataType.DateTime)]
        public DateTime? LockoutTime { get; set; }
    }
}