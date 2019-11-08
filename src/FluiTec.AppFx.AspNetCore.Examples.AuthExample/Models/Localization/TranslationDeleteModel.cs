using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Localization
{
    /// <summary>   A data Model for the translation delete. </summary>
    [LocalizedModel]
    public class TranslationDeleteModel
    {
        /// <value> The identifier. </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public int Id { get; set; }
    }
}