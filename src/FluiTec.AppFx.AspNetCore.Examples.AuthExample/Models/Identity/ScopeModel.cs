namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models.Identity
{
    /// <summary>   A data Model for the scope. </summary>
    public class ScopeModel
    {
        /// <summary>	Gets or sets the name. </summary>
        /// <value>	The name. </value>
        public string Name { get; set; }

        /// <summary>	Gets or sets the name of the display. </summary>
        /// <value>	The name of the display. </value>
        public string DisplayName { get; set; }

        /// <summary>	Gets or sets the description. </summary>
        /// <value>	The description. </value>
        public string Description { get; set; }

        /// <summary>	Gets or sets a value indicating whether the emphasize. </summary>
        /// <value>	True if emphasize, false if not. </value>
        public bool Emphasize { get; set; }

        /// <summary>	Gets or sets a value indicating whether the required. </summary>
        /// <value>	True if required, false if not. </value>
        public bool Required { get; set; }

        /// <summary>	Gets or sets a value indicating whether the checked. </summary>
        /// <value>	True if checked, false if not. </value>
        public bool Checked { get; set; }
    }
}