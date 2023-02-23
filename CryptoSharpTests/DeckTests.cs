using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}