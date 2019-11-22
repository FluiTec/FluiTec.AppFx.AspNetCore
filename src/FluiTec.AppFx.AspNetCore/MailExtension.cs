using System;
using System.Globalization;
using System.IO;
using FluiTec.AppFx.Mail;
using FluiTec.AppFx.Mail.Configuration;
using FluiTec.AppFx.Mail.LocationExpanders;
using FluiTec.AppFx.Mail.RazorLightProjects;
using FluiTec.AppFx.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RazorLight;
using RazorLight.Caching;

namespace FluiTec.AppFx.AspNetCore
{
    /// <summary>	A mail service extension. </summary>
    public static class MailServiceExtension
    {
        /// <summary>	Options for controlling the operation. </summary>
        private static MailServiceOptions _options;

        /// <summary>	Configure mail service. </summary>
        /// <param name="services">			The services. </param>
        /// <param name="configuration">	The configuration. </param>
        /// <param name="environment">  	The environment. </param>
        /// <param name="configure">		The configure. </param>
        /// <returns>	An IServiceCollection. </returns>
        public static IServiceCollection ConfigureMailService(this IServiceCollection services,
            IConfigurationRoot configuration, IWebHostEnvironment environment,
            Action<MailServiceOptions> configure = null)
        {
            // parse options
            _options = configuration.GetConfiguration<MailServiceOptions>();

            // let user apply changes
            configure?.Invoke(_options);

            // register
            services.AddSingleton(_options);

            services.AddRazorLight(environment, _options.TemplateRoot);
            services.AddScoped<ITemplatingMailService, MailKitRazorLightTemplatingMailService>();

            return services;
        }

        /// <summary>	An IServiceCollection extension method that adds a razor light self. </summary>
        /// <param name="services">   	The services. </param>
        /// <param name="environment">	The environment. </param>
        /// <param name="root">		  	The root. </param>
        private static void AddRazorLight(this IServiceCollection services, IWebHostEnvironment environment,
            string root)
        {
            var absoluteRoot = Path.Combine(environment.WebRootPath, root);

            var engine = new RazorLightEngineBuilder()
                .UseFilesystemProject(absoluteRoot)
                .UseMemoryCachingProvider()
                .Build();

            services.AddSingleton<IRazorLightEngine>(engine);
        }

        /// <summary>An IServiceCollection extension method that adds a razor light self.</summary>
        /// <param name="services">             The services. </param>
        /// <param name="configuration">        The configuration. </param>
        /// <param name="environment">          The environment. </param>
        /// <param name="configure">            (Optional) The configure. </param>
        /// <param name="templateKeyExpander">  The template key expander. </param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection ConfigureMailServiceLocalized(this IServiceCollection services,
            IConfigurationRoot configuration, IWebHostEnvironment environment,
            Func<CultureInfo, string, string> templateKeyExpander,
            Action<MailServiceOptions> configure = null)
        {
            // parse options
            _options = configuration.GetConfiguration<MailServiceOptions>();

            // let user apply changes
            configure?.Invoke(_options);

            // register
            services.AddSingleton(_options);
            services.AddRazorLightLocalized(environment, _options.TemplateRoot, _options, templateKeyExpander);
            services.AddScoped<ITemplatingMailService, MailKitRazorLightTemplatingMailService>();

            return services;
        }

        /// <summary>An IServiceCollection extension method that adds a razor light localized.</summary>
        /// <param name="services">             The services. </param>
        /// <param name="environment">          The environment. </param>
        /// <param name="root">                 The root. </param>
        /// <param name="options">              Options for controlling the operation. </param>
        /// <param name="templateKeyExpander">  The template key expander. </param>
        private static void AddRazorLightLocalized(this IServiceCollection services, IWebHostEnvironment environment,
            string root, MailServiceOptions options, Func<CultureInfo, string, string> templateKeyExpander)
        {
            var absoluteRoot = Path.Combine(environment.WebRootPath, root);

            var project = new CultureAwareRazorProject(options, absoluteRoot, templateKeyExpander);
            var engine = new RazorLightEngineBuilder()
                .UseProject(project)
                .UseCachingProvider(new MemoryCachingProvider())
                .Build();
            services.AddSingleton<IRazorLightEngine>(engine);
        }

        /// <summary>An IServiceCollection extension method that configure mail service templated.</summary>
        /// <param name="services">         The services. </param>
        /// <param name="environment">      The environment. </param>
        /// <param name="configuration">    The configuration. </param>
        /// <param name="configure">        (Optional) The configure. </param>
        /// <returns>An IServiceCollection.</returns>
        public static IServiceCollection ConfigureMailServiceTemplated(this IServiceCollection services,
            IWebHostEnvironment environment, IConfigurationRoot configuration,
            Action<MailServiceOptions> configure = null)
        {
            // parse options
            _options = configuration.GetConfiguration<MailServiceOptions>();

            // let user apply changes
            configure?.Invoke(_options);

            // register
            services.AddSingleton(_options);
            services.AddRazorLightTemplating(environment, _options, _options.TemplateRoot);
            services.AddScoped<ITemplatingMailService, MailKitRazorLightTemplatingMailService>();

            return services;
        }

        /// <summary>An IServiceCollection extension method that adds a razor light templating.</summary>
        /// <param name="services">     The services. </param>
        /// <param name="environment">  The environment. </param>
        /// <param name="options">      Options for controlling the operation. </param>
        /// <param name="root">         The root. </param>
        private static void AddRazorLightTemplating(this IServiceCollection services, IWebHostEnvironment environment,
            MailServiceOptions options, string root)
        {
            var provider = services.BuildServiceProvider();
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

            var absoluteRoot = Path.Combine(environment.WebRootPath, root);

            var expanders = new ILocationExpander[]
            {
                new DefaultCultureLocationExpander(),
                new SharedCultureLocationExpander(),
                new SharedLocationExpander(),
                new DefaultLocationExpander()
            };

            var project = new LocationExpandingRazorProject(options, absoluteRoot, expanders, loggerFactory);
            var engine = new RazorLightEngineBuilder()
                .UseProject(project)
                .UseCachingProvider(new MemoryCachingProvider())
                .Build();

            services.AddSingleton<IRazorLightEngine>(engine);
        }
    }
}