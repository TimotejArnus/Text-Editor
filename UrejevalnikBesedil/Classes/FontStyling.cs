using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using UrejevalnikBesedil.Annotations;


namespace UrejevalnikBesedil.Classes
{
    public class FontStyling // Normal, Bolt, Italic
    {           // TextSelection textSelection = RichTextBox1.Selection;
        public string name { get; set; }
        public FontStyle fontStyle { get; set; }
        public FontWeight fontWeight { get; set; }


        //public FontStyle fontStyle { get; set; }
        //public FontWeight fontWeight { get; set; }

        //public FontWeights f;
        //public FontStyles do;

        //public DependencyProperty property;

        public static ObservableCollection<FontStyling> observable = new ObservableCollection<FontStyling>();

        public FontStyling(string name, FontStyle fontStyle, FontWeight fontWeight)
        {
            this.name = name;
            this.fontStyle = fontStyle;
            this.fontWeight = fontWeight;
        }

        public FontStyling(){}

        //public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}


            //private void ListViewStyle_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            //{
            //TextSelection textSelection = RichTextBox1.Selection;

            //ListView chosenStyle = sender as ListView;

            //    if (chosenStyle.SelectedItem.ToString() == "Italic")
            //{
            //   textSelection.ApplyPropertyValue(FontStyleProperty, "Italic");
            //}
            //else if (chosenStyle.SelectedItem.ToString() == "Bold")
            //{
            //    textSelection.ApplyPropertyValue(FontWeightProperty, "Bold");
            //}
            //else if (chosenStyle.SelectedItem.ToString() == "Normal")
            //{
            //    textSelection.ApplyPropertyValue(FontWeightProperty, "Normal");
            //    textSelection.ApplyPropertyValue(FontStyleProperty, "Normal");
            //}
            //else
            //{
            //    SettingsWindow window = new SettingsWindow(this);
            //    window.Show();
            //}


            //}