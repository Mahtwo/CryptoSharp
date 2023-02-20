using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptoSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSharp.Tests
{
    [TestClass()]
    public class EncodeDecodeTests
    {
        //[TestMethod()]
        //public void EncodeMessageTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void DecodeMessageTest()
        //{
        //    Assert.Fail();
        //}

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
    }
}