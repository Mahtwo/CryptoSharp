using System.Windows;
using System.Collections.Generic;
using System;

namespace CryptoSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // 1. CREATE MESSAGE TO ENCODE
            string message = "ILOVERACLETTE";

            // 2. CREATE DECK 
            Deck deck = new();
            //deck.Shuffle();

            // 3. FILL DICTIONNARY WITH CARD VALUES

            int[] encodeKey = new int[message.Length];

            for (int i = 0; i < message.Length; i++)
            {
                bool readingBridgeNumberNotJokerOk = false;
                while (!readingBridgeNumberNotJokerOk)
                {
                    deck.MoveCard(Card.Black_Joker, 1);
                    deck.MoveCard(Card.Red_Joker, 2);
                    deck.DoubleCutting();
                    deck.SingleCuttingLastCard();
                    readingBridgeNumberNotJokerOk = deck.ReadingBridgeNumberNotJoker();
                }
                int letterValue = deck.ReadingPseudoRandomLetters();
                encodeKey[i] = letterValue;
            }

            // 4. ENCODE MESSAGE
            string encodedMessage = EncodeDecode.EncodeMessage(message, encodeKey);

            // 5. DECODE MESSAGE
            string decodedMessage = EncodeDecode.DecodeMessage(encodedMessage, encodeKey);

            // 6. CREATE STRING KEY TO DISPLAY IT
            string key = "";
            for (int i = 0; i < message.Length; i++)
            {
                key += EncodeDecode.IntToAlphabet(encodeKey[i]);
            }

            // 7. DISPLAY - CHECK IF ALL OK
            Console.WriteLine("Original Message : " + message);
            Console.WriteLine("Key : " + key);
            Console.WriteLine("Encoded Message : " + encodedMessage);
            Console.WriteLine("DEBUG : " + (int)'B');
            
            Console.WriteLine("Decoded Message : " + decodedMessage);
        }
    }
}
