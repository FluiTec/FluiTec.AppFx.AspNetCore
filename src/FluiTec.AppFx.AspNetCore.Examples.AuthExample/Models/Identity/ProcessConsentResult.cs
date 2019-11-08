namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Identity
{
    /// <summary>	Encapsulates the result of the process consent. </summary>
    public class ProcessConsentResult
    {
        /// <summary>	. </summary>
        public bool IsRedirect => RedirectUri != null;

        /// <summary>	Gets or sets URI of the redirect. </summary>
        /// <value>	The redirect URI. </value>
        public string RedirectUri { get; set; }

        /// <summary>	. </summary>
        public bool ShowView => ViewModel != null;

        /// <summary>	Gets or sets the view model. </summary>
        /// <value>	The view model. </value>
        public ConsentModel ViewModel { get; set; }

        /// <summary>	. </summary>
        public bool HasValidationError => ValidationError != null;

        /// <summary>	Gets or sets the validation error. </summary>
        /// <value>	The validation error. </value>
        public string ValidationError { get; set; }
    }
}