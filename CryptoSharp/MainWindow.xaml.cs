using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection.Emit;
using System.Windows.Media;

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

        bool dropDown = false;
        int currentIndex = -1;

        private void CardsList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (cardsList.SelectedItem == null)
            {
                return;
            }
            DragDrop.DoDragDrop(cardsList, cardsList.SelectedItem, DragDropEffects.Move);
            Console.WriteLine(cardsList.SelectedItem + " is selected");

            dropDown = true;
        }
        
        private void CardsList_DragOver(object sender, DragEventArgs e)
        {

            e.Effects = DragDropEffects.Move;
            Console.WriteLine(cardsList.SelectedItem + " is dragOver");

            DependencyObject? item = VisualTreeHelper.HitTest(cardsList, Mouse.GetPosition(cardsList)).VisualHit;

            // find ListViewItem (or null)
            while (item != null && !(item is ListBoxItem))
                item = VisualTreeHelper.GetParent(item);

            if (item != null)
            {
                int i = cardsList.Items.IndexOf(((ListBoxItem)item).DataContext);
                Console.WriteLine(string.Format("I'm on item {0}", i));
                currentIndex = i;
            }
        }
        
        public void CardsList_OnMouseMove(object sender, MouseEventArgs e)
        {
            DependencyObject? item = VisualTreeHelper.HitTest(cardsList, Mouse.GetPosition(cardsList)).VisualHit;

            // find ListViewItem (or null)
            while (item != null && !(item is ListBoxItem))
                item = VisualTreeHelper.GetParent(item);

            if (item != null)
            {
                int i = cardsList.Items.IndexOf(((ListBoxItem)item).DataContext);
                Console.WriteLine(string.Format("I'm on item {0}", i));
                currentIndex = i;
            }

            if (dropDown)
            {
                var elem = cardsList.SelectedItem;
                cardsList.Items.Remove(elem);
                cardsList.Items.Insert(currentIndex, elem);
                dropDown = false;
            }
        }
    }
}

