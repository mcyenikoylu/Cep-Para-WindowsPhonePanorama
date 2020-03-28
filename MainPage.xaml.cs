using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ParaBank_PanoramaApp4
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            GetXMLData();
            MetalXML();
            
            //Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        //Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void GetXMLData()
        {
            WebClient xmlClient = new WebClient();
            xmlClient.DownloadStringCompleted += OnXMLFileLoaded;
            xmlClient.DownloadStringAsync(new Uri("http://www.tcmb.gov.tr/kurlar/today.xml", UriKind.RelativeOrAbsolute));
            //xmlClient.DownloadStringAsync(new Uri("kurlar.xml", UriKind.RelativeOrAbsolute));
        }

        private void OnXMLFileLoaded(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string xml = e.Result;
                var data = from c in XElement.Parse(xml).Elements("Currency")
                           where c.Element("ForexSelling") != null
                           select new myData
                           {
                               Doviz = (string)c.Attribute("CurrencyCode"),
                               Isim = (string)c.Element("CurrencyName"),
                               Alis = (string)c.Element("ForexBuying") == "" ? 0 : (decimal)c.Element("ForexBuying"),
                               Satis = (string)c.Element("ForexSelling") == "" ? 0 : (decimal)c.Element("ForexSelling"),
                               Capraz = (string)c.Element("CrossRateUSD") == "" ? 0 : (decimal)c.Element("CrossRateUSD")
                           };
                listBox1.ItemsSource = data;
            }
        }

        //private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selected = ((ListBox)sender).SelectedItem as myData;
        //    showSelected.Text = selected.Doviz + " - " + selected.Isim;
        //}

        public class myData
        {
            public string Doviz { get; set; }
            public string Isim { get; set; }
            public decimal Alis { get; set; }
            public decimal Satis { get; set; }
            public decimal Capraz { get; set; }
        }

        private void MetalXML()
        {
            WebClient xmlClient = new WebClient();
            xmlClient.DownloadStringCompleted += MetalXMLGetir;
            xmlClient.DownloadStringAsync(new Uri("http://www.iab.gov.tr/mtl_fiyat_xml.asp", UriKind.RelativeOrAbsolute));
        }

        private void MetalXMLGetir(object sender, DownloadStringCompletedEventArgs e)
        {
            //if (e.Error == null)
            //{
            //    string xml = e.Result;
            //    var data = from c in XElement.Parse(xml).Descendants("maden")
            //        //where c.Element("altindeger") != null
            //        select new metalFiyatlari
            //        {
            //            deger = (string)c.Attribute("deger"),
            //            lira = (string)c.Element("lira") == "" ? "0" : (string)c.Element("lira"),
            //            dolar = (string)c.Element("dolar") == "" ? "0" : (string)c.Element("dolar"),
            //            euro = (string)c.Element("euro") == "" ? "0" : (string)c.Element("euro")
            //        };
            //    listBox2.ItemsSource = data;
            //}

            if (e.Error == null)
            {
                string xml = e.Result;
                var data = from c in XElement.Parse(xml).Descendants("TL")
                           //where c.Attribute("TL") != null
                           select new metalFiyatlari
                           {
                               tlAltin = (string)c.Element("altindeger") == "" ? "0" : (string)c.Element("altindeger"),
                               tlGumus = (string)c.Element("gumusdeger") == "" ? "0" : (string)c.Element("gumusdeger"),
                               tlPlatin = (string)c.Element("platindeger") == "" ? "0" : (string)c.Element("platindeger")
                           };
                listBox2.ItemsSource = data;
            }
        }

        public class metalFiyatlari
        {
            public string deger { get; set; }
            public string lira { get; set; }
            public string dolar { get; set; }
            public string euro { get; set; }

            public string tlAltin { get; set; }
            public string tlGumus { get; set; }
            public string tlPlatin { get; set; }

            public string dolarAltin { get; set; }
            public string dolarGumus { get; set; }
            public string dolarPlatin { get; set; }

            public string euroAltin { get; set; }
            public string euroGumus { get; set; }
            public string euroPlatin { get; set; }
        }

    }
}