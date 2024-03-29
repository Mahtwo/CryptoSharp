﻿using CryptoSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptoSharpTests
{
    [TestClass()]
    public class DeckTests
    {
        private Deck deck = new();

        [TestMethod()]
        public void GetCardTest()
        {
            Assert.AreEqual(Card.Clubs_A, deck.GetCard(0));
            Assert.AreEqual(Card.Diamonds_10, deck.GetCard(22));
        }

        [TestMethod()]
        public void FindCardPositionTest()
        {
            Assert.AreEqual(22, deck.FindCardPosition(Card.Diamonds_10));
        }

        [TestMethod()]
        public void MoveCardTest()
        {
            // Card at position 0
            Card c = deck.GetCard(0);
            deck.MoveCard(c, -1);
            Assert.AreEqual(c, deck.GetCard(52));

            // Card at position 22
            deck = new();
            c = deck.GetCard(22);
            deck.MoveCard(c, 1);
            Assert.AreEqual(c, deck.GetCard(23));

            // Card at position 53
            deck = new();
            c = deck.GetCard(53);
            deck.MoveCard(c, 1);
            Assert.AreEqual(c, deck.GetCard(1));

            // Move card 10 position down
            deck = new();
            c = deck.GetCard(5);
            deck.MoveCard(c, -10);
            Assert.AreEqual(c, deck.GetCard(48)); // pos 5 4 3 2 1 0 52 51 50 49 48 --> Move 10
        }

        [TestMethod()]
        public void DoubleCuttingTest()
        {
            deck.DoubleCutting();
            Assert.AreEqual(Card.Black_Joker, deck.GetCard(0));
            Assert.AreEqual(Card.Red_Joker, deck.GetCard(1));
            Assert.AreEqual(Card.Clubs_A, deck.GetCard(2));
            Assert.AreEqual(Card.Spades_K, deck.GetCard(53));

            // move Joker to have posJoker1 > posJoker2
            deck = new();
            deck.MoveCard(Card.Red_Joker, 1);
            deck.DoubleCutting();
            Assert.AreEqual(Card.Red_Joker, deck.GetCard(0));

            // test to have third part in doubleCutting
            deck = new();
            deck.MoveCard(Card.Red_Joker, 1);
            deck.MoveCard(Card.Black_Joker, 1);
            deck.DoubleCutting();
            Assert.AreEqual(Card.Clubs_2, deck.GetCard(0));
        }

        [TestMethod()]
        public void GetBridgeNumberTest()
        {
            Assert.AreEqual(12, Deck.GetBridgeNumber(Card.Clubs_Q));
            Assert.AreEqual(53, Deck.GetBridgeNumber(Card.Black_Joker)); // Joker is 53
            Assert.AreEqual(53, Deck.GetBridgeNumber(Card.Red_Joker)); // Joker is 53

        }

        [TestMethod()]
        public void SingleCuttingLastCardTest()
        {
            // Test without Shuffle

            Card c = deck.GetCard(0);
            deck.MoveCard(c, -1);
            c = deck.GetCard(53);
            deck.MoveCard(c, 1);
            deck.SingleCuttingLastCard();

            Assert.AreEqual(Card.Clubs_2, deck.GetCard(52));
            Assert.AreEqual(Card.Red_Joker, deck.GetCard(0));

            // Test with Shuffle

            deck = new();
            deck.Shuffle();
            Card cShuffle = deck.GetCard(53);
            int nbBridge = Deck.GetBridgeNumber(cShuffle);
            Card firstCard = deck.GetCard(0);
            deck.SingleCuttingLastCard();

            Assert.AreEqual(firstCard, deck.GetCard(53 - nbBridge));
        }

        [TestMethod()]
        public void DeckTest()
        {
            LinkedList<Card> cardsList = new();
            cardsList.AddFirst(Card.Clubs_A);
            cardsList.AddLast(Card.Black_Joker);
            Deck deck = new(cardsList);
            Assert.AreEqual(Card.Clubs_A, deck.GetCard(0));
            Assert.AreEqual(Card.Black_Joker, deck.GetCard(1));
        }

        [TestMethod()]
        public void ToStringTest()
        {
            LinkedList<Card> cardsList = new();
            cardsList.AddFirst(Card.Clubs_A);
            cardsList.AddLast(Card.Black_Joker);
            Deck deck = new(cardsList);
            string resultString = deck.ToString();
            Assert.AreEqual("1. Clubs_A\n2. Black_Joker", resultString);
        }

        [TestMethod()]
        public void ReadingBridgeNumberNotJokerTest()
        {
            Assert.AreEqual(true, deck.ReadingBridgeNumberNotJoker());
            // test false if Joker
            // have clubs_A in first position and Joker in second
            deck.MoveCard(Card.Red_Joker, 1);
            Assert.AreEqual(false, deck.ReadingBridgeNumberNotJoker());
        }

        [TestMethod()]
        public void ReadingPseudoRandomLettersTest()
        {
            Assert.AreEqual(deck.ReadingPseudoRandomLetters(), 2);
            deck.MoveCard(Card.Spades_10, 6); // Move Spades_10 to pos 1
            deck.MoveCard(Card.Clubs_A, -1); //  Move Clubs_A to have Spades_10 at pos 0

            // At the position 49 (bridge value of Spades_10), we have Spades_Q and his bridge value is 51
            Assert.AreEqual(deck.GetCard(49), Card.Spades_Q);
            Assert.AreEqual(deck.ReadingPseudoRandomLetters(), 25); // 25 = 51 - 26 (letters of alphabet)
        }
    }
}