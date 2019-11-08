using System.Diagnostics;
using FluiTec.AppFx.AspNetCore.Examples.LocalizationExample.Models;
using FluiTec.AppFx.AspNetCore.Examples.LocalizationExample.Resources;
using FluiTec.AppFx.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FluiTec.AppFx.AspNetCore.Examples.LocalizationExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<ChildResource> _localizer;

        public HomeController(IStringLocalizer<ChildResource> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            ViewData["NormalProperty"] = _localizer.GetString(r => r.NormalProperty);
            ViewData["StaticProperty"] = _localizer.GetString(r => Resource.StaticProperty);
            ViewData["RefactoredProperty"] = _localizer.GetString(r => r.RefactoredProperty);

            return View();
        }

        [HttpGet]
        public IActionResult AnnotationDummy()
        {
            return View(new AnnotationModel());
        }

        [HttpPost]
        public IActionResult AnnotationDummy(AnnotationModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IsError"] = "Fehler verursacht";
            }

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}