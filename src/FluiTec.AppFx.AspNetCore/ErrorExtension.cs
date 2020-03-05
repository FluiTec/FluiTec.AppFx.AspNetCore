using System;
using FluiTec.AppFx.AspNetCore.Configuration;
using FluiTec.AppFx.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>	An error extension. </summary>
    /// <remarks>
    ///     This class parses/configures error-handling-options from the configuration,
    ///     and depending from the current HostingEnvironment, decides to either use
    ///     the default DeveloperExceptionPage or the UserDefined ErrorHandlingPath.
    /// </remarks>
    public static class ErrorExtension
    {
        /// <summary>	Options for controlling the operation. </summary>
        private static ErrorOptions _options;

        /// <summary>	An IServiceCollection extension method that configures error handling. </summary>
        /// <param name="services">			The services to act on. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <param name="configure">		The configure. </param>
        /// <returns>	An IServiceCollection. </returns>
        public static IServiceCollection ConfigureErrorHandling(this IServiceCollection services,
            IConfigurationRoot configuration, Action<ErrorOptions> configure = null)
        {
            // parse options from configuration
            _options = configuration.Configure<ErrorOptions>(services);

            // let the user apply additional changes
            configure?.Invoke(_options);

            // add options to the DI
            services.AddSingleton(configuration.Configure<ErrorOptions>(services));
            return services;
        }

        /// <summary>	An IApplicationBuilder extension method that use hosting services. </summary>
        /// <param name="app">				The app to act on. </param>
        /// <param name="environment">  	The environment. </param>
        /// <returns>	An IApplicationBuilder. </returns>
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder app,
            IWebHostEnvironment environment)
        {
            // if the environment is development - let .NetCore display the exception with full stacktrace
            if (environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // otherwise - let the error-handler do it's job
            else
                app.UseExceptionHandler(_options.ErrorHandlingPath);

            return app;
        }
    }
}