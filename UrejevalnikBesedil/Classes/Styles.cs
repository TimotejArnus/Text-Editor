using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Serialization;
using UrejevalnikBesedil.Annotations;

namespace UrejevalnikBesedil.Classes
{
    [Serializable]
    public class Styles: INotifyPropertyChanged
    {
        [XmlIgnore]
        public MainWindow window;

        public enum StartDocumentWith   // Dokument se lahko odpre prazen ali z prejšnjo vsebino, ki je bila na ztadnje shranjena 
        {
            [XmlEnum(Name = "Empty")]
            Empty,
            [XmlEnum(Name = "Previous")]
            Previous
        }

        public StartDocumentWith StartDocument { get; set; }
        public string DefaultSavePath { get; set; }

        //public MainWindow window { get; set; }
       

        public string FontFamily { get; set; }



        private int _fontSize;

        
        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        public string AutoSave { get; set; }
        
        [XmlIgnore]
        public SolidColorBrush Color;

        public Styles(StartDocumentWith startDocument, string DefaultSavePath, string FontFamily, int FontSize, SolidColorBrush Color, string AutoSave, MainWindow window)
        {
            this.StartDocument = StartDocument;
            this.DefaultSavePath = DefaultSavePath;
            this.FontFamily = FontFamily;
            this.FontSize = FontSize;
            this.Color = Color;
            this.AutoSave = AutoSave;
            this.window = window;
        }

        //public Styles(StartDocumentWith startDocument, string DefaultSavePath, string FontFamily, int FontSize, MainWindow window)
        //{
        //    this.StartDocument = StartDocument;
        //    this.DefaultSavePath = DefaultSavePath;
        //    this.FontFamily = FontFamily;
        //    this.FontSize = FontSize;
        //    this.window = window;
        //}

        public Styles(){}

        public Styles DefaultStyle()
        {
            return new Styles(StartDocumentWith.Empty, "", "Arial", 12, new SolidColorBrush(Colors.Black) , "Disabled", window);
        }

        public static Styles GetChosenSettings()
        {
            XML styles = new XML();
            var style = styles.Load("styles.xml");

            if (style != null)
            {
                return style.ChosenStyles;
            }
            else
            {
                return new Styles().DefaultStyle();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    
}
