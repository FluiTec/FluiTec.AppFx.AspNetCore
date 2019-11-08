using System.Collections.Generic;
using FluiTec.DbLocalizationProvider;
using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>   A database localization resource options. </summary>
    [ConfigurationName("LocalizationResources")]
    public class DbLocalizationResourceOptions
    {
        /// <summary>   Default constructor. </summary>
        public DbLocalizationResourceOptions()
        {
            Resources = new List<LocalizationResource>();
        }

        /// <summary>   Gets or sets the resources. </summary>
        /// <value> The resources. </value>
        public List<LocalizationResource> Resources { get; set; }
    }
}