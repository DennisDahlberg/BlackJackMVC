using DataAccessLayer.Models;

namespace Services.ViewModels;

public class GameViewModel
{
    public decimal BetAmount { get; set; }
    public int PlayerPoints { get; set; }
    public int ComputerPoints { get; set; }
    
    public List<Card> Deck { get; set; } = [];
}