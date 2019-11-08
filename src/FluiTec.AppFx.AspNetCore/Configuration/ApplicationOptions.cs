using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>An application options.</summary>
    [ConfigurationName("ApplicationOptions")]
    public class ApplicationOptions
    {
        /// <summary>Gets or sets the application root display.</summary>
        /// <value>The application root display.</value>
        public string ApplicationRootDisplay { get; set; }

        /// <summary>Gets or sets the application root.</summary>
        /// <value>The application root.</value>
        public string ApplicationRoot { get; set; }

        /// <summary>   Gets or sets the application resource root. </summary>
        /// <value> The application resource root. </value>
        public string ApplicationResourceRoot { get; set; }

        /// <summary>Gets or sets the full pathname of the application login file.</summary>
        /// <value>The full pathname of the application login file.</value>
        public string ApplicationLoginPath { get; set; }

        /// <summary>Gets or sets the application login display.</summary>
        /// <value>The application login display.</value>
        public string ApplicationLoginDisplay { get; set; }

        /// <summary>Gets or sets the application login.</summary>
        /// <value>The application login.</value>
        public string ApplicationLogin { get; set; }
    }
}