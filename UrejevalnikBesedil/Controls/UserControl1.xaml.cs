using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UrejevalnikBesedil.Classes;

namespace UrejevalnikBesedil.Control
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public static MainWindow window;
        public UserControl1()
        {
            InitializeComponent();
            SetControl();
            this.Height = 25;
        }
        public void SetControl()
        {
            var chosenStyles = Styles.GetChosenSettings();

            FontCombobox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source); // Vsi Fonti
            FontCombobox.Text = chosenStyles.FontFamily;
            FontSizeCombobox.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28 };
            FontSizeCombobox.Text = chosenStyles.FontSize.ToString();

            if (window != null)
            {
                var RtBColor = window.RichTextBox1.Foreground as SolidColorBrush;

                var color = (Color)ColorConverter.ConvertFromString( RtBColor.Color.ToString());
                ColorPicker.SelectedColor = color;
                
            }
       

        }

        private void Font_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (window == null)
            {
                return;
            }

            window.TempStyles.FontFamily = FontCombobox.SelectedItem.ToString();
            var size = int.Parse(FontSizeCombobox.SelectedItem.ToString());

            
            //window.SetRichBox();
            window.RichTextBox1.FontSize = size;
            window.TempStyles.FontSize = size;
            window.SetRichBox();
        }

        private void BoldButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (window == null)
            {
                return;
            }

            window.RichTextBox1.Selection.ApplyPropertyValue(FontWeightProperty, "Bold");

        }

        private void ItalicButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (window == null)
            {
                return;
            }
            window.RichTextBox1.Selection.ApplyPropertyValue(FontStyleProperty, "Italic"); 
        }

        private void NormalButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (window == null)
            {
                return;
            }
            window.RichTextBox1.Selection.ApplyPropertyValue(FontStyleProperty, "Normal");
            window.RichTextBox1.Selection.ApplyPropertyValue(FontWeightProperty, "Normal");
        }

        private void ColorPicker_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (window == null)
            {
                return;
            }

            var selected = ColorPicker.SelectedColor;

            var brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(selected.Value.A, selected.Value.R, selected.Value.G, selected.Value.B);

            window.RichTextBox1.Foreground = brush;
            window.TempStyles.Color = brush;
            // TODO ..


        }

        private void Expander_OnCollapsed(object sender, RoutedEventArgs e)
        {
            this.Height = 25;
        }

        private void Expander_OnExpanded(object sender, RoutedEventArgs e)
        {
            SetControl();
            this.Height = 100;
            
        }

    }
}
