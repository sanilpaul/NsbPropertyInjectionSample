using System.Diagnostics;
using DI.ServiceA;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DI.WebApp.MVC.Models;

namespace DI.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IDoSomeServiceAWork ServiceWorker { get; set; }

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
