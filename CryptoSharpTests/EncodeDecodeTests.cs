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
            Assert.AreEqual(1, EncodeDecode.AlphabetToInt(c1));
            Assert.AreEqual(26, EncodeDecode.AlphabetToInt(c2));
            Assert.AreEqual(1, EncodeDecode.AlphabetToInt(c3));
        }

        [TestMethod()]
        public void IntToAlphabetTest()
        {
            int i1 = 1;
            int i2 = 26;
            Assert.AreEqual('A', EncodeDecode.IntToAlphabet(i1));
            Assert.AreEqual('Z', EncodeDecode.IntToAlphabet(i2));
        }

        [TestMethod()]
        public void EncodeMessageTest()
        {
            string inputMessage = "ABA";
            int[] key = new int[] { 1, 2, 26 };
            Assert.AreEqual("BDA", EncodeDecode.EncodeMessage(inputMessage, key));
        }

        [TestMethod()]
        public void DecodeMessageTest()
        {
            string inputMessage = "BDA";
            int[] key = new int[] { 1, 2, 26 };
            Assert.AreEqual("ABA", EncodeDecode.DecodeMessage(inputMessage, key));
        }
    }
}