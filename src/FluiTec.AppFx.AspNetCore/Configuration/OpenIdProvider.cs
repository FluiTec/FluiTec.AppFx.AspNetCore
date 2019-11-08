namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>   An open identifier provider. </summary>
    public class OpenIdProvider
    {
        /// <summary>   Default constructor. </summary>
        public OpenIdProvider()
        {
            Enabled = false;
        }

        /// <summary>   Gets or sets a value indicating whether this object is enabled. </summary>
        /// <value> True if enabled, false if not. </value>
        public bool Enabled { get; set; }

        /// <summary>   Gets or sets the identifier of the client. </summary>
        /// <value> The identifier of the client. </value>
        public string ClientId { get; set; }

        /// <summary>   Gets or sets the client secret. </summary>
        /// <value> The client secret. </value>
        public string ClientSecret { get; set; }
    }
}