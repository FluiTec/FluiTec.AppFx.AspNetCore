using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Localization
{
    /// <summary>   A resource group. </summary>
    [LocalizedModel]
    public class ResourceGroupModel
    {
        /// <summary>   Gets or sets the name. </summary>
        /// <value> The name. </value>
        public string Name { get; set; }

        /// <summary>   Gets or sets the entries. </summary>
        /// <value> The entries. </value>
        public int Entries { get; set; }
    }
}