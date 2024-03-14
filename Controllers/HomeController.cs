using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DirectorySite.Models;
using DirectorySite.Data;
using Microsoft.Extensions.Options;
using DirectorySite.Helper;
using System.Security.Claims;
using System.Collections.Frozen;

namespace DirectorySite.Controllers;

[Auth]
public class HomeController : Controller
{
   
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Get token
        ViewData["Token"] = HttpContext.Session.GetString("JWTToken")??"";

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    
}
