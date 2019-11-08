using System.Collections.Generic;
using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>	The status code options. </summary>
    [ConfigurationName("StatusCode")]
    public class StatusCodeOptions
    {
        /// <summary>	Full pathname of the status code handler file. </summary>
        private string _statusCodeHandlerPath;

        /// <summary>	Gets or sets the self handled codes. </summary>
        /// <value>	The self handled codes. </value>
        public List<int> SelfHandledCodes { get; set; }

        /// <summary>	Gets or sets the full pathname of the status code handler file. </summary>
        /// <value>	The full pathname of the status code handler file. </value>
        /// <remarks>
        ///     This property can not be null or empty.
        /// </remarks>
        public string StatusCodeHandlerPath
        {
            get => _statusCodeHandlerPath;
            set
            {
                if (value != null && !string.IsNullOrWhiteSpace(value))
                    _statusCodeHandlerPath = value;
            }
        }
    }
}