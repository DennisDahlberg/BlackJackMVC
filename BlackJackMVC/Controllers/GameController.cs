using Microsoft.AspNetCore.Mvc;

namespace BlackJackMVC.Controllers;

public class GameController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}