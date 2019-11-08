using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>	Static file options. </summary>
    [ConfigurationName("StaticFiles")]
    public class StaticFileOptions
    {
        /// <summary>	Default constructor. </summary>
        public StaticFileOptions()
        {
            EnableClientCaching = true;
        }

        /// <summary>	Gets or sets a value indicating whether the client caching is enabled. </summary>
        /// <value>	True if enable client caching, false if not. </value>
        public bool EnableClientCaching { get; set; }

        /// <summary>	Gets or sets the duration of the cache. </summary>
        /// <value>	The cache duration. </value>
        public int CacheDuration { get; set; }
    }
}