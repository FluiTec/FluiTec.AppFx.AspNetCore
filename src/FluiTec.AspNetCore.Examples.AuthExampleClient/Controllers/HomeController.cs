using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluiTec.AspNetCore.Examples.AuthExampleClient.Models;
using Microsoft.AspNetCore.Authorization;

namespace FluiTec.AspNetCore.Examples.AuthExampleClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
