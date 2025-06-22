namespace DataAccessLayer.Models;

public class Game
{
    public int GameId { get; set; }
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public decimal BetAmount { get; set; }
    public DateTime Date { get; set; }
    public int PlayerPoints { get; set; }
    public int ComputerPoints { get; set; }
    public bool WonGame { get; set; }
}