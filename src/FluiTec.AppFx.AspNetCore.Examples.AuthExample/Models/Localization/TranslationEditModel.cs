using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Localization
{
    /// <summary>   A translation. </summary>
    [LocalizedModel]
    public class TranslationEditModel : UpdateModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Localization.Translation";

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public int Id { get; set; }

        /// <summary>   Gets or sets the value. </summary>
        /// <value> The value. </value>
        [Display(Name = FullModelName + "Value", Description = "Translation")]
        [DisplayTranslationForCulture("Value", "Übersetzung", "de")]
        public string Value { get; set; }
    }
}