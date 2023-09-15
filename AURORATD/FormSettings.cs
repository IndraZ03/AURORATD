using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace AURORATD
{
    public partial class FormSettings : Form
    {
        public Form1 _Form1;

        public static string penerima = "Pemerintah Daerah Pondok Aren";
        public static long hPpenerima = 6289666444301;
        public static long hPengirim = 6281927914613;
        public static string tISPU = "Tidak Sehat";
        public static double tTemp = 40;
        public static int interval = 30;
        public static string modeP ="Manual";

        public FormSettings(Form1 form1)
        {
            InitializeComponent();
            form1 = new Form1();
            _Form1 = form1;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;


            comboBox1.Items.Add("Manual");
            comboBox1.Items.Add("Otomatis");
            comboBox2.Items.Add("Baik");
            comboBox2.Items.Add("Sedang");
            comboBox2.Items.Add("Tidak Sehat");
            comboBox2.Items.Add("Sangat Tidak Sehat");
            comboBox2.Items.Add("Bahaya");

        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Form1.parentX + 413, Form1.parentY + 245);
          
           

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.success_icon_23192_Windows;
            popup.BodyColor = Color.FromArgb(84, 180, 53);
            popup.HeaderColor = Color.FromArgb(84, 180, 53);
            popup.BorderColor = Color.FromArgb(84, 180, 53);
            popup.TitleText = "Sukses";
            popup.TitleColor = Color.White;
            popup.TitleFont = new Font("Yu Gothic UI ", 15, FontStyle.Bold);
            popup.ContentText = "Sukses Tersimpan";
            popup.ContentColor = Color.White;
            popup.ContentFont = new Font("Yu Gothic UI", 12);
            try { modeP = comboBox1.Text; } catch { modeP = "Manual"; }
       
            interval = Convert.ToInt32(textBox4.Text);
            tISPU = comboBox2.Text;
            tTemp = Convert.ToDouble(textBox6.Text);
            penerima = textBox1.Text;
            hPpenerima = Convert.ToInt64(textBox6.Text);
            popup.Popup();
            this.Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(comboBox1.Text == "Manual")
            {
                textBox4.Visible = false;
                comboBox2.Visible = false;
                textBox6.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
            }
            else
            {
                textBox4.Visible = true;
                comboBox2.Visible = true;
                textBox6.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
