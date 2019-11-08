using System.ComponentModel.DataAnnotations;
using DbLocalizationProvider.Abstractions;
using FluiTec.AppFx.Identity.Models.AccountViewModels;

namespace FluiTec.AppFx.AspNetCore.Examples.LocalizationExample.Models
{
    [LocalizedResource]
    public class AnnotationModel : ConfirmEmailAgainViewModel
    {
        /// <summary>   Name of the full model. </summary>
        private const string FullModelName =
            "FluiTec.AppFx.AspNetCore.Examples.LocalizationExample.Models.AnnotationModel";

        /// <summary>   Gets or sets the name of the user. </summary>
        /// <value> The name of the user. </value>
        [Display(Name = FullModelName + "UserName",
            Description = "Username:")] // translation for cultures: invariant, en
        [DisplayTranslationForCulture("UserName", "Benutzername:", "de")]
        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Username must not be empty.")] // translation for cultures: invariant, en
        [ValidationTranslationForCulture("Required", "Benutzername darf nicht leer sein.",
            "de")] // translation of Required for culture: de-DE
        public string UserName { get; set; }
    }
}