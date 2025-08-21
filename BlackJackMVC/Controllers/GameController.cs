using System.Globalization;
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
        if (user == null)
            return RedirectToAction("Index", "Home");
        if (TempData["BetAmount"] != null)                                                                                                                                                                                                                                                                                                                                                                                                            
        {
            var betAmount = Convert.ToDecimal(TempData["BetAmount"]);
            var bet = new BetViewModel() { BetAmount = betAmount, Balance = user.Balance };
            return View(bet);
        }
        else
        {
            var bet = new BetViewModel() { BetAmount = 0, Balance = user.Balance };
            return View(bet);
        }
    }

    [HttpPost]
    public IActionResult Index(BetViewModel bet, decimal betToAdd)
    {
        bet.BetAmount += betToAdd;
        TempData["BetAmount"] = bet.BetAmount.ToString();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> StartGame(BetViewModel bet)
    {
        if (bet.BetAmount <= 0)
        {
            TempData["Result"] = "You need to enter a wager!";
            return RedirectToAction("Index");
        }
        var user = await _userManager.GetUserAsync(User);
        var result = _setupService.IsBetValid(bet.BetAmount, user.Balance);
        if (!result)
        {
            TempData["Result"] = "Wager is too big!";
            return RedirectToAction("Index");
        }
        
        var model = _cardService.CreateStartingState(bet.BetAmount);
        model.Balance = user.Balance;
        HttpContext.Session.SetObject("GameState", model);
        return RedirectToAction("Start");
    }
    
    public IActionResult Start()
    {
        var model = HttpContext.Session.GetObject<GameViewModel>("GameState");
        if (model == null)
            return RedirectToAction("Index", "Home");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Draw()
    {
        var model = HttpContext.Session.GetObject<GameViewModel>("GameState");
        var result = _cardService.DrawCard(model.Deck);
        model.Deck = result.Item1;
        model.PlayerHand.Add(result.Item2);
        model.PlayerPoints = _cardService.CalculateHandPoints(model.PlayerHand);
        HttpContext.Session.SetObject("GameState", model);
        if (model.PlayerPoints >= 22)
        {
            TempData["Result"] = "Bust!";
            var gameDTO = model.Adapt<GameDTO>();
            var user = await _userManager.GetUserAsync(User);
            await _gameService.Save(gameDTO, user.Id);
        }
        
        return RedirectToAction("Start");
    }

    [HttpPost]
    public async Task<IActionResult> Stand()
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

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Index");
        await _gameService.Save(gameDTO, user.Id);
        
        return RedirectToAction("Start");
    }

    
}





