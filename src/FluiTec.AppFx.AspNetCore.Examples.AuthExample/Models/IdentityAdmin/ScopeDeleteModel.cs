using System.ComponentModel.DataAnnotations;
using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>   A data Model for the scope delete. </summary>
    [LocalizedModel]
    public class ScopeDeleteModel
    {
        /// <summary>Gets or sets the identifier of the client.</summary>
        /// <value>The identifier of the client.</value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public int Id { get; set; }
    }
}