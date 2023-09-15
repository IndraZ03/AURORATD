using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using FileHelpers;
namespace AURORATD
{
    public partial class FormSave : Form
    {
        public Form1 _Form1;
        public static bool f2btnSave = false;
        public static bool f2btnCSV = false;
        public FormSave(Form1 form1)
        {
            InitializeComponent();
            form1 = new Form1();
            _Form1 = form1;
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            string form2CO = Form1.svCO;
            string form2Suhu = Form1.svTemp;
            string form2Kelembaban= Form1.svKelembaban;
            string form2kec = Form1.svKec;

            if (checkBox1.Checked)
            {
                f2btnCSV = true;
            }
            if (checkBox2.Checked)
            {

                f2btnSave = true;
            }
            if (checkBox1.Checked && checkBox2.Checked)
            {
                f2btnCSV = true;
                f2btnSave = true;
            }
            // f2btnSave = false;
            this.Close();

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modalEffect_Timer_Tick_Tick(object sender, EventArgs e)
        {
          
        }

        private void FormSave_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Form1.parentX + 527 , Form1.parentY + 231);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
