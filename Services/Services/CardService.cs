using DataAccessLayer.Models;

namespace Services.Services;

public class CardService
{
    private readonly Random _random = new Random();
    public List<Card> CreateDeck()
    {
        var suits = new[] {"Spades", "Clubs", "Diamonds", "Hearts"};
        var ranks = new[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
        
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


    public Tuple<List<Card>, Card> DrawCard(List<Card> deck)
    {
        var index = _random.Next(0, deck.Count);
        var card = deck[index];
        deck.RemoveAt(index);
        
        return new Tuple<List<Card>, Card>(deck, card);
    }

    public List<Card> CreateHand(List<Card> deck)
    {
        var card1 = DrawCard(deck);
        var card2 = DrawCard(deck);
        
        var hand = new List<Card>
        {
            card1.Item2,
            card2.Item2
        };
        return hand;
    }
    
}