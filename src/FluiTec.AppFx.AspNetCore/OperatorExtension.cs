using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>An operator extension.</summary>
    public static class OperatorExtension
    {
        /// <summary>An IServiceCollection extension method that configure authentication.</summary>
        /// <param name="services">         The services to act on. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection ConfigureOperator(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.AddSingleton(configuration.Configure<OperatorOptions>(services));
            return services;
        }
    }
}