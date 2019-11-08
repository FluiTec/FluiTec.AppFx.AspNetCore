using System.ComponentModel.DataAnnotations;
using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>   A data Model for the add claim. </summary>
    [LocalizedModel]
    public class ResourceClaimAddModel : UpdateModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin.ResourceClaimAddModel";

        /// <summary>   Gets or sets the identifier of the resource. </summary>
        /// <value> The identifier of the resource. </value>
        public int ResourceId { get; set; }

        /// <summary>   Gets or sets the type. </summary>
        /// <value> The type. </value>
        [Display(Name = FullModelName + "Type", Description = "Type")]
        [DisplayTranslationForCulture("Type", "Typ", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Type { get; set; }
    }
}
