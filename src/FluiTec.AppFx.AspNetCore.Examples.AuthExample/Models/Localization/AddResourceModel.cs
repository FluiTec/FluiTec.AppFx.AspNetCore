using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Localization
{
    /// <summary>   A data Model for the resource add. </summary>
    [LocalizedModel]
    public class ResourceAddModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Localization.ResourceAddModel";

        /// <summary>   Gets or sets the name. </summary>
        /// <value> The name. </value>
        [Display(Name = FullModelName + "Name", Description = "Name")]
        [DisplayTranslationForCulture("Name", "Name", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Name { get; set; }

        /// <summary>   Gets or sets the author. </summary>
        /// <value> The author. </value>
        [Display(Name = FullModelName + "Author", Description = "Author")]
        [DisplayTranslationForCulture("Author", "Autor", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Author { get; set; }
    }
}