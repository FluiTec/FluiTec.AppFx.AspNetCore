using System;
using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.Authorization.Activity;
using FluiTec.AppFx.Authorization.Activity.AuthorizationHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluiTec.AppFx.Authorization.Activity.Dynamic;
using FluiTec.AppFx.Authorization.Activity.Entities;
using FluiTec.AppFx.Authorization.Activity.Requirements;
using FluiTec.AppFx.Identity.Entities;
using FluiTec.AppFx.IdentityServer.Entities;
using FluiTec.AppFx.Localization.Entities;
using FluiTec.AppFx.Options;
using Microsoft.AspNetCore.Authorization;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>An authorization extension.</summary>
    public static class AuthorizationExtension
    {
        /// <summary>   An IServiceCollection extension method that configure authorization. </summary>
        /// <param name="services">         The services to act on. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <param name="configureAction">  (Optional) The configure action. </param>
        /// <returns>   An IServiceCollection. </returns>
        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services, IConfigurationRoot configuration, Action<AuthorizationOptions> configureAction = null)
        {
            services.ConfigureAppFxAuthorizationData(configuration);
            services.AddScoped<IAuthorizationHandler, ResourceTypOperationAuthorizationHandler>();
            services.AddScoped<ResourceTypOperationAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, AdministrativeAccessAuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyNames.AdministrativeAccess, policy =>
                    policy.Requirements.Add(new AdministrativeAccessRequirement(new[]
                    {
                        ResourceActivities.AccessRequirement(typeof(IdentityUserEntity)),
                        ResourceActivities.AccessRequirement(typeof(IdentityRoleEntity)),
                        ResourceActivities.AccessRequirement(typeof(ClientEntity)),
                        ResourceActivities.AccessRequirement(typeof(ScopeEntity)),
                        ResourceActivities.AccessRequirement(typeof(ApiResourceEntity)),
                        ResourceActivities.AccessRequirement(typeof(IdentityResourceEntity)),
                        ResourceActivities.AccessRequirement(typeof(ResourceEntity))
                    })));

                AddConfigurationActivities(services, options, configuration);
                AddDefaultPolicies(services, options);
                configureAction?.Invoke(options);
            });

            return services;
        }

        /// <summary>An IServiceCollection extension method that adds a configuration policies.</summary>
        /// <param name="services">             The services to act on. </param>
        /// <param name="authorizationOptions"> Options for controlling the authorization. </param>
        /// <param name="configuration">        The configuration. </param>
        public static void AddConfigurationActivities(this IServiceCollection services,
            AuthorizationOptions authorizationOptions, IConfigurationRoot configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IAuthorizationDataService>();

            var options = configuration.Configure<ActivityAuthorizationOptions>(services);
            var activities = new[]
            {
                Activities.Access,
                Activities.Create,
                Activities.Read,
                Activities.Update,
                Activities.Delete
            };

            // add resources as activities to the database
            using (var uow = service.StartUnitOfWork())
            {
                foreach (var resource in options.Resources)
                {
                    foreach (var activity in activities)
                    {
                        var entry = uow.ActivityRepository.GetByResourceAndActivity(resource.Name, activity.Name);
                        if (entry == null)
                        {
                            uow.ActivityRepository.Add(new ActivityEntity
                            { 
                                Name = activity.Name,
                                ResourceName  = resource.Name,
                                ResourceDisplayName = resource.DisplayName,
                                GroupName = resource.GroupName,
                                GroupDisplayName = resource.GroupDisplayName
                            });
                        }
                    }
                }
                uow.Commit();
            }
        }

        /// <summary>An IServiceCollection extension method that adds a default policies.</summary>
        /// <param name="services">             The services to act on. </param>
        /// <param name="authorizationOptions"> Options for controlling the authorization. </param>
        public static void AddDefaultPolicies(this IServiceCollection services, AuthorizationOptions authorizationOptions)
        {
            foreach (var policy in new DefaultPolicies().Policies)
            {
                authorizationOptions.AddPolicy(policy.Name, builder =>
                {
                    foreach (var requirement in policy.Requirements)
                    {
                        builder.Requirements.Add(requirement);
                    }
                });
            }
        }
    }
}