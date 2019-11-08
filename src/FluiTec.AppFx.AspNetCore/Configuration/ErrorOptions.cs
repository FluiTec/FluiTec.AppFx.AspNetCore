using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>	An error options. </summary>
    [ConfigurationName("ErrorOptions")]
    public class ErrorOptions
    {
        private string _errorHandlingPath;

        /// <summary>	Default constructor. </summary>
        public ErrorOptions()
        {
            ErrorHandlingPath = "Home/Error";
        }

        /// <summary>	Gets or sets the error recipient. </summary>
        /// <value>	The error recipient. </value>
        public string ErrorRecipient { get; set; }

        /// <summary>	Gets or sets the full pathname of the error handling file. </summary>
        /// <value>	The full pathname of the error handling file. </value>
        /// <remarks>
        ///     Cant be null or whitespace.
        /// </remarks>
        public string ErrorHandlingPath
        {
            get => _errorHandlingPath;
            set
            {
                if (value != null && !string.IsNullOrWhiteSpace(value))
                    _errorHandlingPath = value;
            }
        }
    }
}