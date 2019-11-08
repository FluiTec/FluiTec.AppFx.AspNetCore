namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>A resource option.</summary>
    public class ResourceOption
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the name of the display.</summary>
        /// <value>The name of the display.</value>
        public string DisplayName { get; set; }

        /// <summary>Gets or sets the name of the group.</summary>
        /// <value>The name of the group.</value>
        public string GroupName { get; set; }

        /// <summary>Gets or sets the name of the group display.</summary>
        /// <value>The name of the group display.</value>
        public string GroupDisplayName { get; set; }
    }
}