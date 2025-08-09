using DataAccessLayer.Models;

namespace Services.DTOs;

public class GameDTO
{
    public decimal BetAmount { get; set; }
    public int PlayerPoints { get; set; }
    public int ComputerPoints { get; set; }
    
    public List<Card> Deck { get; set; } = [];
    public List<Card> PlayerHand { get; set; } = [];
    public List<Card> HouseHand { get; set; } = [];
}