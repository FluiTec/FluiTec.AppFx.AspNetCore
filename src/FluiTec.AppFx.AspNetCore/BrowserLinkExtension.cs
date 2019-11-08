using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>   A browser link extension. </summary>
    public static class BrowserLinkExtension
    {
        /// <summary>	An IApplicationBuilder extension method that use hosting services. </summary>
        /// <param name="app">				The app to act on. </param>
        /// <param name="environment">  	The environment. </param>
        /// <returns>	An IApplicationBuilder. </returns>
        public static IApplicationBuilder UseBrowserLinkExtension(this IApplicationBuilder app,
            IHostingEnvironment environment)
        {
            if (environment.IsDevelopment()) app.UseBrowserLink();

            return app;
        }
    }
}