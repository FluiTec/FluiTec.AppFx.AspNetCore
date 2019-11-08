using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>	A logging extension. </summary>
    public static class LoggingExtension
    {
        /// <summary>	An IApplicationBuilder extension method that use logging. </summary>
        /// <param name="app">				The app to act on. </param>
        /// <param name="environment">  	The environment. </param>
        /// <param name="appLifetime">  	The application lifetime. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <param name="loggerFactory">	The logger factory. </param>
        /// <returns>	An IApplicationBuilder. </returns>
        /// <remarks>
        ///     Development:
        ///     - This class enables Console and Debug-Logging
        ///     Otherwise:
        ///     - This class assumes u installed a serilog-sink and configure it via appsettings,
        ///     wrapping the sink in the AsyncSink
        /// </remarks>
        public static IApplicationBuilder UseLogging(this IApplicationBuilder app, IHostingEnvironment environment,
            IApplicationLifetime appLifetime, IConfigurationRoot configuration, ILoggerFactory loggerFactory)
        {
            if (environment.IsDevelopment())
            {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
                loggerFactory.AddConsole(configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
#pragma warning restore CS0618 // Typ oder Element ist veraltet
            }
            else
            {
                // async logger wrapping rolling-file logger
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Async(a =>
                        a.Logger(config => config.ReadFrom.Configuration(configuration)))
                    .CreateLogger();

                // add static Log to AspNetCoreLogging
                var fact = loggerFactory.AddSerilog();

                // close and flush on stopping application
                appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            }

            return app;
        }
    }
}