using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoSharp
{
    internal class Deck
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
            cards.Remove(c);
            LinkedListNode<Card> node = cards.Find(GetCard(newPos))!;
            cards.AddBefore(node, c);

            //TODO : add before at position 0 ?
        }
    }
}
