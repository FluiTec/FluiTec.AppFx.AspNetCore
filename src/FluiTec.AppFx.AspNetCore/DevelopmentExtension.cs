using FluiTec.AppFx.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>	A development extension. </summary>
    /// <remarks>
    ///     This class simply autolaunches the browser if the
    ///     HostingEnvironment is set to Development.
    /// </remarks>
    public static class DevelopmentExtension
    {
        /// <summary>	An IApplicationBuilder extension method that uses hosting services. </summary>
        /// <param name="app">				The app to act on. </param>
        /// <param name="environment">  	The environment. </param>
        /// <returns>	An IApplicationBuilder. </returns>
        public static IApplicationBuilder UseDevelopmentExtension(this IApplicationBuilder app,
            IWebHostEnvironment environment)
        {
            return app.UseBrowserLinkExtension(environment);
        }
    }
}