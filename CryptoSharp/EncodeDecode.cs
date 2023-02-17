using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSharp
{
    internal class EncodeDecode
    {
        public static string EncodeMessage(int[] message, int[] key)
        {
            // TODO : NEXT TIME
            return null;
        }

        public static string DecodeMessage(int[] message, int[] key)
        {
            // TODO : NEXT TIME
            return null;
        }
        
        public static int AlphabetToInt(char c)
        {
            return char.ToUpper(c) - 64;
        }

        public static char IntToAlphabet(int i)
        {
            return (char)(i + 64);
        }
    }
}
