using CryptoSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptoSharpTests
{
    [TestClass()]
    public class EncodeDecodeTests
    {
        [TestMethod()]
        public void AlphabetToIntTest()
        {
            char c1 = 'A';
            char c2 = 'Z';
            char c3 = 'a';
            Assert.AreEqual(EncodeDecode.AlphabetToInt(c1), 1);
            Assert.AreEqual(EncodeDecode.AlphabetToInt(c2), 26);
            Assert.AreEqual(EncodeDecode.AlphabetToInt(c3), 1);
        }

        [TestMethod()]
        public void IntToAlphabetTest()
        {
            int i1 = 1;
            int i2 = 26;
            Assert.AreEqual(EncodeDecode.IntToAlphabet(i1), 'A');
            Assert.AreEqual(EncodeDecode.IntToAlphabet(i2), 'Z');
        }

        [TestMethod()]
        public void EncodeMessageTest()
        {
            string inputMessage = "ABA";
            int[] key = new int[] { 1, 2, 26 };
            string expected = "BDA";
            string actual = EncodeDecode.EncodeMessage(inputMessage, key);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void DecodeMessageTest()
        {
            string inputMessage = "BDA";
            int[] key = new int[] { 1, 2, 26 };
            string expected = "ABA";
            string actual = EncodeDecode.DecodeMessage(inputMessage, key);
            Assert.AreEqual(actual, expected);
        }
    }
}