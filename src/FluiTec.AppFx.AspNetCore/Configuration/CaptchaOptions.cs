using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>A captcha options.</summary>
    [ConfigurationName("Captcha")]
    public class CaptchaOptions
    {
        /// <summary>Gets or sets the key.</summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>Gets or sets the secret.</summary>
        /// <value>The secret.</value>
        public string Secret { get; set; }
    }
}