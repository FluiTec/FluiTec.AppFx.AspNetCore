using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin
{
    /// <summary>   A data Model for the delete claim. </summary>
    [LocalizedModel]
    public class ClaimDeleteModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin.ClaimDeleteModel";

        /// <summary>   Gets or sets the identifier of the user. </summary>
        /// <value> The identifier of the user. </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public int UserId { get; set; }

        /// <summary>   Gets or sets the type. </summary>
        /// <value> The type. </value>
        [Display(Name = FullModelName + "Type", Description = "Type")]
        [DisplayTranslationForCulture("Type", "Typ", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Type { get; set; }
    }
}
