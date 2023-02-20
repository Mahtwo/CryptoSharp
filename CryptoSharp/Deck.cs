using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoSharp
{
    public class Deck
    {

        private LinkedList<Card> cards = new(); // 52 cards + 2 jokers

        public Deck()
        {
            foreach (Card c in Enum.GetValues(typeof(Card)))
            {
                cards.AddLast(c);
            }
        }
        
        public Card GetCard(int index)
        {
            return cards.ElementAt(index);
        }

        public Card GetRandomCard()
        {
            Random rnd = new();
            int index = rnd.Next(0, 54);
            return cards.ElementAt(index);
        }

        public void Shuffle()
        {
            // shuffle the deck
            Random rnd = new();
            cards = new LinkedList<Card>(cards.OrderBy(x => rnd.Next()));
        }

        public int FindCardPosition(Card c)
        {
            for (int i = 0; i < 54; i++)
            {
                if (cards.ElementAt(i) == c)
                {
                    return i;
                }
            }
            return -1;
        }

        public void MoveCard(Card c, int direction) // direction -1 is down | direction +1 is up
        {
            int pos = FindCardPosition(c);
            int newPos = (((pos + direction) % 54) + 54) % 54; // canonical modlulus (work with negative numbers, not native in C#)
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

        public void DoubleCutting()
        {
            // find jokers
            int posJoker1 = FindCardPosition(Card.Black_Joker);
            int posJoker2 = FindCardPosition(Card.Red_Joker);
            if (posJoker1 > posJoker2)
            {
                // swap jokers
                int tmp = posJoker1;
                posJoker1 = posJoker2;
                posJoker2 = tmp;
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

        public bool ReadingBridgeNumberNotJoker()
        {
            int bridgeNumberFirstCard = GetBridgeNumber(GetCard(0)); // = n
            // TODO : DONT KNOW IF +1 OR NOT IN 'bridgeNumberSecondCard'
            int bridgeNumberSecondCard = GetBridgeNumber(GetCard(bridgeNumberFirstCard)); // = m 
            return bridgeNumberSecondCard != 53; // false if we have a Joker
        }

        public int ReadingPseudoRandomLetters()
        {
            int bridgeNumberFirstCard = GetBridgeNumber(GetCard(0)); // = n
            int bridgeNumberSecondCard = GetBridgeNumber(GetCard(bridgeNumberFirstCard)); // = m 
            if(bridgeNumberSecondCard > 26)
            {
                bridgeNumberSecondCard -= 26;
            }
            return bridgeNumberSecondCard;
        }

        public override string ToString()
        {
            string str = "";
            int index = 1;
            foreach(Card c in cards)
            {
                str += index.ToString() + ". " + c.ToString() + "\n";
                index++;
            }
            return str;
        }
    }
}
