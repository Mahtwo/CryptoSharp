using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
            // 0. INITIALIZE WINDOW 
            InitializeComponent();
            cardsList.AddHandler(ListBox.MouseDownEvent, new System.Windows.Input.MouseButtonEventHandler(CardsList_MouseDown), true);
            // Add cards to cardsList
            foreach (Card c in Enum.GetValues(typeof(Card)))
            {
                string cardSpace = c.ToString().Replace('_', ' ');
                cardsList.Items.Add(cardSpace);
            }

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
        }

        private void CardsList_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
    }
}
