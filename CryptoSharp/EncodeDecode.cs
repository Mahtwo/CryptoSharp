namespace CryptoSharp
{
    public class EncodeDecode
    {
        public static string EncodeMessage(string message, int[] key)
        {
            int[] letters = new int[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                letters[i] = EncodeDecode.AlphabetToInt(message[i]);
            }

            string encodedMessage = "";
            for (int i = 0; i < letters.Length; i++)
            {
                int letterValue = letters[i] + key[i];
                if (letterValue > 26)
                {
                    letterValue -= 26;
                }
                encodedMessage += IntToAlphabet(letterValue);
            }
            return encodedMessage;
        }

        public static string DecodeMessage(string message, int[] key)
        {
            int[] letters = new int[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                letters[i] = EncodeDecode.AlphabetToInt(message[i]);
            }

            string decodedMessage = "";
            for (int i = 0; i < letters.Length; i++)
            {
                int letterValue = letters[i] - key[i];
                if (letterValue < 1)
                {
                    letterValue += 26;
                }
                decodedMessage += IntToAlphabet(letterValue);
            }
            return decodedMessage;
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
