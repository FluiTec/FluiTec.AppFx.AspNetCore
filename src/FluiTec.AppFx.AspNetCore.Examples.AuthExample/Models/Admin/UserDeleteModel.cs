using System.ComponentModel.DataAnnotations;
using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin
{
    /// <summary>   A data Model for the user delete. </summary>
    [LocalizedModel]
    public class UserDeleteModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin.UserDeleteModel";

        /// <summary>   Gets or sets the email. </summary>
        /// <value> The email. </value>
        [Display(Name = FullModelName + "Email", Description = "Email")]
        [DisplayTranslationForCulture("Email", "Email", "de")]
        public string Email { get; set; }
    }
}