using FluiTec.DbLocalizationProvider.Abstractions;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Admin
{
    /// <summary>A data Model for the role.</summary>
    [LocalizedModel]
    public class UserRoleModel
    {
        /// <summary>Gets or sets the identifier of the user.</summary>
        /// <value>The identifier of the user.</value>
        public int UserId { get; set; }

        /// <summary>   Gets or sets the name. </summary>
        /// <value> The name. </value>
        public string Name { get; set; }
    }
}