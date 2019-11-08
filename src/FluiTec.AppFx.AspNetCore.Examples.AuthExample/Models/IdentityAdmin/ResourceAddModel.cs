using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>   A data Model for the resource add. </summary>
    [LocalizedModel]
    public class ResourceAddModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin.ResourceAddModel";

        /// <summary>   Gets or sets the name. </summary>
        /// <value> The name. </value>
        [Display(Name = FullModelName + "Name", Description = "Name")]
        [DisplayTranslationForCulture("Name", "Name", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Name { get; set; }

        /// <summary>   Gets or sets the name of the display. </summary>
        /// <value> The name of the display. </value>
        [Display(Name = FullModelName + "DisplayName", Description = "Display name")]
        [DisplayTranslationForCulture("DisplayName", "Anzeigename", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string DisplayName { get; set; }

        /// <summary>   Gets or sets the description. </summary>
        /// <value> The description. </value>
        [Display(Name = FullModelName + "Description", Description = "Description")]
        [DisplayTranslationForCulture("Description", "Beschreibung", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Description { get; set; }

        /// <summary>   Gets or sets a value indicating whether this object is enabled. </summary>
        /// <value> True if enabled, false if not. </value>
        [Display(Name = FullModelName + "Enabled", Description = "Enabled")]
        [DisplayTranslationForCulture("Enabled", "Angeschaltet", "de")]
        public bool Enabled { get; set; }
    }
}