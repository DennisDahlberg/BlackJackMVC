using DataAccessLayer.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Extensions;
using Services.Services;
using Services.ViewModels;

namespace BlackJackMVC.Controllers;

[Authorize]
public class GameController : Controller
{
    private readonly SetupService _setupService;
    private readonly CardService _cardService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly GameService _gameService;

    public GameController(SetupService setupService, UserManager<ApplicationUser> userManager, CardService cardService, GameService gameService)
    {
        _setupService = setupService;
        _userManager = userManager;
        _cardService = cardService;
        _gameService = gameService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var bet = new BetViewModel() { BetAmount = 0, Balance = user.Balance };
        
        return View(bet);
    }

    [HttpPost]
    public IActionResult Index(BetViewModel bet, decimal betToAdd)
    {
        bet.BetAmount += betToAdd;
        return View(bet);
    }

    [HttpPost]
    public async Task<IActionResult> StartGame(BetViewModel bet)
    {
        var user = await _userManager.GetUserAsync(User);
        var result = _setupService.IsBetValid(bet.BetAmount, user.Balance);
        if (!result)
        {
            TempData["Result"] = "Wager is too big!";
            return RedirectToAction("Index");
        }
        
        return RedirectToAction("Start", new {betAmount = bet.BetAmount});
    }

    public IActionResult Start(decimal betAmount)
    {
        if (betAmount <= 0)
            RedirectToAction("Index");
        var model = _cardService.CreateStartingState();
        HttpContext.Session.SetObject("GameState", model);
        return View(model);
    }

    [HttpPost]
    public IActionResult Draw()
    {
        var model = HttpContext.Session.GetObject<GameViewModel>("GameState");
        var result = _cardService.DrawCard(model.Deck);
        model.Deck = result.Item1;
        model.PlayerHand.Add(result.Item2);
        model.PlayerPoints = _cardService.CalculateHandPoints(model.PlayerHand);
        HttpContext.Session.SetObject("GameState", model);
        if (model.PlayerPoints >= 22)
            TempData["Result"] = "Bust!";
        
        
        return View("Start", model);
    }

    [HttpPost]
    public IActionResult Stand()
    {
        var model = HttpContext.Session.GetObject<GameViewModel>("GameState");
        var gameDTO = model.Adapt<GameDTO>();
        var updatedModel = _cardService.DrawDealerCards(gameDTO).Adapt<GameViewModel>();
        HttpContext.Session.SetObject("GameState", updatedModel);
        gameDTO = updatedModel.Adapt<GameDTO>();
        
        if (_gameService.CheckWin(gameDTO))
            TempData["Result"] = "You Win";
        else
            TempData["Result"] = "You Lose";
        
        return View("Start", updatedModel);
    }

    
}





