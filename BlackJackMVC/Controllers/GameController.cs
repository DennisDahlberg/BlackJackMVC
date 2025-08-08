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
    private readonly CardService _cardService;
    private readonly UserManager<ApplicationUser> _userManager;

    public GameController(SetupService setupService, UserManager<ApplicationUser> userManager, CardService cardService)
    {
        _setupService = setupService;
        _userManager = userManager;
        _cardService = cardService;
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
        
        return RedirectToAction("Start", new {betAmount = bet.BetAmount});
    }

    public IActionResult Start(decimal betAmount)
    {
        if (betAmount <= 0)
            RedirectToAction("Index");
        ViewBag.BodyClass = "green-bg";
        var model = _cardService.CreateStartingState();
        
        return View(model);
    }
}





