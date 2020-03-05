using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>A captcha extension.</summary>
    public static class CaptchaExtension
    {
        /// <summary>An IServiceCollection extension method that configure captcha.</summary>
        /// <param name="services">         The services to act on. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection ConfigureCaptcha(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            // parse config
            var options = configuration.Configure<CaptchaOptions>(services);
            services.AddSingleton(options);

            // add recaptcha
            services.AddRecaptcha(new RecaptchaOptions
            {
                SiteKey = options.Key,
                SecretKey = options.Secret
            });

            return services;
        }
    }
}