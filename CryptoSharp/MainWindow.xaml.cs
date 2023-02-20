using System.Windows;
using System.Collections.Generic;
using System;
using System.Windows.Controls;

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
            Console.WriteLine("Decoded Message : " + decodedMessage);


            for(int i = 0; i < 54; i++)
            {
                cardsList.Items.Add(i);
            }
        }

        private void CardsList_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (cardsList.SelectedItem == null)
            {
                return;
            }
            DragDrop.DoDragDrop(cardsList, cardsList.SelectedItem, DragDropEffects.Move);
            Console.WriteLine(cardsList.SelectedItem + " is selected");
        }

        private void CardsList_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            Console.WriteLine(cardsList.SelectedItem + " is dragOver");
        }

        private void CardsList_Drop(object sender, DragEventArgs e)
        {
            Console.WriteLine(cardsList.SelectedItem + " is drop");
            //Point point = cardsList.PointToScreen(new Point(e.GetPosition(cardsList).X, e.GetPosition(cardsList).Y));
            //int index = cardsList.IndexFromPoint(point);
            //if (index < 0) index = cardsList.Items.Count - 1;
            //object data = e.Data.GetData(typeof(DateTime));
            //cardsList.Items.Remove(data);
            //cardsList.Items.Insert(index, data);
        }
    }
}
