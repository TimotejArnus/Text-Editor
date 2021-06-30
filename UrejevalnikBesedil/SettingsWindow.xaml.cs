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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using UrejevalnikBesedil.Classes;

namespace UrejevalnikBesedil
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {

        MainWindow mainWindow;

        Storyboard story = new Storyboard();  // ANIMACIJA  



        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            WindowSetUp();
            Animation();

        }

        public void Animation()
        {
            StringAnimationUsingKeyFrames frames = new StringAnimationUsingKeyFrames();
            frames.Duration = new Duration(new TimeSpan(0,0,12));
            frames.FillBehavior = FillBehavior.HoldEnd;
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("O", TimeSpan.FromSeconds(1)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OP", TimeSpan.FromSeconds(2)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OPE", TimeSpan.FromSeconds(3)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OPEN D", TimeSpan.FromSeconds(4)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OPEN DO", TimeSpan.FromSeconds(5)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OPEN DOC", TimeSpan.FromSeconds(5)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OPEN DOCU", TimeSpan.FromSeconds(6)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OPEN DOCUM", TimeSpan.FromSeconds(7)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OPEN DOCUME", TimeSpan.FromSeconds(8)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OPEN DOCUMEN", TimeSpan.FromSeconds(9)));
            frames.KeyFrames.Add(new DiscreteStringKeyFrame("OPEN DOCUMENT", TimeSpan.FromSeconds(10)));

            Storyboard.SetTargetName(frames, OpenDocumentLabel.Name);
            Storyboard.SetTargetProperty(frames, new PropertyPath(Label.ContentProperty));

            story.Children.Add(frames);

            story.Begin(this);
          
        }

        public void WindowSetUp()
        {
            var chosenStyles = Styles.GetChosenSettings();

            if (chosenStyles == null)   // Ce je odpiranje datoteke neuspešno se naložijo privzeti stili 
            {
                chosenStyles = new Styles().DefaultStyle();
            }

            if (chosenStyles.StartDocument == Styles.StartDocumentWith.Empty)
            {
                Empty.IsChecked = true;
            }
            else
            {
                Previous.IsChecked = true;
            }

            SavePathTextBox.Text = chosenStyles.DefaultSavePath;


            ComboBoxFont.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source); // Vsi Fonti
            ComboBoxFont.Text = chosenStyles.FontFamily;
            ComboBoxFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28 };
            ComboBoxFontSize.Text = chosenStyles.FontSize.ToString();

            // Auto Save

            ComboBoxSave.ItemsSource = new List<string>() {"Disabled", "1 min", "5 min", "10 min" };
            ComboBoxSave.Text = chosenStyles.AutoSave;

        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var styles = new Styles();
            if (Empty.IsChecked == true)
            {
                styles.StartDocument = Styles.StartDocumentWith.Empty;
            }
            else
            {
                styles.StartDocument = Styles.StartDocumentWith.Previous;
            }

            styles.DefaultSavePath = SavePathTextBox.Text;

            styles.FontFamily = ComboBoxFont.Text;
            styles.FontSize = int.Parse(ComboBoxFontSize.Text);

            // Auto SAVE

            styles.AutoSave = ComboBoxSave.Text;
            mainWindow.SetAutoSave(ComboBoxSave.Text);  // direktno spremenimo delovanje timerja 

            SaveSettings(styles);

            mainWindow.SetTempStyles(styles);   // Poenostavi pisavo na izbrano
            mainWindow.SetRichBox();

            this.Close();

        }

        public void SaveSettings(Styles styles)
        {
            var xml = new XML();
            xml.ChosenStyles = styles;
            xml.Save("styles.xml");

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SavePath_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Filter = "XML-File | *.xml";
            sDialog.ShowDialog();
            SavePathTextBox.Text = sDialog.FileName;
        }

        private void ComboBoxFontSize_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindow.TempStyles.FontSize = int.Parse(ComboBoxFontSize.Text);
        }
    }
}
