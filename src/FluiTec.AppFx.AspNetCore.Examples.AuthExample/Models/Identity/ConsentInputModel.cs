using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Identity
{
    /// <summary>	A data Model for the consent input. </summary>
    [LocalizedModel]
    public class ConsentInputModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName = "FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Identity.ConsentInputModel";

        /// <summary>	Gets or sets the button. </summary>
        /// <value>	The button. </value>
        public string Button { get; set; }

        /// <summary>	Gets or sets the scopes consented. </summary>
        /// <value>	The scopes consented. </value>
        public IEnumerable<string> ScopesConsented { get; set; }

        /// <summary>	Gets or sets a value indicating whether the remember consent. </summary>
        /// <value>	True if remember consent, false if not. </value>
        [Display(Name = FullModelName + "RememberConsent", Description = "Remember consent?")]
        [DisplayTranslationForCulture("RememberConsent", "An Zustimmung erinnern?", "de")]
        public bool RememberConsent { get; set; }

        /// <summary>	Gets or sets URL of the return. </summary>
        /// <value>	The return URL. </value>
        public string ReturnUrl { get; set; }
    }
}
