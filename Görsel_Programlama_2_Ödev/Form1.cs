using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Görsel_Programlama_2_Ödev
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (File.Exists(temp))
            {
                string jsondata = File.ReadAllText(temp);
                Cars = JsonSerializer.Deserialize<List<Car>>(jsondata);
            }
            ShowList(Cars);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public List<Car> Cars = new List<Car>()
        {
            new Car()
            {
                Plaka = "06 ZYA 1907",
                Marka = "Volkswagen",
                Model = "Golf",
                Yakıt = "Dizel",
                Renk = "Lacivert",
                Vites = "Otomatik",
                Kasa_Tipi = "Hatchback 5 kapı",
                Yıl = "2014",
            }
        };

        public void ShowList (List<Car> list)
        {
            listView1.Items.Clear();
            foreach (Car car in Cars)
            {
              ListeyeAracEkleme (car);
            }
        }

        public void ListeyeAracEkleme(Car car)
        {
            ListViewItem item = new ListViewItem(new string[]
            {
                    car.Plaka,
                    car.Marka,
                    car.Model,
                    car.Yakıt,
                    car.Renk,
                    car.Vites,
                    car.Kasa_Tipi,
                    car.Yıl,
            });
            item.Tag = car;
            listView1.Items.Add(item);
        }

        void ListeAracGüncelleme(ListViewItem aItem, Car car)
        {
            aItem.SubItems[0].Text = car.Plaka;
            aItem.SubItems[1].Text = car.Marka;
            aItem.SubItems[2].Text = car.Model;
            aItem.SubItems[3].Text = car.Yakıt;
            aItem.SubItems[4].Text = car.Renk;
            aItem.SubItems[5].Text = car.Vites;
            aItem.SubItems[6].Text = car.Kasa_Tipi;
            aItem.SubItems[7].Text = car.Yıl;

            aItem.Tag = car;
        }

        private void AddCommand(object sender, EventArgs e)
        {
            AraçForm ac = new AraçForm() { Text = "Araç Ekle", StartPosition = FormStartPosition.CenterParent, Car = new Car()};

            if(ac.ShowDialog() == DialogResult.OK)
            {
                Cars.Add(ac.Car);
                ListeyeAracEkleme(ac.Car);
               
            }

        }

        private void EditCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem aItem = listView1.SelectedItems[0];
            Car Secili = aItem.Tag as Car;
            

            AraçForm ac = new AraçForm() 
            {
                Text = "Araç Düzenle",
                StartPosition = FormStartPosition.CenterParent,
                Car = Clone(Secili),
            };

            if (ac.ShowDialog() == DialogResult.OK)
            {
               Secili = ac.Car;
                ListeAracGüncelleme(aItem, Secili);
            }
        }

        Car Clone(Car car)
        {
            return new Car()
            {
                Plaka = car.Plaka,
                Marka = car.Marka,
                Model = car.Model,
                Yakıt = car.Yakıt,
                Renk = car.Renk,
                Vites = car.Vites,
                Kasa_Tipi = car.Kasa_Tipi,
                Yıl = car.Yıl,
            };

        }

        private void DeleteCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem aItem = listView1.SelectedItems[0];
            Car Secili = aItem.Tag as Car;

            var sonuc = MessageBox.Show($"Araç Bilgileri Silinsin mi?\n{Secili.Plaka} \n {Secili.Marka}",
               "Araç Bilgilerini Silmek İstiyor Musunuz?",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
              

            if (sonuc == DialogResult.Yes)
            {
                Cars.Remove( Secili );
                listView1.Items.Remove( aItem );
            }

        }

        private void SaveCommand(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog()
            {
                Filter = "Json Formatı|*.json|xml Formatı|*.xml",

            };

            if (sf.ShowDialog() == DialogResult.OK)
            {
                if(sf.FileName.EndsWith("json"))
                {
                    string data = JsonSerializer.Serialize(Cars);
                    File.WriteAllText(sf.FileName, data );

                }
                else if (sf.FileName.EndsWith("xml")) 
                {
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Car>));
                    serializer.Serialize(sw, Cars);
                    sw.Close();
                }
            }

        }

        private void LoadCommand(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog()
            {
                Filter = "Json, XML Formatları|*.json;*xml",
            };

            if(of.ShowDialog() == DialogResult.OK)
            {
                if (of.FileName.ToLower().EndsWith("json"))
                {
                    string jsondata = File.ReadAllText(of.FileName);
                    Cars = JsonSerializer.Deserialize<List<Car>>(jsondata);
                }
                else if (of.FileName.ToLower().EndsWith("xml"))
                {
                  StreamReader sr = new StreamReader(of.FileName);
                  XmlSerializer serializer = new XmlSerializer(typeof(List<Car>));
                  Cars = (List<Car>)  serializer.Deserialize(sr);
                  sr.Close();

                }

                ShowList(Cars);
            }
        }

        string temp = Path.Combine(Application.CommonAppDataPath, "data");

        protected override void OnClosing(CancelEventArgs e)
        {
            string data = JsonSerializer.Serialize(Cars);
            File.WriteAllText(temp, data);
            base.OnClosing(e);
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class Car
    {
        public string Plaka { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Yakıt { get; set; }
        public string Renk { get; set; }
        public string Vites { get; set; }
        public string Kasa_Tipi { get; set; }
        public string Yıl { get; set; }


    }

   
}
