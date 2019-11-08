using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>   A data Model for the delete claim. </summary>
    [LocalizedModel]
    public class ClientClaimDeleteModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin.ClientClaimDeleteModel";

        /// <summary>   Gets or sets the identifier of the client. </summary>
        /// <value> The identifier of the client. </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public int ClientId { get; set; }

        /// <summary>   Gets or sets the type. </summary>
        /// <value> The type. </value>
        [Display(Name = FullModelName + "Type", Description = "Type")]
        [DisplayTranslationForCulture("Type", "Typ", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Type { get; set; }
    }
}
