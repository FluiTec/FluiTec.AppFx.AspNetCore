using System;

namespace FluiTec.AppFx.AspNetCore.Error
{
    public class ErrorMessage
    {
        /// <summary>	Gets or sets the exception. </summary>
        /// <value>	The exception. </value>
        public Exception Exception { get; set; }

        /// <summary>	Gets or sets the route. </summary>
        /// <value>	The route. </value>
        public string Route { get; set; }
    }
}