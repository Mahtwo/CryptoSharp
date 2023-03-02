using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoSharp
{
    /// <summary>
    /// Class who represents a deck of cards.
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// List of cards in the deck.
        /// </summary>
        private LinkedList<Card> cards = new(); // 52 cards + 2 jokers

        /// <summary>
        /// Constructor of the deck without argument. Create a deck with 52 cards and 2 jokers (all cards in Card enum).
        /// </summary>
        public Deck()
        {
            foreach (Card c in Enum.GetValues(typeof(Card)))
            {
                cards.AddLast(c);
            }
        }

        /// <summary>
        /// Constructor of the deck with a list of cards. Create a deck with the cards in the list.
        /// </summary>
        /// <param name="cards">cards who will be in the deck</param>
        public Deck(LinkedList<Card> cards)
        {
            this.cards = cards;
        }

        /// <summary>
        /// Get the card at the index in the deck.
        /// </summary>
        /// <param name="index">Index of the card you want</param>
        /// <returns>Card a the index in parameters</returns>
        public Card GetCard(int index)
        {
            return cards.ElementAt(index);
        }

        /// <summary>
        /// Shuffle the deck randomly.
        /// </summary>
        public void Shuffle()
        {
            // shuffle the deck
            Random rnd = new();
            cards = new LinkedList<Card>(cards.OrderBy(x => rnd.Next()));
        }

        /// <summary>
        /// Find the position of a specific card in the deck
        /// </summary>
        /// <param name="c">Card you want to find the position in the deck</param>
        /// <returns>Position of the card in parameters</returns>
        public int FindCardPosition(Card c)
        {
            int pos = -1;
            for (int i = 0; i < 54; i++)
            {
                if (cards.ElementAt(i) == c)
                {
                    pos = i;
                    break;
                }
            }
            return pos;
        }

        /// <summary>
        /// Move card in the deck in a direction. You can move up or down a specific card and you can move several position at the same time
        /// If you move outside the front of the deck, the card will be in second to last position
        /// If you move outside the back of tje deck, the card will be in second position
        /// </summary>
        /// <param name="c">The card you want to move</param>
        /// <param name="direction">How many position you want to move and if the sign is minus, you move down, if the sign is plus, you move up</param>
        public void MoveCard(Card c, int direction) // direction -1 is down | direction +1 is up
        {
            int pos = FindCardPosition(c);
            int newPos = (((pos + direction) % 54) + 54) % 54; // canonical modulus (work with negative numbers, not native in C#)
            LinkedListNode<Card> node = cards.Find(GetCard(newPos))!;
            cards.Remove(c);
            if (direction > 0)
            {
                cards.AddAfter(node, c);
            }
            else
            {
                cards.AddBefore(node, c);
            }
        }

        /// <summary>
        /// Locate the two jokers and swap the deck of cards above the joker that is cards above the first joker with the deck of cards below the second joker which is in second place
        /// </summary>
        public void DoubleCutting()
        {
            // find jokers
            int posJoker1 = FindCardPosition(Card.Black_Joker);
            int posJoker2 = FindCardPosition(Card.Red_Joker);
            if (posJoker1 > posJoker2)
            {
                // swap jokers
                (posJoker2, posJoker1) = (posJoker1, posJoker2);
            }

            // get first part of the deck
            LinkedList<Card> firstPart = new();
            for (int i = 0; i < posJoker1; i++)
            {
                firstPart.AddLast(GetCard(i));
            }

            // get second part of the deck
            LinkedList<Card> secondPart = new();
            for (int i = posJoker1; i <= posJoker2; i++)
            {
                secondPart.AddLast(GetCard(i));
            }

            // get third part of the deck
            LinkedList<Card> thirdPart = new();
            for (int i = posJoker2 + 1; i < 54; i++)
            {
                thirdPart.AddLast(GetCard(i));
            }

            // rebuild the deck
            cards = new LinkedList<Card>(thirdPart.Concat(secondPart).Concat(firstPart));
        }

        /// <summary>
        /// Get the Bridge Number of a specific card.
        /// The bridge number is the number of the card in normal order in deck. 
        /// Jokers will be 53. 
        /// The first card will be 1.
        /// </summary>
        /// <param name="c">Card you want Bridge Number</param>
        /// <returns>Bridge number of the card in parameters</returns>
        public static int GetBridgeNumber(Card c)
        {
            if (c == Card.Black_Joker || c == Card.Red_Joker)
            {
                return 53;
            }
            else
            {
                return (int)c + 1;
            }
        }

        /// <summary>
        /// You look at the last card and evaluate its number according to the order of the Bridge: trefoil-card-heart-spade and in each suit ace, 2, 3, 4, 5, 6, 7, 8, 9, 10, jack, queen and king
        /// (the ace of clubs has the number 1, the king of spades has the number 52). 
        /// The jokers have by convention the number 53. If the number of the last card is n you take the first n cards from the top of the deck and place them on the table from the top of the deck and place them behind the other cards except for the last card which remains the last
        /// </summary>
        public void SingleCuttingLastCard()
        {
            Card c = GetCard(53);
            int bridgeNumber = GetBridgeNumber(c);

            // first part
            LinkedList<Card> firstPart = new();
            for (int i = 0; i < bridgeNumber; i++)
            {
                firstPart.AddLast(GetCard(i));
            }

            // second part
            LinkedList<Card> secondPart = new();
            for (int i = bridgeNumber; i < 53; i++)
            {
                secondPart.AddLast(GetCard(i));
            }

            // third part
            LinkedList<Card> thirdPart = new();
            thirdPart.AddLast(GetCard(53));

            // rebuild the deck
            cards = new LinkedList<Card>(secondPart.Concat(firstPart).Concat(thirdPart));
        }

        /// <summary>
        /// Get if the bridge number of the card at position = bridge number of the first card, is 53
        /// </summary>
        /// <returns>Return True if it's not a Joker, false then</returns>
        public bool ReadingBridgeNumberNotJoker()
        {
            int bridgeNumberFirstCard = GetBridgeNumber(GetCard(0)); // = n
            // We want the card at "bridgeNumberFirstCard + 1",
            // however bridgeNumberFirstCard returns between 1 and 53 included AND our index starts at 0,
            // So we are basically doing "bridgeNumberFirstCard + 1 - 1"
            int bridgeNumberSecondCard = GetBridgeNumber(GetCard(bridgeNumberFirstCard)); // = m
            return bridgeNumberSecondCard != 53; // false if we have a Joker
        }

        /// <summary>
        ///  Get the bridge number of the card at position = bridge number of the first card, minus 26 if > 26.
        ///  The aim is to get a Letter in the alphabet
        /// </summary>
        /// <returns>A number between 1 and 26 who can be assimilated to an alphabet letter</returns>
        public int ReadingPseudoRandomLetters()
        {
            int bridgeNumberFirstCard = GetBridgeNumber(GetCard(0)); // = n
            int bridgeNumberSecondCard = GetBridgeNumber(GetCard(bridgeNumberFirstCard)); // = m 
            if (bridgeNumberSecondCard > 26)
            {
                bridgeNumberSecondCard -= 26;
            }
            return bridgeNumberSecondCard;
        }

        /// <summary>
        /// Get the deck in string like this : 
        /// 1. cardName
        /// 2. cardName
        /// 3. cardName
        /// etc.
        /// </summary>
        /// <returns>Return a string who describe the deck</returns>
        public override string ToString()
        {
            string str = "";
            int index = 1;
            foreach (Card c in cards)
            {
                str += index.ToString() + ". " + c.ToString();
                if (cards.Last?.Value != c)
                {
                    str += "\n";
                }
                index++;
            }
            return str;
        }
    }
}