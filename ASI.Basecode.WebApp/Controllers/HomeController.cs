using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASI.Basecode.Data.Models;

namespace Student_Performance_Tracker.Controllers;

public class HomeController : Controller
{
    public HomeController() { }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
