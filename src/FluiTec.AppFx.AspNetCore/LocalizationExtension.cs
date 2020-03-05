using System;
using System.Globalization;
using System.Linq;
using FluiTec.AppFx.AspNetCore;
using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.AspNetCore.CultureProviders;
using FluiTec.AppFx.AspNetCore.RouteConstraints;
using FluiTec.AppFx.Localization;
using FluiTec.AppFx.Localization.Entities;
using FluiTec.AppFx.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>	A localization extension. </summary>
    public static class LocalizationExtension
    {
        /// <summary>	Options for controlling the operation. </summary>
        private static CultureOptions _options;

        /// <summary>	An IServiceCollection extension method that configure localization. </summary>
        /// <param name="services">			The services to act on. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <param name="configure">		The configure. </param>
        /// <returns>	An IServiceCollection. </returns>
        public static IServiceCollection ConfigureLocalization(this IServiceCollection services,
            IConfigurationRoot configuration, Action<CultureOptions> configure = null)
        {
            // add localization
            services.AddLocalization();

            // parse options
            _options = configuration.Configure<CultureOptions>(services);

            // let user apply changes
            configure?.Invoke(_options);

            // register options
            services.AddSingleton(_options);

            // configure localization
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = _options.SupportedCultures.Select(e => new CultureInfo(e)).ToList();
                options.DefaultRequestCulture = new RequestCulture(_options.DefaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            
            services.ConfigureLocalizationDataService(configuration);
            services.AddDbLocalizationProvider();

            // import json-defined localizations
            var resourceOptions = configuration.Configure<DbLocalizationResourceOptions>(services);
            if (resourceOptions == null || !resourceOptions.Resources.Any())
                return services;
            ImportJsonLocalizations(services, resourceOptions);

            return services;
        }

        /// <summary>
        ///     An IServiceCollection extension method that configure route localization.
        /// </summary>
        /// <param name="services">         The services to act on. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>   An IServiceCollection. </returns>
        public static IServiceCollection ConfigureRouteLocalization(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.PostConfigure<RequestLocalizationOptions>(options =>
            {
                var provider = new PathStringRequestCultureProvider
                {
                    RouteDataStringKey = "language",
                    UIRouteDataStringKey = "language",
                    Options = options
                };
                options.RequestCultureProviders.Insert(0, provider);
                options.FallBackToParentCultures = true;
                options.FallBackToParentUICultures = true;
            });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("language", typeof(LanguageRouteConstraint));
            });

            return services;
        }

        /// <summary>   Import JSON localizations. </summary>
        /// <param name="services">         The services to act on. </param>
        /// <param name="resourceOptions">  Options for controlling the resource. </param>
        private static void ImportJsonLocalizations(IServiceCollection services, DbLocalizationResourceOptions resourceOptions)
        {
            var resources = resourceOptions.Resources;
            using (var uow = services.BuildServiceProvider().GetService<ILocalizationDataService>().StartUnitOfWork())
            {
                foreach (var resource in resources)
                {
                    var r = uow.ResourceRepository.GetByKey(resource.ResourceKey) ?? uow.ResourceRepository.Add(
                                new ResourceEntity
                                {
                                    Author = resource.Author,
                                    FromCode = resource.FromCode,
                                    IsHidden = resource.IsHidden,
                                    IsModified = resource.IsModified,
                                    ModificationDate = resource.ModificationDate,
                                    ResourceKey = resource.ResourceKey
                                });

                    var existingTranslations = uow.TranslationRepository.ByResource(r).ToList();
                    foreach (var translation in resource.Translations)
                        if (!existingTranslations.Any(e => e.ResourceId == r.Id && e.Language == translation.Language))
                            uow.TranslationRepository.Add(new TranslationEntity
                            {
                                Language = translation.Language,
                                ResourceId = r.Id,
                                Value = translation.Value
                            });
                }

                uow.Commit();
            }
        }

        /// <summary>	An IApplicationBuilder extension method that use localization. </summary>
        /// <param name="app">				The app to act on. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <returns>	An IApplicationBuilder. </returns>
        public static IApplicationBuilder UseLocalization(this IApplicationBuilder app,
            IConfigurationRoot configuration)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseDbLocalizationProvider();

            return app;
        }
    }
}