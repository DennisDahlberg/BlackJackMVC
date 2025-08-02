using DataAccessLayer.Models;

namespace Services.Services;

public class CardService
{
    public List<Card> CreateDeck()
    {
        var suits = new[] {"Spades", "Clubs", "Diamonds", "Hearts"};
        var ranks = new[] {"A", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "J", "Q", "K"};
        
        var deck = new List<Card>();

        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                deck.Add(new Card
                {
                    Suit = suit,
                    Rank = rank,
                    ImageUrl = $"{rank}{suit[0]}.png"
                });
            }
        }
        return deck;
    }
    
    
}