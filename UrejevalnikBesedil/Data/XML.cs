using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace UrejevalnikBesedil.Classes
{
    public class XML
    {
        public Styles ChosenStyles { get; set; }    // Uporabno za shranjevanje privzetaga stila v posamezno mapo ali skupno mapo v shranjen text

        public string text { get; set; }    // text iz Rich boxa todo: shani še stile!

        public string LastSavedPath { get; set; }   // Ce je uporabnik izbral možnost za opiranje prejšnjega projekta.. Tukaj se hrani pot do datoteke, ki je bila zadnja shranjena.

       


        public void Save(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.OpenOrCreate))      //  Shranjevanje na privzeto lokacijo
            {
                var XML = new XmlSerializer(typeof(XML));

                XML.Serialize(stream, this);
            }
        }
        public void Save(string filename, string path)  // TESt ce je path "" ali se shrani v mapo projekta 
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(XML));
                TextWriter writer = new StreamWriter(path);

                XML.Serialize(writer, this);
            }
        }

        public XML Load(string fileName) // odpri privzeto lokacijo
        {
            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    var XML = new XmlSerializer(typeof(XML));
                    return (XML)XML.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Napaka pri pridobivanju podatkov ! \n" + e.Message);
                return null;
            }

        }

    }
}

