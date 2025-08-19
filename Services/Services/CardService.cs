using DataAccessLayer.Models;
using Services.DTOs;
using Services.ViewModels;

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
    
    public GameViewModel CreateHands(List<Card> deck)
    {
        var model = new GameViewModel();
        var card1 = DrawCard(deck);
        var card2 = DrawCard(deck);
        var card3 = DrawCard(deck);
        var card4 = DrawCard(deck);
        
        var playerHand = new List<Card>
        {
            card1.Item2,
            card2.Item2
        };
        var houseHand = new List<Card>
        {
            card3.Item2,
            card4.Item2
        };
        model.PlayerHand = playerHand;
        model.HouseHand = houseHand;
        return model;
    }

    public GameViewModel CreateStartingState(decimal betAmount)
    {
        var deck = CreateDeck();
        var model = CreateHands(deck);
        model.HouseHand[1].HideCard = model.HouseHand[1].ImageUrl;
        model.HouseHand[1].ImageUrl = $"blue_back.png";
        model.BetAmount = betAmount;
        model.Deck = deck;
        model.ComputerPoints = CalculateHandPoints([model.HouseHand[0]]);
        model.PlayerPoints = CalculateHandPoints(model.PlayerHand);
        return model;
    }


    public Tuple<List<Card>, Card> DrawCard(List<Card> deck)
    {
        var index = _random.Next(0, deck.Count);
        var card = deck[index];
        deck.RemoveAt(index);
        
        return new Tuple<List<Card>, Card>(deck, card);
    }

    public int CalculateHandPoints(List<Card> deck)
    {
        var totalPoints = 0;
        var aces = 0;
        foreach (var card in deck)
        {
            if (card.Rank == "A")
            {
                aces++;
                totalPoints += 11;
            }
                
            else if (card.Rank == "K" ||
                card.Rank == "Q" ||
                card.Rank == "J" ||
                card.Rank == "A")
            {
                totalPoints += 10;
            }
            else
            {
                totalPoints += int.Parse(card.Rank);
            }
        }

        while (totalPoints > 21 && aces > 0)
        {
            totalPoints -= 10;
            aces--;
        }
        return totalPoints;
    }

    public GameDTO DrawDealerCards(GameDTO model)
    {
        model.HouseHand[1].ImageUrl = model.HouseHand[1].HideCard;
        model.ComputerPoints = CalculateHandPoints(model.HouseHand);
        while (true)
        {
            if (model.ComputerPoints < 17 && model.ComputerPoints < model.PlayerPoints)
            {
                var card = DrawCard(model.Deck);
                model.HouseHand.Add(card.Item2);
                model.Deck = card.Item1;
                model.ComputerPoints = CalculateHandPoints(model.HouseHand);
            }
            else
                return model;
        }
    }



}