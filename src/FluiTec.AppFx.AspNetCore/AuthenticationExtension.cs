using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>   An authentication extension. </summary>
    public static class AuthenticationExtension
    {
        /// <summary>   An IServiceCollection extension method that configure authentication. </summary>
        /// <param name="services">         The services to act on. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>   An IServiceCollection. </returns>
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            var options = configuration.GetConfiguration<AuthenticationOptions>();
            services.AddSingleton(options);

            var appOptions = configuration.GetConfiguration<ApplicationOptions>();

            var auth = services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
            auth.AddCookie(o => o.LoginPath = new PathString(appOptions.ApplicationLoginPath));

            if (options.Google != null && options.Google.Enabled)
                auth.AddGoogle(o =>
                {
                    o.ClientId = options.Google.ClientId;
                    o.ClientSecret = options.Google.ClientSecret;
                });

            if (options.Amazon != null && options.Amazon.Enabled)
                auth.AddAmazon(o =>
                {
                    o.ClientId = options.Amazon.ClientId;
                    o.ClientSecret = options.Amazon.ClientSecret;
                });

            return services;
        }
    }
}