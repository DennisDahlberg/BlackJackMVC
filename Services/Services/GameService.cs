using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Services.DTOs;
using Services.ViewModels;

namespace Services.Services;

public class GameService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public GameService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public bool CheckWin(GameDTO model)
    {
        if (model.PlayerPoints > 21)
            return false;
        if (model.ComputerPoints < 22 && model.ComputerPoints >= model.PlayerPoints)
            return false;
        return true;
    }

    public async Task Save(GameDTO model, string userId)
    {
        var game = model.Adapt<Game>();
        var user = await _userManager.FindByIdAsync(userId);
        
        game.Date = DateTime.UtcNow;
        game.WonGame = CheckWin(model);
        if (game.WonGame)
            user.Balance += game.BetAmount;
        else
        {
            user.Balance -= game.BetAmount;
        }
        game.ApplicationUserId = userId;
        game.ApplicationUser = user;
        
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
    }
    
}