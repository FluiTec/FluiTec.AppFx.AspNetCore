using System.Collections.Generic;
using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin
{
    /// <summary>   A data Model for the user claims. </summary>
    [LocalizedModel]
    public class UserClaimsModel
    {
        /// <summary>   Gets or sets the identifier of the user. </summary>
        /// <value> The identifier of the user. </value>
        public int UserId { get; set; }

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