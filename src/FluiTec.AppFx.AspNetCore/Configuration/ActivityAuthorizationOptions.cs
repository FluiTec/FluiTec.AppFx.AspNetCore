using System.Collections.Generic;
using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>An activity authorization options.</summary>
    [ConfigurationName("ActivityAuthorization")]
    public class ActivityAuthorizationOptions
    {
        /// <summary>Default constructor.</summary>
        public ActivityAuthorizationOptions()
        {
            Resources = new List<ResourceOption>();
        }

        /// <summary>Gets or sets the resources.</summary>
        /// <value>The resources.</value>
        public List<ResourceOption> Resources { get; set; }
    }
}
