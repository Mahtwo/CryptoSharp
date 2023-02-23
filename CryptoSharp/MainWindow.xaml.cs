using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CryptoSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Deck deck = new();

        public MainWindow()
        {
            // Initialize window
            InitializeComponent();
            cardsList.AddHandler(ListBox.MouseDownEvent, new MouseButtonEventHandler(CardsList_MouseDown), true);
            // Add cards to cardsList
            foreach (Card c in Enum.GetValues(typeof(Card)))
            {
                string cardSpace = c.ToString().Replace('_', ' ');
                cardsList.Items.Add(cardSpace);
            }
        }

        private void CardsList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (cardsList.SelectedItem != null)
            {
                DragDrop.DoDragDrop(cardsList, cardsList.SelectedItem, DragDropEffects.Move | DragDropEffects.None);
                cardsList.Items.Refresh(); // Needed to refresh the visual offset of the dragged card
            }
        }

        private void CardsList_DragEnterOver(object sender, DragEventArgs e)
        {
            e.Handled = true;

            // Allow dropping only if it is a card (string)
            if (e.Data.GetDataPresent(typeof(string))) // Data can be converted to string
            {
                string card = (string)e.Data.GetData(typeof(string));
                string cardUnderscore = card.Replace(' ', '_');
                if (Enum.IsDefined(typeof(Card), cardUnderscore)) // Data can be converted to card
                {
                    e.Effects = DragDropEffects.Move;

                    // Change the visual offset of the dragged card to the cursor position
                    ListBoxItem item = (ListBoxItem)cardsList.ItemContainerGenerator.ContainerFromItem(card);
                    Point cursorPositionWindow = e.GetPosition(cardsList);
                    item.GetType().GetProperty("VisualOffset", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(item, new Vector(cursorPositionWindow.X, cursorPositionWindow.Y)); // VisualOffset is a protected property

                    return;
                }
            }

            e.Effects = DragDropEffects.None;
        }

        private void CardsList_Drop(object sender, DragEventArgs e)
        {
            if (e.Effects != DragDropEffects.None)
            {
                string card = (string)e.Data.GetData(typeof(string));

                // Get the index of the item on the cursor, to insert the card at this index
                Point cursorPositionCardsList = e.GetPosition(cardsList);
                IInputElement itemOnCursor = cardsList.InputHitTest(cursorPositionCardsList);
                if (itemOnCursor != null)
                {
                    // Get the index of the item on the cursor
                    DependencyObject listBoxItemDO = (DependencyObject)itemOnCursor;
                    while (listBoxItemDO.GetType() != typeof(ListBoxItem))
                    {
                        listBoxItemDO = VisualTreeHelper.GetParent(listBoxItemDO);

                        // If we can't get the ListBoxItem, stop the method
                        if (listBoxItemDO == null)
                        {
                            return;
                        }
                    }
                    int indexAtCursor = cardsList.ItemContainerGenerator.IndexFromContainer(listBoxItemDO);
                    cardsList.Items.Remove(card); // Remove the dragged card from the list

                    // If cursor is in the upper half, then insert the card "on top", else insert "below"
                    ListBoxItem listBoxItem = (ListBoxItem)listBoxItemDO;
                    Point cursorPositionListBoxItem = e.GetPosition(listBoxItem); // Get cursor position relative to the item
                    double listBoxItemHeight = listBoxItem.ActualHeight;
                    if (cursorPositionListBoxItem.Y < listBoxItemHeight / 2)
                    {
                        // Inserting the card on top of the item at the cursor position
                        cardsList.Items.Insert(indexAtCursor, card);
                    }
                    else
                    {
                        // Check if we're not inserting below the last card (index out of range)
                        if (cardsList.Items.Count < indexAtCursor + 1)
                        {
                            // Inserting the card at the end of the list
                            cardsList.Items.Add(card);
                        }
                        else
                        {
                            // Inserting the card below the item at the cursor position
                            cardsList.Items.Insert(indexAtCursor + 1, card);
                        }
                    }
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "")
            {
                textBox.Foreground = Brushes.Gray;
                if (textBox.Name == "inputEncodeMessage")
                {
                    textBox.Text = "Enter your message to encode";
                }
                else if (textBox.Name == "inputDecodeMessage")
                {
                    textBox.Text = "Enter your message to decode";
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAFA"));
            }
        }

        private int[] GetKeyFromCardsList(string message)
        {
            // get all cards in ListBox
            LinkedList<Card> cardsListLinkedList = new();

            foreach (string card in cardsList.Items)
            {
                string cardUnderscore = card.Replace(' ', '_');
                cardsListLinkedList.AddLast(Enum.Parse<Card>(cardUnderscore));
            }
            deck = new Deck(cardsListLinkedList);

            // create the key
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
            return encodeKey;
        }

        private void ButtonEncodeMessage_Click(object sender, RoutedEventArgs e)
        {
            // Check if message exists
            string message = inputEncodeMessage.Text;
            if (message == "" || message == "Enter your message to encode")
            {
                return;
            }

            // Encode
            int[] encodeKey = GetKeyFromCardsList(message);
            string encodedMessage = EncodeDecode.EncodeMessage(message, encodeKey);
            inputDecodeMessage.Text = encodedMessage;
            inputDecodeMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAFA"));
            inputEncodeMessage.Foreground = Brushes.Gray;


            // Display in terminal
            string key = "";
            for (int i = 0; i < message.Length; i++)
            {
                key += EncodeDecode.IntToAlphabet(encodeKey[i]);
            }
            Console.WriteLine("----------------------");
            Console.WriteLine("You encode a message");
            Console.WriteLine("Original Message : " + message);
            Console.WriteLine("Key : " + key);
            Console.WriteLine("Encoded Message : " + encodedMessage);
        }

        private void ButtonDecodeMessage_Click(object sender, RoutedEventArgs e)
        {
            // Check if message exists
            string message = inputDecodeMessage.Text;
            if (message == "" || message == "Enter your message to decode")
            {
                return;
            }

            // Decode
            int[] encodeKey = GetKeyFromCardsList(message);
            string decodedMessage = EncodeDecode.DecodeMessage(message, encodeKey);
            inputEncodeMessage.Text = decodedMessage;
            inputEncodeMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAFA"));
            inputDecodeMessage.Foreground = Brushes.Gray;

            // Display in terminal
            string key = "";
            for (int i = 0; i < message.Length; i++)
            {
                key += EncodeDecode.IntToAlphabet(encodeKey[i]);
            }

            Console.WriteLine("----------------------");
            Console.WriteLine("You decode a message");
            Console.WriteLine("Encoded Message : " + message);
            Console.WriteLine("Key : " + key);
            Console.WriteLine("Decoded Message : " + decodedMessage);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Don't check the text if the text color is gray (placeholder)
            if (textBox.Foreground != Brushes.Gray)
            {
                // Build the new text (uppercase letters only)
                string currentText = textBox.Text;
                StringBuilder newTextBuilder = new();
                int removedCharacters = 0;
                foreach (char letter in currentText)
                {
                    if (char.IsAsciiLetter(letter))
                    {
                        newTextBuilder.Append(char.ToUpperInvariant(letter));
                    }
                    else
                    {
                        removedCharacters++;
                    }
                }
                string newText = newTextBuilder.ToString();

                // If different than current text, apply the new text
                if (currentText != newText)
                {
                    // Changing the text always call the TextChanged event,
                    // so we temporarily change the text color to gray
                    textBox.Foreground = Brushes.Gray;
                    // Backup the current caret position to re-apply it with the correct position
                    int caretPosition = textBox.CaretIndex;
                    textBox.Text = newText;
                    textBox.CaretIndex = caretPosition - removedCharacters;
                    textBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAFAFA"));
                }
            }
        }
    }
}