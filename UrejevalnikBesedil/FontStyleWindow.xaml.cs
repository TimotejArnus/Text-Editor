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
using System.Windows.Shapes;

namespace UrejevalnikBesedil
{
    /// <summary>
    /// Interaction logic for FontStyleWindow.xaml
    /// </summary>
    public partial class FontStyleWindow : Window
    {
        private MainWindow mainWindow;

        public FontStyleWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            
            this.mainWindow = mainWindow;
            SetComboBoxes();
        }

        private void SetComboBoxes()
        {
            ComboBoxFont.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source); // Vsi Fonti
            ComboBoxFont.Text = mainWindow.TempStyles.FontFamily;
            ComboBoxFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28 };
            ComboBoxFontSize.Text = mainWindow.TempStyles.FontSize.ToString();
        }

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            mainWindow.TempStyles.FontFamily = ComboBoxFont.Text.ToString();
            mainWindow.TempStyles.FontSize = int.Parse(ComboBoxFontSize.Text.ToString());
            mainWindow.SetRichBox();

            // Set userControl
            mainWindow.UserControlWindow.FontCombobox.Text = ComboBoxFont.Text.ToString();
            mainWindow.UserControlWindow.FontSizeCombobox.Text = ComboBoxFontSize.Text.ToString();

            this.Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
