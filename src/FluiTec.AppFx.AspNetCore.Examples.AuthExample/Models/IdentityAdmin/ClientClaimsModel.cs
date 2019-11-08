using System.Collections.Generic;
using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.IdentityAdmin
{
    /// <summary>   A data Model for the user claims. </summary>
    [LocalizedModel]
    public class ClientClaimsModel
    {
        /// <summary>   Gets or sets the identifier of the client. </summary>
        /// <value> The identifier of the client. </value>
        public int ClientId { get; set; }

        /// <summary>   Gets or sets the claim entries. </summary>
        /// <value> The claim entries. </value>
        public List<ClaimEntry> ClaimEntries { get; set; }

        /// <summary>   A claim entry. </summary>
        public class ClaimEntry
        {
            /// <summary>   Gets or sets the type. </summary>
            /// <value> The type. </value>
            public string Type { get; set; }

            /// <summary>   Gets or sets the value. </summary>
            /// <value> The value. </value>
            public string Value { get; set; }
        }
    }
}