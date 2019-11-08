using System.Collections.Generic;
using DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>   A data Model for the resource claims. </summary>
    [LocalizedModel]
    public class ResourceClaimsModel
    {
        /// <summary>   Gets or sets the identifier of the resource. </summary>
        /// <value> The identifier of the resource. </value>
        public int ResourceId { get; set; }

        /// <summary>   Gets or sets the claim entries. </summary>
        /// <value> The claim entries. </value>
        public List<ClaimEntry> ClaimEntries { get; set; }

        /// <summary>   A claim entry. </summary>
        public class ClaimEntry
        {
            /// <summary>   Gets or sets the type. </summary>
            /// <value> The type. </value>
            public string Type { get; set; }
        }
    }
}