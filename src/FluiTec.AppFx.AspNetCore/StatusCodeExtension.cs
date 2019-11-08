using System;
using System.Globalization;
using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>	The status code extension. </summary>
    /// <remarks>
    ///     This class add's a middleware that calls a user-defined StatusCodeHandler
    ///     for SelfHandled StatusCodes, except for API-only requests.
    /// </remarks>
    public static class StatusCodeExtension
    {
        /// <summary>	Options for controlling the StatusCodeExtension. </summary>
        private static StatusCodeOptions _options;

        /// <summary>	Options for controlling the API. </summary>
        private static ApiOptions _apiOptions;

        /// <summary>
        ///     An IServiceCollection extension method that handler, called when the configure status code.
        /// </summary>
        /// <param name="services">			The services to act on. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <param name="configure">		The configure. </param>
        /// <returns>	An IServiceCollection. </returns>
        public static IServiceCollection ConfigureStatusCodeHandler(this IServiceCollection services,
            IConfigurationRoot configuration, Action<StatusCodeOptions> configure = null)
        {
            // parse options
            _options = configuration.GetConfiguration<StatusCodeOptions>();
            _apiOptions = configuration.GetConfiguration<ApiOptions>();

            // let user apply chjanges
            configure?.Invoke(_options);

            // register options
            services.AddSingleton(_options);

            return services;
        }

        /// <summary>
        ///     An IApplicationBuilder extension method that handler, called when the use status code.
        /// </summary>
        /// <param name="app">				The app to act on. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <returns>	An IApplicationBuilder. </returns>
        public static IApplicationBuilder UseStatusCodeHandler(this IApplicationBuilder app,
            IConfigurationRoot configuration)
        {
            const string pathFormat = "/StatusCode/{0}";

            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.UseStatusCodePages(async context =>
            {
                // check if we dont have an API-request here and want to handle the statuscode
                if (!context.HttpContext.Request.Path.StartsWithSegments(_apiOptions.ApiOnlyPath)
                    && _options.SelfHandledCodes.Contains(context.HttpContext.Response.StatusCode))

                    // if so - let the statuscodehandler do its stuff
                {
                    var newPath = new PathString(
                        string.Format(CultureInfo.InvariantCulture, pathFormat,
                            context.HttpContext.Response.StatusCode));

                    var originalPath = context.HttpContext.Request.Path;
                    var originalQueryString = context.HttpContext.Request.QueryString;

                    // Store the original paths so the app can check it.
                    context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature
                    {
                        OriginalPathBase = context.HttpContext.Request.PathBase.Value,
                        OriginalPath = originalPath.Value,
                        OriginalQueryString = originalQueryString.HasValue ? originalQueryString.Value : null
                    });

                    context.HttpContext.Request.Path = newPath;
                    context.HttpContext.Request.QueryString = QueryString.Empty;
                    try
                    {
                        await context.Next(context.HttpContext);
                    }
                    finally
                    {
                        context.HttpContext.Request.QueryString = originalQueryString;
                        context.HttpContext.Request.Path = originalPath;
                        context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(null);
                    }
                }
            });
        }

        /// <summary>
        ///     An IApplicationBuilder extension method that use status code handler with language.
        /// </summary>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        /// <param name="app">              The app to act on. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>   An IApplicationBuilder. </returns>
        public static IApplicationBuilder UseStatusCodeHandlerWithLanguage(this IApplicationBuilder app,
           IConfigurationRoot configuration)
        {
            const string pathFormat = "/{0}/StatusCode/Index/{1}";

            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.UseStatusCodePages(async context =>
            {
                var requestLanguage = context.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.TwoLetterISOLanguageName;
                // check if we dont have an API-request here and want to handle the statuscode
                if (!context.HttpContext.Request.Path.StartsWithSegments(_apiOptions.ApiOnlyPath)
                    && _options.SelfHandledCodes.Contains(context.HttpContext.Response.StatusCode))

                // if so - let the statuscodehandler do its stuff
                {
                    var newPath = new PathString(
                        string.Format(CultureInfo.InvariantCulture, pathFormat,
                            requestLanguage, context.HttpContext.Response.StatusCode));

                    var originalPath = context.HttpContext.Request.Path;
                    var originalQueryString = context.HttpContext.Request.QueryString;

                    // Store the original paths so the app can check it.
                    context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature
                    {
                        OriginalPathBase = context.HttpContext.Request.PathBase.Value,
                        OriginalPath = originalPath.Value,
                        OriginalQueryString = originalQueryString.HasValue ? originalQueryString.Value : null
                    });

                    context.HttpContext.Request.Path = newPath;
                    context.HttpContext.Request.QueryString = QueryString.Empty;
                    try
                    {
                        await context.Next(context.HttpContext);
                    }
                    finally
                    {
                        context.HttpContext.Request.QueryString = originalQueryString;
                        context.HttpContext.Request.Path = originalPath;
                        context.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(null);
                    }
                }
            });
        }
    }
}