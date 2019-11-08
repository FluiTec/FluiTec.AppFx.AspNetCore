using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>   A data Model for the identity resource add. </summary>
    [LocalizedModel]
    public class IdentityResourceAddModel : ResourceAddModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin.IdentityResourceAddModel";

        /// <summary>   Gets or sets a value indicating whether the required. </summary>
        /// <value> True if required, false if not. </value>
        [Display(Name = FullModelName + "Required", Description = "Required")]
        [DisplayTranslationForCulture("Required", "Benötigt", "de")]
        public bool Required { get; set; }

        /// <summary>   Gets or sets a value indicating whether the emphasize. </summary>
        /// <value> True if emphasize, false if not. </value>
        [Display(Name = FullModelName + "Emphasize", Description = "Emphasize")]
        [DisplayTranslationForCulture("Emphasize", "Hervorheben", "de")]
        public bool Emphasize { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the in discovery document is shown.
        /// </summary>
        /// <value> True if show in discovery document, false if not. </value>
        [Display(Name = FullModelName + "ShowInDiscoveryDocument", Description = "Show in discovery document")]
        [DisplayTranslationForCulture("ShowInDiscoveryDocument", "Bei Entdeckungsdokument anzeigen", "de")]
        public bool ShowInDiscoveryDocument { get; set; }
    }
}