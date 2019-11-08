using Microsoft.AspNetCore.Mvc;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample.Controllers
{
    /// <summary>	A controller for handling contacts. </summary>
    public class ContactController : Controller
    {
        /// <summary>	Gets the imprint. </summary>
        /// <returns>	An IActionResult. </returns>
        public IActionResult Imprint()
        {
            return View();
        }

        /// <summary>	Gets the privacy. </summary>
        /// <returns>	An IActionResult. </returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>	Terms of use. </summary>
        /// <returns>	An IActionResult. </returns>
        public IActionResult TermsOfUse()
        {
            return View();
        }
    }
}