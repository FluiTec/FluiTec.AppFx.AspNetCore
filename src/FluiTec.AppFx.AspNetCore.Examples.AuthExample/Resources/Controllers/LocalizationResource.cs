using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Resources.Controllers
{
    /// <summary>   A localization resource. </summary>
    [LocalizedResource]
    public class LocalizationResource
    {
        /// <summary>   Gets the ungrouped. </summary>
        /// <value> The ungrouped. </value>
        [TranslationForCulture("Nicht gruppiert", "de")]
        public string Ungrouped => "ungrouped";
    }
}
