using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AURORATD
{
    public partial class FormInfo : Form
    {

        public Form1 _Form1;
        public FormInfo(Form1 form1)
        {
            InitializeComponent();
           
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void FormInfo_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Form1.parentX + 414, Form1.parentY + 245);
            label10.Text = FormSettings.modeP;
            long hPenerima = FormSettings.hPpenerima;
            int numberOfDigits = 4; // Jumlah angka yang ingin diambil dari belakang
            long extractedDigits = hPenerima % (long)Math.Pow(10, numberOfDigits);
            label12.Text = "xxxxxxx" + extractedDigits;
            label13.Text = FormSettings.penerima;

        }

        private void label11_Click(object sender, EventArgs e)
        {
            //no.wa bot
        }

        private void label13_Click(object sender, EventArgs e)
        {
            //penerima
        }

        private void label12_Click(object sender, EventArgs e)
        {
            //No,penerima
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
