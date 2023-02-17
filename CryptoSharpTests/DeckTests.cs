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

        [TestMethod()]
        public void DoubleCuttingTest()
        {
            Deck localDeck = new();
            localDeck.DoubleCutting();
            Assert.AreEqual(localDeck.GetCard(0), Card.Black_Joker);
            Assert.AreEqual(localDeck.GetCard(1), Card.Red_Joker);
            Assert.AreEqual(localDeck.GetCard(2), Card.Clubs_A);
            Assert.AreEqual(localDeck.GetCard(53), Card.Spades_K);
        }

        [TestMethod()]
        public void GetBridgeNumberTest()
        {
            Assert.AreEqual(Deck.GetBridgeNumber(Card.Clubs_Q), 12);
            Assert.AreEqual(Deck.GetBridgeNumber(Card.Black_Joker), 53); // Joker is 53
            Assert.AreEqual(Deck.GetBridgeNumber(Card.Red_Joker), 53); // Joker is 53

        }

        [TestMethod()]
        public void SingleCuttingLastCardTest()
        {
            // Test without Shuffle

            Deck localDeck = new();
            Card c = localDeck.GetCard(0);
            localDeck.MoveCard(c, -1);
            c = localDeck.GetCard(53);
            localDeck.MoveCard(c, 1);
            localDeck.SingleCuttingLastCard();
            
            Assert.AreEqual(localDeck.GetCard(52), Card.Clubs_2);
            Assert.AreEqual(localDeck.GetCard(0), Card.Red_Joker);

            // Test with Shuffle
            
            Deck localDeckShuffle = new();
            localDeckShuffle.Shuffle();
            Card cShuffle = localDeckShuffle.GetCard(53);
            int nbBridge = Deck.GetBridgeNumber(cShuffle);
            Card firstCard = localDeckShuffle.GetCard(0);
            localDeckShuffle.SingleCuttingLastCard();
            
            Assert.AreEqual(firstCard, localDeckShuffle.GetCard(53 - nbBridge));
        }
    }
}