using CryptoSharp;
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
            Card c = deck.GetCard(0);
            deck.MoveCard(c, -1);
            Assert.AreEqual(deck.GetCard(52), c);

            // Card at position 22
            deck = new();
            c = deck.GetCard(22);
            deck.MoveCard(c, 1);
            Assert.AreEqual(deck.GetCard(23), c);

            // Card at position 53
            deck = new();
            c = deck.GetCard(53);
            deck.MoveCard(c, 1);
            Assert.AreEqual(deck.GetCard(1), c);

            // Move card 10 position down
            deck = new();
            c = deck.GetCard(5);
            deck.MoveCard(c, -10);
            Assert.AreEqual(deck.GetCard(48), c); // pos 5 4 3 2 1 0 52 51 50 49 48 --> Move 10
        }

        [TestMethod()]
        public void DoubleCuttingTest()
        {
            deck.DoubleCutting();
            Assert.AreEqual(deck.GetCard(0), Card.Black_Joker);
            Assert.AreEqual(deck.GetCard(1), Card.Red_Joker);
            Assert.AreEqual(deck.GetCard(2), Card.Clubs_A);
            Assert.AreEqual(deck.GetCard(53), Card.Spades_K);

            // move Joker to have posJoker1 > posJoker2
            deck = new();
            deck.MoveCard(Card.Red_Joker, 1);
            deck.DoubleCutting();
            Assert.AreEqual(deck.GetCard(0), Card.Red_Joker);

            // test to have third part in doubleCutting
            deck = new();
            deck.MoveCard(Card.Red_Joker, 1);
            deck.MoveCard(Card.Black_Joker, 1);
            deck.DoubleCutting();
            Assert.AreEqual(deck.GetCard(0), Card.Clubs_2);
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

            Card c = deck.GetCard(0);
            deck.MoveCard(c, -1);
            c = deck.GetCard(53);
            deck.MoveCard(c, 1);
            deck.SingleCuttingLastCard();

            Assert.AreEqual(deck.GetCard(52), Card.Clubs_2);
            Assert.AreEqual(deck.GetCard(0), Card.Red_Joker);

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
            Assert.AreEqual(deck.GetCard(0), Card.Clubs_A);
            Assert.AreEqual(deck.GetCard(1), Card.Black_Joker);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            LinkedList<Card> cardsList = new();
            cardsList.AddFirst(Card.Clubs_A);
            cardsList.AddLast(Card.Black_Joker);
            Deck deck = new(cardsList);
            string resultString = deck.ToString();
            Assert.AreEqual(resultString, "1. Clubs_A\n2. Black_Joker");
        }

        [TestMethod()]
        public void ReadingBridgeNumberNotJokerTest()
        {
            Assert.AreEqual(deck.ReadingBridgeNumberNotJoker(), true);
            // test false if Joker
            // have clubs_A in first position and Joker in second
            deck.MoveCard(Card.Red_Joker, 1);
            Assert.AreEqual(deck.ReadingBridgeNumberNotJoker(), false);
        }

        [TestMethod()]
        public void ReadingPseudoRandomLettersTest()
        {
            Assert.AreEqual(deck.ReadingPseudoRandomLetters(), 2);
            deck.MoveCard(Card.Spades_10, 6); // Spades_10 have value 48 (49 bridge) --> move to pos 1

            deck.MoveCard(Card.Clubs_A, -1); // Spades_10 have value 48 (49 bridge)--> move to pos 0
            // We have Spades_K and his value is 51 (52 bridge)
            Assert.AreEqual(deck.ReadingPseudoRandomLetters(), 26); // 26 = 52 - 26 (letters of alphabet)
            // TODO : PROBLEM HERE
        }
    }
}