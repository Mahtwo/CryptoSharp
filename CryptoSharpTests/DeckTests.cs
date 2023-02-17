using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoSharp.Tests
{
    [TestClass()]
    public class DeckTests
    {
        private Deck deck = new();

        [TestMethod()]
        public void GetCardTest()
        {
            Assert.AreEqual(deck.GetCard(0), Card.Clubs_A);
            Assert.AreEqual(deck.GetCard(22), Card.Diamonds_10);
        }

        [TestMethod()]
        public void FindCardPositionTest()
        {
            Assert.AreEqual(deck.FindCardPosition(Card.Diamonds_10), 22);
        }

        [TestMethod()]
        public void MoveCardTest()
        {
            // Card at position 0
            Deck localDeck = new();
            Card c = localDeck.GetCard(0);
            localDeck.MoveCard(c, -1);
            Assert.AreEqual(localDeck.GetCard(52), c);

            // Card at position 22
            localDeck = new();
            c = localDeck.GetCard(22);
            localDeck.MoveCard(c, 1);
            Assert.AreEqual(localDeck.GetCard(23), c);

            // Card at position 53
            localDeck = new();
            c = localDeck.GetCard(53);
            localDeck.MoveCard(c, 1);
            Assert.AreEqual(localDeck.GetCard(1), c);

            // Move card 10 position down
            localDeck = new();
            c = localDeck.GetCard(5);
            localDeck.MoveCard(c, -10);
            Assert.AreEqual(localDeck.GetCard(48), c); // pos 5 4 3 2 1 0 52 51 50 49 48 --> Move 10
        }
    }
}