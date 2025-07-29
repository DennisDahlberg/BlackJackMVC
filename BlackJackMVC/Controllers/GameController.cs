using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace BlackJackMVC.Controllers;

public class GameController : Controller
{
    public IActionResult Index()
    {
        var bet = new BetViewModel() { BetAmount = 0 };
        return View(bet);
    }

    [HttpPost]
    public IActionResult Index(BetViewModel bet, decimal betToAdd)
    {
        bet.BetAmount += betToAdd;
        return View(bet);
    }
}