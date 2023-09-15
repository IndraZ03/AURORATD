using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using GMap.NET.CacheProviders;
using GMap.NET.Properties;
using GMap.NET.Internals;
using Geolocation;
using CsvHelper;
using FileHelpers;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Timers;
using System.Threading;
using System.Windows.Forms.VisualStyles;

namespace AURORATD
{
    public partial class Form1 : Form
    {
        public static double temp;
        public static string lati;
        public static string longi;
        public static string suhu;
        public static string ppmCO;
        public static string nISPU;
        public static string hp;
        public static string tanggal;
        public static string waktu;
        public static string lokasi;
        public static string kategori;
        public static string rh;

        public static bool ID1 = false;
        public static bool ID2 = false;
        Bitmap bmpPesawat;

        int nilai1, nilai2;
        double kCO, Xx, dpm25, dpm10;
        public static string status;
        int ba, bb, Ia, Ib, Xa, Xb;
        int[] ispu_ambang = { 0, 50, 100, 200, 300 };
        int[] ambien_ambang = { 4, 5, 15, 30, 45 };
        int[] pm25ambien_ambang = { 16, 55, 150, 250, 500 };
        int[] pm10ambien_ambang = { 50, 150, 350, 420, 500 };
        double ISPU;
        string alert;
        static bool bisaSend = true;
        string str_CO, pm10,pm25,cemar;
        string str_Temp;
        string str_Kelembaban;
        string str_Tekanan;
        string str_kec;
        string str_lat;
        string str_lon;
        string str_pm25;
        string str_pm10;
        bool updateData = false;
        int jam= DateTime.Now.Hour;
        int menit= DateTime.Now.Minute;
        int detik = DateTime.Now.Second;
        int CO,Temp,Kelembaban,Tekanan;
        double  lat, lon;
        GMapOverlay Overlay_Marker = new GMapOverlay("Overlay_Marker");
        GMapOverlay Overlay_Trace = new GMapOverlay("Overlay_Trace");
        GMapMarker Marker_Payload;
        List<PointLatLng> Koordinat_Trace = new List<PointLatLng>();
        GMapRoute Route_Trace;
        GMapMarker lastMarker;

        public static string svCO = "";
        public static string svTemp = "";
        public static string svKelembaban = "";
        public static string svKec = "";
        public static int parentX, parentY;
        bool btnSave = FormSave.f2btnSave;

        PointLatLng Koordinat_Awal = new PointLatLng();

        FileHelperEngine<data> engine;
        List<data> data;
        

        public Form1()
        {
            InitializeComponent();
            engine = new FileHelperEngine<data>();
            data = new List<data>();
            data.Add(new data()
            {
                csvWaktu = "waktu",
                csvLokasi = "Lokasi",
                csvCO = "Karbon Monoksida (ppm)",
                csvSuhu = "Suhu udara (C)",
                csvKelembaban = "Kelembaban (%)",
                csvpm25 = "PM2.5(µg/m3)",
                csvpm10 = "PM10(µg/m3)",
                csvISPU = "ISPU",
               csvkategori = "Kategori" 
                
            });
            timer2.Interval = 1000 *60*FormSettings.interval;
            timer2.Tick += Timer_Tick;
     
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
           bisaSend = true;
            timer2.Stop();
        }


        void stylingChart()
        {
            // <--Chart CO--> //
            chartCO1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chartCO1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartCO1.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            chartCO1.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
            chartCO1.ChartAreas[0].AxisX.LineColor = Color.LightGray;
            chartCO1.ChartAreas[0].AxisY.LineColor = Color.LightGray;
            chartCO1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("roboto", 8, FontStyle.Bold);
            chartCO1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("roboto", 8, FontStyle.Bold);



        }
        

        private void button3_Click(object sender, EventArgs e)
        {

            serialPort2.PortName = comboBox1.Text;
            serialPort2.Open();

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            map1.MapProvider = GMapProviders.GoogleMap;
            map1.MinZoom = 5;
            map1.MaxZoom = 100;
            map1.Zoom = 18;
            bmpPesawat = (Bitmap)Image.FromFile("C:/Users/Administrator/Pictures/AURORATD/AURORATD/AURORATD/AURORATD/bmpPesawat.ico");
            label21.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            timer1.Start();
          

        }

        private void button4_Click(object sender, EventArgs e)
        {
            svCO = textBox4.Text;
            svTemp= textBox6.Text;
            svKelembaban = textBox7.Text;
            // FormSave frm2 = new FormSave(this);//
            //frm2.Show();//
            //buat modal lebih kece//
            FormCover modalbackground = new FormCover();
            using (FormSave modal = new FormSave(this))
            {
                modalbackground.StartPosition = FormStartPosition.Manual;
                modalbackground.Opacity = .60d;
                modalbackground.BackColor = Color.Black;
                modalbackground.Size = this.Size;
                modalbackground.Location = this.Location;
                modalbackground.ShowInTaskbar = false;
                modalbackground.Show();
                modal.Owner = modalbackground;

                parentX = this.Location.X;
                parentY = this.Location.Y;

                modal.ShowDialog();
                modalbackground.Dispose();
                

            }
        }

        private void serialPort2_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string dataIn = serialPort2.ReadTo("\n");
            Data_Parsing(dataIn);
            this.BeginInvoke(new EventHandler(Show_Data));

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
           
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

       

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox8_Click_1(object sender, EventArgs e)
        {
            FormCover modalbg = new FormCover();
            using (FormInfo modal = new FormInfo(this))
            {
                modalbg.StartPosition = FormStartPosition.Manual;
                modalbg.Opacity = .50d;
                modalbg.BackColor = Color.Black;
                modalbg.Size = this.Size;
                modalbg.Location = this.Location;
                modalbg.ShowInTaskbar = false;
                modalbg.Show();
                modal.Owner = modalbg;

                parentX = this.Location.X;
                parentY = this.Location.Y;

                modal.ShowDialog();
                modalbg.Dispose();
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FormSettings.modeP == "Manual")
            {
                button6.Visible = true;
            }
            else
            {
                button6.Visible = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormCover modalbackground = new FormCover();
            using (FormPeringatan modal = new FormPeringatan(this))
            {
                modalbackground.StartPosition = FormStartPosition.Manual;
                modalbackground.Opacity = .60d;
                modalbackground.BackColor = Color.Black;
                modalbackground.Size = this.Size;
                modalbackground.Location = this.Location;
                modalbackground.ShowInTaskbar = false;
                modalbackground.Show();
                modal.Owner = modalbackground;

                parentX = this.Location.X;
                parentY = this.Location.Y;

                modal.ShowDialog();
                modalbackground.Dispose();

            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            FormCover modalbackground = new FormCover();
            using (FormSettings modal = new FormSettings(this))
            {
                modalbackground.StartPosition = FormStartPosition.Manual;
                modalbackground.Opacity = .60d;
                modalbackground.BackColor = Color.Black;
                modalbackground.Size = this.Size;
                modalbackground.Location = this.Location;
                modalbackground.ShowInTaskbar = false;
                modalbackground.Show();
                modal.Owner = modalbackground;

                parentX = this.Location.X;
                parentY = this.Location.Y;

                modal.ShowDialog();
                modalbackground.Dispose();

            }
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void chartTemp1_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void chartCO1_Click(object sender, EventArgs e)
        {

        }

        private void chartKelembaban1_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click_1(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void Data_Parsing(string data)
        {
            sbyte karakterAwal = (sbyte)data.IndexOf("@");
            sbyte index1 = (sbyte)data.IndexOf("a"); //co
            sbyte index2 = (sbyte)data.IndexOf("b"); //temp
            sbyte index3 = (sbyte)data.IndexOf("c"); //kelemaban
            sbyte index4 = (sbyte)data.IndexOf("d"); //pm1
            sbyte index5 = (sbyte)data.IndexOf("e"); //pm2.5
            sbyte index6 = (sbyte)data.IndexOf("f"); //pm10


            if (index1 != -1 && index2 != -1 && index3 != -1 && index4 != 1 && index5 != 1 && 
                index6 != 1 && karakterAwal != -1)
            {
                try
                {
                    str_CO = data.Substring(karakterAwal + 1, (index1 - karakterAwal) - 1);
                    str_Temp = data.Substring(index1 + 1, (index2 - index1) - 1);
                    str_Kelembaban = data.Substring(index2 + 1, (index3 - index2) - 1);
                    string str_pm1 = data.Substring(index3 + 1, (index4 - index3) - 1);
                    str_pm25 = data.Substring(index4 + 1, (index5 - index4) - 1);
                    str_lat = "-6.2643171416259795";
                    str_lon = " 106.7486394859972";
                    str_pm10 = data.Substring(index5 + 1, (index6 - index5) - 1);
                    
                    updateData = true;


                }
                catch (Exception)
                {

                }
            }
            else
            {
                updateData = false;
            }
        }
        public double ISPUcal(double input, int[] ispu_ambang, int[] ambien_ambang)
        {

            ba = batas_atas(input, ispu_ambang);
            ba = Convert.ToInt16(ba);
            bb = batas_bawah(input, ispu_ambang);
            bb = Convert.ToInt16(bb);
            Ia = ispu_ambang[ba];
            Ib = ispu_ambang[bb];
            Xa = ambien_ambang[ba];
            Xb = ambien_ambang[bb];
            Xx = input;


            double ISPU = ((Ia - Ib) / (Xa - Xb)) * (Xx - Xb) + Ib;
            return ISPU;
        }

        private void Show_Data(object sender, EventArgs e)
        {
            //Konversi lat lon ke lokasi//

            double lat = Convert.ToDouble(str_lat);
            double lon = Convert.ToDouble(str_lon);

            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                  "Windows NT 5.2; .NET CLR 1.0.3705;)");
            webClient.Headers.Add("Referer", "http://www.microsoft.com");
            var jsonData = webClient.DownloadData($"http://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lon}");

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));
            RootObject rootObject = (RootObject)ser.ReadObject(new MemoryStream(jsonData));
            label33.Text = rootObject.address.village + ", " + rootObject.address.city_district + ", " + rootObject.address.city + ", " + rootObject.address.state;


            textBox11.Text = str_pm25; 
            textBox10.Text = str_pm10;
            textBox4.Text = str_CO;
            textBox6.Text = str_Temp;
            textBox7.Text = str_Kelembaban;
            // textBox4.Text = str_Tekanan;
            label22.Text = DateTime.Now.ToString("hh:mm:ss tt");
            if (lastMarker != null)
                lastMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver;  // Hide tooltip for the previous marker
            PointLatLng newPosition = new PointLatLng(double.Parse(str_lat), double.Parse(str_lon));


            Marker_Payload = new GMarkerGoogle(newPosition, bmpPesawat);
            Marker_Payload.ToolTipText = "Lat: " + newPosition.Lat + "\nLng: " + newPosition.Lng;  // Tooltip for the current marker
            lastMarker = Marker_Payload;  // Set the current marker as the last marker*

            Overlay_Marker.Markers.Add(Marker_Payload);
            Koordinat_Trace.Add(newPosition);
            Route_Trace = new GMapRoute(Koordinat_Trace, "Route");
            Route_Trace.Stroke = new Pen(Color.Red, 3);

            Overlay_Trace.Routes.Clear();  // Clear previous route
            Overlay_Trace.Routes.Add(Route_Trace);

            Koordinat_Awal = new PointLatLng(double.Parse(str_lat), double.Parse(str_lon));
            map1.Overlays.Add(Overlay_Marker);
            map1.Overlays.Add(Overlay_Trace);

            map1.Position = newPosition;


            chartCO1.Series[0].Points.AddXY(DateTime.Now.ToString("hh:mm:ss tt"), str_CO);
            chartTemp1.Series[0].Points.AddXY(DateTime.Now.ToString("hh:mm:ss tt"), str_Temp);
            chartKelembaban1.Series[0].Points.AddXY(DateTime.Now.ToString("hh:mm:ss tt"), str_Kelembaban);
            chartpm25.Series[0].Points.AddXY(DateTime.Now.ToString("hh:mm:ss tt"), str_pm25);
            chartpm10.Series[0].Points.AddXY(DateTime.Now.ToString("hh:mm:ss tt"), str_pm10);
            // chartTekanan.Series[0].Points.AddXY(DateTime.Now.ToString("hh:mm:ss tt"), str_Tekanan);
            try
            {

                kCO = Convert.ToDouble(str_CO);
                dpm25 = Convert.ToDouble(str_pm25);
                dpm10 = Convert.ToDouble(str_pm10);
                // Convert.ToInt16(textBox4.Text);
            }
            catch { kCO = 0; dpm25 = 0; dpm10 = 0; }
            //kCO = Convert.ToInt16(textBox4.Text);

            double coISPU = ISPUcal(kCO, ispu_ambang, ambien_ambang);
            double pm25ISPU = ISPUcal(dpm25, ispu_ambang, pm25ambien_ambang);
            double pm10ISPU = ISPUcal(dpm10, ispu_ambang, pm10ambien_ambang);


            if (coISPU >= pm25ISPU && coISPU >= pm10ISPU)
            {
                ISPU = coISPU;
                cemar = "CO";
            }
            else if (pm25ISPU >= coISPU && pm25ISPU >= pm10ISPU)
            {
                ISPU = pm25ISPU;
                cemar = "PM2.5";
            }
            else
            {
                ISPU = pm10ISPU;
                cemar = "PM10";
            }


            ISPU = Convert.ToInt32(ISPU);
            string ISPUs = Convert.ToString(ISPU);

            if (ISPU <= 50)
            {
                status = "Baik";
                label31.ForeColor = Color.FromArgb(22, 255, 0);
            }
            else if (ISPU <= 100)
            {
                status = "Sedang";
                label31.ForeColor = Color.FromArgb(117, 194, 246);
            }
            else if (ISPU <= 200)
            {
                status = "Tidak Sehat";
                label31.ForeColor = Color.FromArgb(248, 222, 34);
            }
            else if (ISPU <= 300)
            {
                status = "Sangat Tidak Sehat";
                label31.ForeColor = Color.FromArgb(199, 0, 57);
            }
            else if (ISPU > 300)
            {
                status = "Berbahaya";
                label31.ForeColor = Color.FromArgb(0, 0, 0);
            }
            string ISPUS = Convert.ToString(ISPU);
            label32.Text = ISPUS;
            label31.Text = status;
            data.Add(new data()
            {
                csvWaktu = DateTime.Now.ToString("dd/MMMM/yyyy/hh:mm:ss"),
                csvLokasi = rootObject.address.village+"/"+ rootObject.address.city + "/" + rootObject.address.state,
                csvCO = textBox4.Text,
                csvSuhu = textBox6.Text,
                csvKelembaban = textBox7.Text,
                csvpm25 = textBox11.Text,
                csvISPU = label32.Text,
                csvkategori =label31.Text,
                csvpm10 = textBox10.Text,
                

            });;
            if (FormSave.f2btnCSV == true)
            {
                FormSave.f2btnCSV = false;
                tulis();
                MessageBox.Show("CSV saved");
            }
            if (FormSave.f2btnSave == true)
            {
                FormSave.f2btnSave = false;
                saveChart();

            }
             temp = Convert.ToDouble(str_Temp);
             lati = Convert.ToString(lat);
             longi = Convert.ToString(lon);
             suhu = Convert.ToString(temp);
             ppmCO = Convert.ToString(Xx);
             nISPU = Convert.ToString(ISPU);
             hp = Convert.ToString(FormSettings.hPpenerima);
            tanggal = label21.Text;
            waktu = label22.Text;
            lokasi = label33.Text;
            rh = textBox7.Text;

            pm10 = textBox10.Text;
            pm25 = textBox11.Text;
            


            //early warning ku dan kh otomatis
            if (FormSettings.modeP == "Otomatis" && bisaSend == true)
            {
                if (status == FormSettings.tISPU || status != "Baik" || status != "Sedang")
                {

                    if (temp >= 40)
                    {
                        alert = "pku";

                        send_data(alert, tanggal, waktu, lokasi, FormSettings.penerima, hp,
                            lati, longi, suhu, rh, ppmCO,pm25,pm10,cemar, nISPU, status);
                    }
                    else
                    {
                        alert = "pkh";
                        send_data(alert, tanggal, waktu, lokasi, FormSettings.penerima, hp,
                             lati, longi, suhu, rh, ppmCO, pm25, pm10, cemar, nISPU, status);
                    }
                }
                else if (temp >= 30)
                {
                    alert = "ppkh";
                    send_data(alert, tanggal, waktu, lokasi, FormSettings.penerima, hp,
                             lati, longi, suhu, rh, ppmCO, pm25, pm10, cemar, nISPU, status);
                }

                bisaSend = false;
                timer2.Start();



            }
        }

        public void send_data(string alert,string tanggal, string jam, string lokasi, string penerima, string hp, string lat, string lon, string temp, string hum, string kCO, string pm25, string pm10, string cemar,string ISPU, string kategori) 
        {
            Data_json data = new Data_json {

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
                pm25 = pm25,
                pm10 = pm10,
                cemar = cemar,
                ISPU = ISPU,
                kategori = kategori

        };

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string json = JsonConvert.SerializeObject(data);
        }
        public void tulis()
        {
            engine.WriteFile("data.csv", data);
        }

        public int batas_bawah(double input, int[] ambien_ambang)
        {
            for (int i = 0; i < ambien_ambang.Length; i++)
            {
                if (input <= ambien_ambang[i])
                {
                    nilai2 = i - 1;
                }
            }
            return nilai2;

        }

        public int batas_atas(double input, int[] ispu_ambang)
        {

            for (int i = 0; i < ispu_ambang.Length; i++)
            {
                if (input<= ispu_ambang[i])
                {
                    nilai1 = i;
                }
            }
            return nilai1;

        }


        public void saveChart()
        {
            //<---save chart CO-->//
            SaveFileDialog svchgelPantai = new SaveFileDialog();
            svchgelPantai.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            svchgelPantai.Title = "Save Chart CO";
            svchgelPantai.ShowDialog();
            if (svchgelPantai.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)svchgelPantai.OpenFile();
                chartCO1.SaveImage(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                string path = System.IO.Path.GetFullPath(svchgelPantai.FileName);

            }

            //<---save chart SUHU -->//
            SaveFileDialog svchgelLaut = new SaveFileDialog();
            svchgelLaut.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            svchgelLaut.Title = "Save Char Suhu";
            svchgelLaut.ShowDialog();
            if (svchgelLaut.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)svchgelLaut.OpenFile();
                chartTemp1.SaveImage(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                string path = System.IO.Path.GetFullPath(svchgelLaut.FileName);

            }

            //<---save chart Kelembaban -->//
            SaveFileDialog svchsuhuPntai = new SaveFileDialog();
            svchsuhuPntai.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            svchsuhuPntai.Title = "Save Chart Kelembaban";
            svchsuhuPntai.ShowDialog();
            if (svchsuhuPntai.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)svchsuhuPntai.OpenFile();
                chartpm25.SaveImage(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                string path = System.IO.Path.GetFullPath(svchsuhuPntai.FileName);

            }
            //<---save chart Kelembaban -->//
            SaveFileDialog svchhumPntai = new SaveFileDialog();
            svchhumPntai.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            svchhumPntai.Title = "Save Chart Kelembaban";
            svchhumPntai.ShowDialog();
            if (svchhumPntai.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)svchhumPntai.OpenFile();
                chartKelembaban1.SaveImage(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                string path = System.IO.Path.GetFullPath(svchhumPntai.FileName);

            }

            SaveFileDialog svchpmPntai = new SaveFileDialog();
            svchpmPntai.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            svchpmPntai.Title = "Save PM10";
            svchpmPntai.ShowDialog();
            if (svchpmPntai.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)svchpmPntai.OpenFile();
                chartpm10.SaveImage(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                string path = System.IO.Path.GetFullPath(svchpmPntai.FileName);

            }



        }


    }
}
