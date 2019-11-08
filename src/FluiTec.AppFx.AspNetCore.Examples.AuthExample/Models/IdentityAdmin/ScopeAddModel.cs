using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>A data Model for the scope add.</summary>
    [LocalizedModel]
    public class ScopeAddModel
    {
        /// <summary>Name of the full model.</summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin.ScopeAddModel";

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        [Display(Name = FullModelName + "Name", Description = "Name")]
        [DisplayTranslationForCulture("Name", "Name", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Name { get; set; }

        /// <summary>Gets or sets the name of the display.</summary>
        /// <value>The name of the display.</value>
        [Display(Name = FullModelName + "DisplayName", Description = "Display name")]
        [DisplayTranslationForCulture("DisplayName", "Anzeigename", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string DisplayName { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        [Display(Name = FullModelName + "Description", Description = "Description")]
        [DisplayTranslationForCulture("Description", "Beschreibung", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public string Description { get; set; }

        /// <summary>Gets or sets a value indicating whether the required.</summary>
        /// <value>True if required, false if not.</value>
        [Display(Name = FullModelName + "Required", Description = "Required")]
        [DisplayTranslationForCulture("Required", "Erforderlich", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public bool Required { get; set; }

        /// <summary>Gets or sets a value indicating whether the emphasize.</summary>
        /// <value>True if emphasize, false if not.</value>
        [Display(Name = FullModelName + "Emphasize", Description = "Emphasize")]
        [DisplayTranslationForCulture("Emphasize", "Hervorheben", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public bool Emphasize { get; set; }

        /// <summary>Gets or sets a value indicating whether the in discovery document is shown.</summary>
        /// <value>True if show in discovery document, false if not.</value>
        [Display(Name = FullModelName + "ShowInDiscoveryDocument", Description = "Show in discovery")]
        [DisplayTranslationForCulture("ShowInDiscoveryDocument", "Im Entdeckungsdokument zeigen", "de")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequiredMessage")]
        public bool ShowInDiscoveryDocument { get; set; }
    }
}