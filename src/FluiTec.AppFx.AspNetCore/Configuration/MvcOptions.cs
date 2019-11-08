using Microsoft.Extensions.DependencyInjection;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>	A MVC options. </summary>
    public class MvcOptions
    {
        /// <summary>	Constructor. </summary>
        /// <param name="builder">	The builder. </param>
        public MvcOptions(IMvcBuilder builder)
        {
            Builder = builder;
        }

        /// <summary>	Gets the builder. </summary>
        /// <value>	The builder. </value>
        public IMvcBuilder Builder { get; }
    }
}