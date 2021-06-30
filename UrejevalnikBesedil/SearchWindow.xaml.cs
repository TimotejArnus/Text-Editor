using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Controls;

namespace UrejevalnikBesedil
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public MainWindow window = null;
        public SearchWindow(MainWindow window)
        {
            InitializeComponent();

            this.window = window;
        }




        private void FindTextButton_OnClick(object sender, RoutedEventArgs e)
        {
            TextRange foundRange = FindTextInRange();
            window.RichTextBox1.Focus();
            foundRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.DodgerBlue));

            this.Close();
        }

        public TextRange FindTextInRange()
        {
            if (String.IsNullOrEmpty(FindTextBox.Text))
            {
                return null;
            }


            var textRange = window.GetTextFromRCB();
            int offset = textRange.Text.IndexOf(FindTextBox.Text, StringComparison.OrdinalIgnoreCase);

            if (offset < 0)
            {
                return null;
            }

            var start = GetTextPositionAtOffset(textRange.Start, offset);
            TextRange result = new TextRange(start, GetTextPositionAtOffset(start, FindTextBox.Text.Length));

            return result;
        }

        TextPointer GetTextPositionAtOffset(TextPointer position, int characterCount)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    int count = position.GetTextRunLength(LogicalDirection.Forward);
                    if (characterCount <= count)
                    {
                        return position.GetPositionAtOffset(characterCount);
                    }

                    characterCount -= count;
                }

                TextPointer nextContextPosition = position.GetNextContextPosition(LogicalDirection.Forward);
                if (nextContextPosition == null)
                    return position;

                position = nextContextPosition;
            }

            return position;
        }

        //private void FindTextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
        //{


        //}
    }
}
