using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using Services.ViewModels;

namespace BlackJackMVC.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly SetupService _setupService;

    public GameController(SetupService setupService)
    {
        _setupService = setupService;
    }

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