using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SportsPro.Models;

namespace SportsPro.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

  [Route("/")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("[action]")]
    public IActionResult Product()
    {
        return View();
    }
    [Route("[action]")]
    public IActionResult Technician()
    {
        return View();
    }

    [Route("[action]")]
    public IActionResult Customer()
    {
        return View();
    }

    [Route("[action]")]
    public IActionResult Incident()
    {
        return View();
    }

    [Route("[action]")]
    public IActionResult Registration()
    {
        return View();
    }

    [Route("[action]")]
    public IActionResult About()
    {
        return View();
    }

}

