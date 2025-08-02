using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using Services.ViewModels;

namespace BlackJackMVC.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly SetupService _setupService;
    private readonly UserManager<ApplicationUser> _userManager;

    public GameController(SetupService setupService, UserManager<ApplicationUser> userManager)
    {
        _setupService = setupService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        ViewBag.BodyClass = "green-bg";
        var bet = new BetViewModel() { BetAmount = 0, Balance = user.Balance };
        
        return View(bet);
    }

    [HttpPost]
    public IActionResult Index(BetViewModel bet, decimal betToAdd)
    {
        ViewBag.BodyClass = "green-bg";
        bet.BetAmount += betToAdd;
        return View(bet);
    }

    [HttpPost]
    public async Task<IActionResult> StartGame(BetViewModel bet)
    {
        var user = await _userManager.GetUserAsync(User);
        // var result = _setupService.IsBetValid(bet.BetAmount, user.Balance);
        var result = _setupService.IsBetValid(bet.BetAmount, 1000);
        if (!result)
        {
            ViewBag.BodyClass = "green-bg";
            TempData["Result"] = "Wager is too big!";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Game");
    }

    public IActionResult Game()
    {
        return View();
    }
}