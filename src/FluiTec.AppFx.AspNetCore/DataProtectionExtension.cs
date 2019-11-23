using FluiTec.AppFx.DataProtection;
using FluiTec.AppFx.DataProtection.Dynamic;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>   A data protection extension. </summary>
    public static class DataProtectionExtension
    {
        /// <summary>   An IServiceCollection extension method that configure data protection. </summary>
        /// <param name="services">         The services to act on. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>   An IServiceCollection. </returns>
        public static IServiceCollection ConfigureDataProtection(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.ConfigureDataProtectionDataService(configuration);

            services.AddScoped<IXmlRepository, DataProtectionKeyRepository>();
            var serviceProvider = services.BuildServiceProvider();
            services.AddDataProtection().AddKeyManagementOptions(options => options.XmlRepository = serviceProvider.GetService<IXmlRepository>());

            return services;
        }
    }
}
