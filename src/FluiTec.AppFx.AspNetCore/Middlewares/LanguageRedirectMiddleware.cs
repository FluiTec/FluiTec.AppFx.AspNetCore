using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluiTec.AppFx.AspNetCore.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;

namespace FluiTec.AppFx.AspNetCore.Middlewares
{
    /// <summary>   A language middleware. </summary>
    public class LanguageRedirectMiddleware
    {
        private List<string> _cultures;

        /// <summary>   The next. </summary>
        private readonly RequestDelegate _next;

        /// <summary>   Constructor. </summary>
        /// <param name="next"> The next. </param>
        public LanguageRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///     Executes the given operation on a different thread, and waits for the result.
        /// </summary>
        /// <param name="httpContext">  Context for the HTTP. </param>
        /// <returns>   An asynchronous result. </returns>
        public Task Invoke(HttpContext httpContext)
        {
            var routeData = httpContext.GetRouteData();
            if (!httpContext.Request.Path.HasValue || httpContext.Request.Path.Value == "/" || routeData != null && !routeData.Values.ContainsKey("language") && !ContainsLanguageSpecifier(httpContext))
                return RedirectToLanguage(httpContext);
            // otherwise - let other handlers do their work
            return _next(httpContext); 
        }

        /// <summary>   Query if 'context' contains language specifier. </summary>
        /// <param name="context">  The context. </param>
        /// <returns>   True if it succeeds, false if it fails. </returns>
        private bool ContainsLanguageSpecifier(HttpContext context)
        {
            if (_cultures == null)
            {
                var options = context.RequestServices.GetService(typeof(CultureOptions)) as CultureOptions;
                _cultures = options == null ? Enumerable.Empty<string>().ToList() : options.SupportedCultures;
            }

            var path = context.Request.Path.Value;
            return _cultures.Any(c => path.StartsWith($"/{c}") || path == $"/{c}");
        }

        /// <summary>   Redirect to language. </summary>
        /// <param name="httpContext">  Context for the HTTP. </param>
        /// <returns>   An asynchronous result. </returns>
        private Task RedirectToLanguage(HttpContext httpContext)
        {
            var language = string.Empty;
            var routeData = httpContext.GetRouteData();
            if (routeData != null && routeData.Values.ContainsKey("language")) // return url-specified language
            {
                language = routeData.Values["language"].ToString().ToLower();
            }
            if (string.IsNullOrEmpty(language))
            {
                var feature = httpContext.Features.Get<IRequestCultureFeature>();
                language = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
            }

            var newPath = $"/{language}";
            if (httpContext.Request.Path != null && httpContext.Request.Path != "/")
                newPath = newPath + httpContext.Request.Path;
            if (httpContext.Request.QueryString.HasValue && httpContext.Request.QueryString.Value != string.Empty)
                newPath = newPath + httpContext.Request.QueryString.Value;
            httpContext.Response.Redirect(newPath);
            return Task.CompletedTask;
        }
    }
}