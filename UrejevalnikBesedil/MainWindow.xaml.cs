using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing.Text;
using System.Drawing.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Microsoft.Win32;
using UrejevalnikBesedil.Classes;
using UrejevalnikBesedil.Control;
using Style = System.Windows.Style;
using TextRange = System.Windows.Documents.TextRange;


namespace UrejevalnikBesedil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>s
    public partial class MainWindow : Window
    {
        
       
        public Styles TempStyles = null;  // Začasni styli

        public bool test { get; set; }
        public Help h = new Help();

        public DispatcherTimer dt = new DispatcherTimer(); // TIMER !

        public MainWindow()
        {
            InitializeComponent();
            SetTempStyles(Styles.GetChosenSettings());
            UserControl1.window = this;
            SetRichBox(); // Nastavitve za RichBox;
            SetListView();

            if (TempStyles != null && TempStyles.StartDocument == Styles.StartDocumentWith.Empty)   // Ce ni izbrana opcija za odpiranje prejse strani.
            {
                RichTextBox1.AppendText(
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            }
            else
            {
                var XML = new XML();
                var path = XML.Load("lastSave.xml").LastSavedPath;

                var xml = new XML();
                var data = xml.Load(path);
                RichTextBox1.AppendText(data.text);
            }

            

        }

        public void SetAutoSave(string settings)        // Chosen settings for AutoSave ( disabled, 1 min , 5 min  )
        {
            int time = 0;
            switch (settings)
            {
                case "Disabled":  
                    dt.Stop();
                    break;
                case "1 min":
                    SetTimer(1);
                    break;
                case "5 min":
                    SetTimer(5);
                    break;
                case "10 min":
                    SetTimer(10);
                    break;
            }

            
            
        }

        public void SetTimer(int time)
        {
            dt.Interval = TimeSpan.FromSeconds(time * 60);
            dt.Tick += Save_Click;
            dt.Start();
        }

        public void SetTempStyles(UrejevalnikBesedil.Classes.Styles styles)
        {
            if (styles == null)
            {
                styles = new Styles().DefaultStyle();
            }

            SetAutoSave(styles.AutoSave);

            TempStyles = styles;
        }

        public void SetRichBox()
        {
            if (TempStyles != null)
            {
                FontFamily fontfamily = new FontFamily(null, TempStyles.FontFamily);
                RichTextBox1.Document.FontFamily = fontfamily;

                RichTextBox1.FontSize = TempStyles.FontSize;


                if (TempStyles.Color == null)
                {
                    RichTextBox1.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    RichTextBox1.Foreground = TempStyles.Color;
                }
            }
         


            
        }
         
        public void SetListView()
        {
            FontStyling.observable.Add(new FontStyling("Normal", FontStyles.Normal, FontWeights.Normal));
            FontStyling.observable.Add(new FontStyling("Italic", FontStyles.Italic, FontWeights.Normal));
            FontStyling.observable.Add(new FontStyling("Bold", FontStyles.Normal, FontWeights.Bold));
            //FontStyling.observable.Add(new FontStyling("Bold&Italic", System.Drawing.FontStyle.Bold));


            ListViewStyle.ItemsSource = FontStyling.observable;

        }

        public TextRange GetTextFromRCB()
        {            
            return new TextRange(RichTextBox1.Document.ContentStart, RichTextBox1.Document.ContentEnd);   // prebere textbox od začetka do konca                 
        }

        private void Edit_Click(object sender, RoutedEventArgs e)   //Menu
        {
            
        }

        private void Tools_Click(object sender, RoutedEventArgs e)   //Menu
        {

        }

        private void Open_Click(object sender, RoutedEventArgs e) // DATOTEKA MenuItem
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML-File | *.xml";
            dialog.ShowDialog();

            var xml = new XML();
            var data = xml.Load(dialog.FileName);
            RichTextBox1.Document.Blocks.Clear();
            RichTextBox1.AppendText(data.text);
            TempStyles = data.ChosenStyles;
            SetRichBox();
        }

        private void Save_Click(object sender, EventArgs e) // DATOTEKA MenuItem
        {

            var xml = new XML();
            xml.text = GetTextFromRCB().Text;
            string path = Styles.GetChosenSettings().DefaultSavePath;

            if (String.IsNullOrEmpty(path))
            {
                xml.Save("file.xml");
            }
            else
            {
                xml.Save(path + @"\" + "file.xml");   // Shranjevanje na privzeto pot, ki si jo je uporabnik izbral v nastavitvah 
            }

            SavePathForLastSave(path + @"\" + "file.xml");
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e) // TODO: Belezenje poti shranjevanja shranjevanja 
        {
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Filter = "XML-File | *.xml";
            sDialog.ShowDialog();

            var xml = new XML();
            xml.text = GetTextFromRCB().Text;
            xml.ChosenStyles = TempStyles;
            xml.Save(sDialog.FileName);

            SavePathForLastSave(sDialog.FileName);
        }

        private void SavePathForLastSave(string path)  // Shranjevanje poti zagnjega shranjevanja 
        {
            var xml = new XML();
            xml.LastSavedPath = path;
            xml.Save("lastSave.xml");
        }

        private void Exit_Click(object sender, RoutedEventArgs e) // DATOTEKA MenuItem
        {
            this.Close();
        }

        private void RichTextBox1_OnGotFocus(object sender, EventArgs e)
        {
            CanvasAnimation.Children.Clear();
            StatusBarText.Text = " » Entering ...« ";
        }

        private void RichTextBox1_OnLostFocus(object sender, RoutedEventArgs e)
        {
            
            TextRange range = GetTextFromRCB();
            range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.WhiteSmoke));
           
            StatusBarText.Text =
                "Characters: " +
                GetTextFromRCB().Text.Length
                    .ToString(); // Ko RichBoxIzgubi focus lahko spraznimo statusno okno in vpisemo stevilo characterjev
        }

        private void Color_OnClick(object sender, RoutedEventArgs e)
        {

            Random random = new Random();   // random color generator
            SolidColorBrush brush =
                new SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(
                        (byte)random.Next(255),
                        (byte)random.Next(255),
                        (byte)random.Next(255)
                    ));

            RichTextBox1.Foreground = brush;
            h.textColor = brush;

        }

        private void Copy_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GetTextFromRCB().Text);
        }

        private void Font_OnClick(object sender, RoutedEventArgs e)
        {
            FontStyleWindow window = new FontStyleWindow(this);
            window.Show();
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow(this);
            window.Show();
        }

        private void ListViewStyle_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextSelection textSelection = RichTextBox1.Selection;

            FontStyling chosenStyle = ListViewStyle.SelectedItem as FontStyling;

            if (chosenStyle.name == "Italic")
            {
                textSelection.ApplyPropertyValue(FontStyleProperty, "Italic");
            }
            else if (chosenStyle.name == "Bold")
            {
                textSelection.ApplyPropertyValue(FontWeightProperty, "Bold");
            }
            else if (chosenStyle.name == "Normal")
            {
                textSelection.ApplyPropertyValue(FontWeightProperty, "Normal");
                textSelection.ApplyPropertyValue(FontStyleProperty, "Normal");
            }
            else
            {
                SettingsWindow window = new SettingsWindow(this);
                window.Show();
            }
            
            
        }


        private void Paste_OnClick(object sender, RoutedEventArgs e)
        {
            RichTextBox1.AppendText(Clipboard.GetText());
        }

       
        private void RichTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            SearchWindow window = new SearchWindow(this);
            window.Show();
        }

        private void ChangeView_OnClick(object sender, RoutedEventArgs e)
        {
            if (Grid.GetRow(TabControlWindow) == 1) // TO Left, Right
            {
                //Grid.SetRow(UserControlWindow,5);
                //Grid.SetColumnSpan(UserControlWindow, 1);

                Grid.SetRow(RichTextBox1, 3);
                Grid.SetColumn(RichTextBox1, 1);
                Grid.SetRowSpan(RichTextBox1,5);
                Grid.SetColumnSpan(RichTextBox1, 3);

                Grid.SetRow(TabControlWindow,3 );
                Grid.SetColumnSpan(TabControlWindow,1);
                
            }

            else
            {
                Grid.SetRow(RichTextBox1, 3);
                Grid.SetColumn(RichTextBox1, 0);
                Grid.SetRowSpan(RichTextBox1, 2);
                Grid.SetColumnSpan(RichTextBox1, 4);

                Grid.SetRow(TabControlWindow, 1);
                Grid.SetColumnSpan(TabControlWindow, 4);
            }
            
            
        }
    }
}
