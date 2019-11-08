using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Models
{
    /// <summary>An application resource.</summary>
    /// <remarks>
    ///     In order to localize this - set 'FluiTec.AppFx.AspNetCore.Models.[PropertyName]'
    /// </remarks>
    [LocalizedResource]
    public class ApplicationResource
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public static string Name { get; set; }
    }
}