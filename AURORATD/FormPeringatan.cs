using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AURORATD
{
    public partial class FormPeringatan : Form
    {
        public Form1 _Form1;
        public string alert;
        public double temp = Form1.temp;
        public string lati = Form1.lati;
        public string longi = Form1.longi;
        public string suhu = Form1.suhu;
        public string ppmCO = Form1.ppmCO;
        public string nISPU = Form1.nISPU;
        public string hp = Form1.hp;
        public string tanggal = Form1.tanggal;
        public string lokasi = Form1.lokasi;
        public string waktu = Form1.waktu;
        public string rh = Form1.rh;
        public string status = Form1.status;


        public FormPeringatan(Form form1)
        {
            InitializeComponent();
           


        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            alert = "pku";
            send_data(alert, tanggal, waktu, lokasi, FormSettings.penerima, hp,
                           lati, longi, suhu, rh, ppmCO, nISPU, status);
            //pku
        }

        private void button2_Click(object sender, EventArgs e)
        {
            alert = "pkh";
            send_data(alert, tanggal, waktu, lokasi, FormSettings.penerima, hp,
                           lati, longi, suhu, rh, ppmCO, nISPU, status);
            //pkh
        }

        private void button3_Click(object sender, EventArgs e)
        {
            alert = "ppkh";
            send_data(alert, tanggal, waktu, lokasi, FormSettings.penerima, hp,
                           lati, longi, suhu, rh, ppmCO, nISPU, status);
            //ppkh
        }

        public void send_data(string alert, string tanggal, string jam, string lokasi, string penerima, string hp, string lat, string lon, string temp, string hum, string kCO, string ISPU, string kategori)
        {
            Data_json data = new Data_json
            {

                alert = alert,
                tanggal = tanggal,
                jam = jam,
                lokasi = lokasi,
                penerima = penerima,
                hpPenerima = hp,
                lat = lat,
                lon = lon,
                suhu = temp,
                hum = hum,
                kCO = kCO,
                ISPU = ISPU,
                kategori = kategori

            };

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = JsonConvert.SerializeObject(data);
        }

        private void FormPeringatan_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Form1.parentX + 414, Form1.parentY + 245);
        }
    }
}
