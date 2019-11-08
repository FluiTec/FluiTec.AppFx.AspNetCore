using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Models
{
    /// <summary>   A data Model for the pageable. </summary>
    public class PageableModel
    {
        /// <summary>   The current page. </summary>
        private int _currentPage;

        /// <summary>   Specialised default constructor for use only by derived class. </summary>
        protected PageableModel()
        {

        }

        /// <summary>   Gets or sets the current page. </summary>
        /// <value> The current page. </value>
        public int CurrentPage
        {
            get => _currentPage;
            set => _currentPage = value <= Pages ? value : Pages;
        }

        /// <summary>   Gets or sets the pages. </summary>
        /// <value> The pages. </value>
        public int Pages { get; set; }

        /// <summary>   Gets a value indicating whether this object has previous. </summary>
        /// <value> True if this object has previous, false if not. </value>
        public bool HasPrevious => CurrentPage != 1;

        /// <summary>   Gets a value indicating whether this object has next. </summary>
        /// <value> True if this object has next, false if not. </value>
        public bool HasNext => CurrentPage != Pages;

        /// <summary>   Gets or sets the controller. </summary>
        /// <value> The controller. </value>
        public string Controller { get; set; }

        /// <summary>   Gets or sets the action. </summary>
        /// <value> The action. </value>
        public string Action { get; set; }

        /// <summary>   Initialises this object. </summary>
        /// <param name="controller">   The controller. </param>
        /// <param name="action">       The action. </param>
        protected void InitRouteInternal(Controller controller, string action)
        {
            var controllerName = controller.GetType().Name;
            Controller = controllerName.Substring(0, controllerName.LastIndexOf("Controller", StringComparison.Ordinal));
            Action = action;
        }
    }

    /// <summary>   A data Model for the pageable. </summary>
    /// <typeparam name="TItem">    Type of the item. </typeparam>
    public class PageableModel<TItem> : PageableModel
    {
        /// <summary>   Gets or sets the items. </summary>
        /// <value> The items. </value>
        public IEnumerable<TItem> Items { get; set; }

        /// <summary>   Pages a set of existing items. </summary>
        /// <param name="items">        The items. </param>
        /// <param name="currentPage">  (Optional) The current page. </param>
        /// <param name="maxCount">     (Optional) Number of maximums. </param>
        /// <returns>   A PageableModel&lt;TItem&gt; </returns>
        public static PageableModel<TItem> PageExisting(IEnumerable<TItem> items, int currentPage = 1, int maxCount = 20)
        {
            // make sure we dont get bullshit
            if (currentPage < 1) currentPage = 1;
            if (maxCount < 1) maxCount = 10;

            var model = new PageableModel<TItem>();
            var enumerable = items as TItem[] ?? items.ToArray();

            // calculate number of pages
            var modAddition = enumerable.Length % maxCount > 0 ? 1 : 0;
            model.Pages = enumerable.Any() ? enumerable.Length / maxCount + modAddition : 1;

            // make sure currentPage isnt too big
            if (currentPage > model.Pages)
                currentPage = model.Pages;

            // take the needed elements
            model.Items = !enumerable.Any()
                ? Enumerable.Empty<TItem>()
                : enumerable.Skip((currentPage-1) * maxCount).Take(maxCount);
            model.CurrentPage = currentPage;

            return model;
        }

        /// <summary>   Initialises the route. </summary>
        /// <param name="controller">   The controller. </param>
        /// <param name="action">       The action. </param>
        /// <returns>   A PageableModel&lt;TItem&gt; </returns>
        public PageableModel<TItem> InitRoute(Controller controller, string action)
        {
            InitRouteInternal(controller, action);
            return this;
        }
    }
}
