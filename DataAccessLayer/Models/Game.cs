namespace DataAccessLayer.Models;

public class Game
{
    public int GameId { get; set; }
    public DateTime Date { get; set; }
    // Foreign Reference to User
    public int PlayerPoints { get; set; }
    public int ComputerPoints { get; set; }
    public bool WonGame { get; set; }
}