using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

// Source: http://benjii.me/2017/01/is-active-route-tag-helper-asp-net-mvc-core/

namespace FluiTec.AppFx.AspNetCore.TagHelpers
{
    /// <summary>
    ///     An active route tag helper giving the used html-element the class "active" if it's active.
    ///     Usage: 1. Add TagHelper to ViewImports.cshtml like '@addTagHelper *, FluiTec.AppFx.AspNetCore
    ///     2. Specify Controller, Action and is-active-route on (any!) html-element like
    ///     'li class="nav-item" is-active-route asp-controller="Home" asp-action="Index"'
    /// </summary>
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        private IDictionary<string, string> _routeValues;

        /// <summary>The name of the action method.</summary>
        /// <remarks>
        ///     Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-
        ///     <c>null</c>.
        /// </remarks>
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        /// <summary>The name of the controller.</summary>
        /// <remarks>
        ///     Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-
        ///     <c>null</c>.
        /// </remarks>
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        /// <summary>Additional parameters for the route.</summary>
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get => _routeValues ?? (_routeValues =
                       new Dictionary<string, string>(
                           StringComparer.OrdinalIgnoreCase));
            set => _routeValues = value;
        }

        /// <summary>
        ///     Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.Rendering.ViewContext" /> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext] public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (ShouldBeActive()) MakeActive(output);

            output.Attributes.RemoveAll("is-active-route");
        }

        private bool ShouldBeActive()
        {
            var currentController = ViewContext.RouteData.Values["Controller"].ToString();
            var currentAction = ViewContext.RouteData.Values["Action"].ToString();

            if (!string.IsNullOrWhiteSpace(Controller) && !string.Equals(Controller, currentController,
                    StringComparison.CurrentCultureIgnoreCase)) return false;

            if (!string.IsNullOrWhiteSpace(Action) &&
                !string.Equals(Action, currentAction, StringComparison.CurrentCultureIgnoreCase)) return false;

            return RouteValues.All(routeValue =>
                ViewContext.RouteData.Values.ContainsKey(routeValue.Key) &&
                ViewContext.RouteData.Values[routeValue.Key].ToString() == routeValue.Value);
        }

        private static void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "active");
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null ||
                     classAttr.Value.ToString().IndexOf("active", StringComparison.Ordinal) < 0)
            {
                output.Attributes.SetAttribute("class", classAttr.Value == null
                    ? "active"
                    : classAttr.Value + " active");
            }
        }
    }
}