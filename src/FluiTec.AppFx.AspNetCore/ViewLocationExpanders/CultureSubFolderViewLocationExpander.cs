using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace FluiTec.AppFx.AspNetCore.ViewLocationExpanders
{
    /// <summary>   A culture sub folder view location expander. </summary>
    public class CultureSubFolderViewLocationExpander : IViewLocationExpander
    {
        /// <summary>
        ///     Invoked by a <see cref="T:Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine" /> to determine
        ///     the values that would be consumed by this instance of
        ///     <see cref="T:Microsoft.AspNetCore.Mvc.Razor.IViewLocationExpander" />. The calculated
        ///     values are used to determine if the view location has changed since the last time it was
        ///     located.
        /// </summary>
        /// <param name="context">
        ///     The
        ///     <see cref="T:Microsoft.AspNetCore.Mvc.Razor.ViewLocationExpanderContext" />
        ///     for the current view location expansion operation.
        /// </param>
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // ignore
        }

        /// <summary>
        ///     Invoked by a <see cref="T:Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine" /> to determine
        ///     potential locations for a view.
        /// </summary>
        /// <param name="context">
        ///     The
        ///     <see cref="T:Microsoft.AspNetCore.Mvc.Razor.ViewLocationExpanderContext" />
        ///     for the current view location expansion operation.
        /// </param>
        /// <param name="viewLocations">    The sequence of view locations to expand. </param>
        /// <returns>   A list of expanded view locations. </returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            var shortName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var name = CultureInfo.CurrentUICulture;

            return new[]
            {
                "Views/{1}/" + shortName + "/{0}.cshtml",
                "Views/{1}/" + name + "/{0}.cshtml",
                "Views/{1}/{0}.cshtml",

                "Views/Shared/" + shortName + "/{0}.cshtml",
                "Views/Shared/" + name + "/{0}.cshtml",
                "Views/Shared/{0}.cshtml"
            };
        }
    }
}