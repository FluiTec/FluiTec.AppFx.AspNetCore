using System.Threading.Tasks;
using FluiTec.AppFx.AspNetCore.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FluiTec.AppFx.AspNetCore.CultureProviders
{
    /// <summary>   A path string request culture provider. </summary>
    public class PathStringRequestCultureProvider : RouteDataRequestCultureProvider
    {
        /// <summary>   Determine provider culture result. </summary>
        /// <param name="httpContext">  Context for the HTTP. </param>
        /// <returns>   An asynchronous result that yields a ProviderCultureResult. </returns>
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            // dont even try if there's no path
            var path = httpContext.Request?.Path;
            if (!path.HasValue || string.IsNullOrWhiteSpace(path.Value.Value))
                return Task.FromResult<ProviderCultureResult>(null);

            // check if the first segment of the path starts with any available culture
            var realPath = path.Value;
            var cultureOptions = httpContext.RequestServices.GetRequiredService<CultureOptions>();
            foreach (var culture in cultureOptions.SupportedCultures)
            {
                var startsWithCulture = realPath.StartsWithSegments($"/{culture}") || realPath.Value == $"/{culture}";
                if (startsWithCulture) // if so - provide discovered culture
                {
                    return Task.FromResult(new ProviderCultureResult(culture));
                }
            }

            return Task.FromResult<ProviderCultureResult>(null);
        }
    }
}