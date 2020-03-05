using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>An application extension.</summary>
    public static class ApplicationExtension
    {
        /// <summary>An IServiceCollection extension method that configure application.</summary>
        /// <param name="services">         The services to act on. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection ConfigureApplication(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.AddSingleton(configuration.Configure<ApplicationOptions>(services));
            return services;
        }
    }
}