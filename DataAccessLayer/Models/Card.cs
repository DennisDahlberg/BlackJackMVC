namespace DataAccessLayer.Models;

public class Card
{
    // public int CardId { get; set; }
    public string Suit { get; set; } = null!;
    public string Rank { get; set; } = null!;
    public bool HideCard { get; set; }
    public string ImageUrl { get; set; } = null!;
    // public int GameId { get; set; }
    // public Game Game { get; set; } = null!;
}