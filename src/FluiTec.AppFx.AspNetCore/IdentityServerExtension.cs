using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.Identity.Entities;
using FluiTec.AppFx.IdentityServer;
using FluiTec.AppFx.IdentityServer.Configuration;
using FluiTec.AppFx.IdentityServer.Validators;
using FluiTec.AppFx.Options;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>   An identity server extension. </summary>
    public static class IdentityServerExtension
    {
        /// <summary>	Configure identity server. </summary>
        /// <param name="services">			The services. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <returns>	An IServiceCollection. </returns>
        public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.ConfigureIdentityServerDataService(configuration);

            var signingOptions = configuration.GetConfiguration<SigningOptions>();
            if (signingOptions != null)
                services.AddSingleton(signingOptions);

            var certificateOptions = configuration.GetConfiguration<CertificateOptions>();
            if (certificateOptions != null)
                services.AddSingleton(certificateOptions);

            services.AddSingleton(configuration.GetConfiguration<IdentityServerClaimOptions>());

            var idSrv = services.AddIdentityServer(options =>
                {
                    options.UserInteraction.ConsentUrl = "/Identity/Consent";
                });

            idSrv.Services.AddScoped<IRedirectUriValidator, LocalhostRedirectUriValidator>();
            idSrv.Services.AddScoped<IExtensionGrantValidator, DelegationGrantValidator>();
            idSrv.Services.AddScoped<ISigningCredentialStore, CertificateCredentialStore>();
            idSrv.Services.AddScoped<IValidationKeysStore, CertificateCredentialStore>();
            idSrv.Services.AddScoped<IPersistedGrantStore, GrantStore>();

            idSrv.AddAspNetIdentity<IdentityUserEntity>();
            idSrv.AddClientStore<ClientStore>();
            idSrv.AddResourceStore<ResourceStore>();
            idSrv.AddProfileService<ProfileService>();

            return services;
        }

        /// <summary>	An IApplicationBuilder extension method that use identity server. </summary>
        /// <param name="app">				The app to act on. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <returns>	An IApplicationBuilder. </returns>
        public static IApplicationBuilder UseIdentityServer(this IApplicationBuilder app, IConfigurationRoot configuration)
        {
            // enable identityserver
            app.UseIdentityServer();

            return app;
        }
    }
}