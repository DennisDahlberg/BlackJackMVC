using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlackJackMVC.Models;

namespace BlackJackMVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.BodyClass = "green-bg";
        return View();
    }

    public IActionResult Rules()
    {
        ViewBag.BodyClass = "green-bg";
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