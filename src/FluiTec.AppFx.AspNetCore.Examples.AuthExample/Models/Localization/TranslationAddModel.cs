using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Localization
{
    [LocalizedModel]
    public class TranslationAddModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Localization.TranslationAddModel";

        /// <summary>   Gets or sets the identifier. </summary>
        /// <value> The identifier. </value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public int ResourceId { get; set; }

        [Display(Name = FullModelName + "Language", Description = "Language")]
        [DisplayTranslationForCulture("Language", "Sprache", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Language { get; set; }

        /// <summary>   Gets or sets the value. </summary>
        /// <value> The value. </value>
        [Display(Name = FullModelName + "Value", Description = "Translation")]
        [DisplayTranslationForCulture("Value", "Übersetzung", "de")]
        public string Value { get; set; }
    }
}