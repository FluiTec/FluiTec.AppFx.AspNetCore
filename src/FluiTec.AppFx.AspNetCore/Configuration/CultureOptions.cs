using System.Collections.Generic;
using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>	A culture options. </summary>
    [ConfigurationName("Localization")]
    public class CultureOptions
    {
        private string _resourcesPath;

        /// <summary>	Default constructor. </summary>
        public CultureOptions()
        {
            SupportedCultures = new List<string>();
            ResourcesPath = "Resources";
        }

        /// <summary>	Gets or sets the default culture. </summary>
        /// <value>	The default culture. </value>
        public string DefaultCulture { get; set; }

        /// <summary>	Gets or sets the supported cultures. </summary>
        /// <value>	The supported cultures. </value>
        public List<string> SupportedCultures { get; set; }

        /// <summary>	Gets or sets the full pathname of the resources file. </summary>
        /// <value>	The full pathname of the resources file. </value>
        /// <remarks>
        ///     Can not be null
        /// </remarks>
        public string ResourcesPath
        {
            get => _resourcesPath;
            set
            {
                if (value != null && !string.IsNullOrWhiteSpace(value))
                    _resourcesPath = value;
            }
        }
    }
}