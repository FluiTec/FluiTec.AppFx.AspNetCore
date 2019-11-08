using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>   A data Model for the delete claim. </summary>
    [LocalizedModel]
    public class ResourceClaimDeleteModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin.ResourceClaimDeleteModel";

        /// <summary>   Gets or sets the identifier of the resource. </summary>
        /// <value> The identifier of the resource. </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public int ResourceId { get; set; }

        /// <summary>   Gets or sets the type. </summary>
        /// <value> The type. </value>
        [Display(Name = FullModelName + "Type", Description = "Type")]
        [DisplayTranslationForCulture("Type", "Typ", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Type { get; set; }
    }
}
