using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>A default policy.</summary>
    public class DefaultPolicy
    {
        /// <summary>Constructor.</summary>
        /// <param name="name">         The name. </param>
        /// <param name="requirements"> The requirements. </param>
        public DefaultPolicy(string name, params IAuthorizationRequirement[] requirements)
        {
            Name = name;
            Requirements = requirements.ToList();
        }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the requirements.</summary>
        /// <value>The requirements.</value>
        public IEnumerable<IAuthorizationRequirement> Requirements { get; set; }
    }
}