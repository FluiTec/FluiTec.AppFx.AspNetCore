using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.Identity;
using FluiTec.AppFx.Identity.Entities;
using FluiTec.AppFx.Identity.Localization;
using FluiTec.AppFx.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>	An identity server extension. </summary>
    public static class IdentityExtension
    {
        /// <summary>	An IServiceCollection extension method that configure identity server. </summary>
        /// <param name="services">			The services to act on. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <returns>	An IServiceCollection. </returns>
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.ConfigureIdentityDataService(configuration);

            var adminOptions = configuration.GetConfiguration<AdminOptions>();
            services.AddSingleton(adminOptions);

            // configure aspnet-identity
            services.AddIdentity<IdentityUserEntity, IdentityRoleEntity>(config =>
                {
                    config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ öäüÖÄÜ ß 12345678";
                    config.User.RequireUniqueEmail = true;
                    config.SignIn.RequireConfirmedEmail = true;
                    config.Lockout.AllowedForNewUsers = true;
                    config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                })
                .AddErrorDescriber<MultiLanguageIdentityErrorDescriber>()
                .AddDefaultTokenProviders();
            services.AddIdentityStores();

            var serviceDescriptor = services.SingleOrDefault(descriptor => descriptor.ImplementationType == typeof(Microsoft.AspNetCore.Identity.UserValidator<IdentityUserEntity>));
            if (serviceDescriptor != null)
                services.Remove(serviceDescriptor);

            // configure cookies
            return services.ConfigureLoginCookieRedirect(configuration);
        }

        /// <summary>
        ///     An IServiceCollection extension method that configure login cookie redirect.
        /// </summary>
        /// <param name="services">         The services to act on. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>   An IServiceCollection. </returns>
        public static IServiceCollection ConfigureLoginCookieRedirect(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var apiOptions = configuration.GetConfiguration<ApiOptions>();
            services.AddSingleton(apiOptions);

            services.ConfigureApplicationCookie(cOptions =>
            {
                cOptions.Events = new CookieAuthenticationEvents
                {
                    // disable redirect to login for api-users
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.StartsWithSegments(apiOptions.ApiOnlyPath) &&
                            context.Response.StatusCode == (int)HttpStatusCode.OK)
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        else
                            context.Response.Redirect(context.RedirectUri);

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}