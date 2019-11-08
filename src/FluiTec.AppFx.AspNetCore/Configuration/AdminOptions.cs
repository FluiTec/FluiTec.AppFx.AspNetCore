using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>	Admin options. </summary>
    [ConfigurationName("Admin")]
    public class AdminOptions
    {
        /// <summary>   Gets or sets the confirmation recipient. </summary>
        /// <value> The confirmation recipient. </value>
        public MailAddressConfirmationRecipient ConfirmationRecipient { get; set; }

        /// <summary>	Gets or sets the confirmation recipient. </summary>
        /// <value>	The confirmation recipient. </value>
        public string AdminConfirmationRecipient { get; set; }
    }
}