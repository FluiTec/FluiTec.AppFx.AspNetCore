using System.Collections.Generic;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Identity
{ 
    /// <summary>   A data Model for the consent. </summary>
    public class ConsentModel : ConsentInputModel
    {
        /// <summary>	Gets or sets the name of the client. </summary>
        /// <value>	The name of the client. </value>
        public string ClientName { get; set; }

        /// <summary>	Gets or sets URL of the client. </summary>
        /// <value>	The client URL. </value>
        public string ClientUrl { get; set; }

        /// <summary>	Gets or sets URL of the client logo. </summary>
        /// <value>	The client logo URL. </value>
        public string ClientLogoUrl { get; set; }

        /// <summary>	Gets or sets a value indicating whether we allow remember consent. </summary>
        /// <value>	True if allow remember consent, false if not. </value>
        public bool AllowRememberConsent { get; set; }

        /// <summary>	Gets or sets the identity scopes. </summary>
        /// <value>	The identity scopes. </value>
        public IEnumerable<ScopeModel> IdentityScopes { get; set; }

        /// <summary>	Gets or sets the resource scopes. </summary>
        /// <value>	The resource scopes. </value>
        public IEnumerable<ScopeModel> ResourceScopes { get; set; }
    }
}