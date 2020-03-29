using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_rently
{
    public partial class ORDER_PAGE : Form
    {
        public string Last_name
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        public string First_name
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }

        public string Patronymic
        {
            get { return label3.Text; }
            set { label3.Text = value; }
        }
        public string Brand
        {
            get { return label4.Text; }
            set { label4.Text = value; }
        }
        public string Model
        {
            get { return label5.Text; }
            set { label5.Text = value; }
        }
        public string Price
        {
            get { return label7.Text; }
            set { label7.Text = value; }
        }

        public ORDER_PAGE()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void metroDateTime2_ValueChanged(object sender, EventArgs e)
        {
            DateTime start = metroDateTime1.Value;
            DateTime end = metroDateTime2.Value;
            TimeSpan result = (end - start).Duration();
            label15.Text = ($"{result.Days + 1}");

            int first = int.Parse($"{result.Days+1}");
            int second = int.Parse(label7.Text);
            int price = first * second;
            label6.Text = price.ToString();
        }

        
        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            metroDateTime1.MinDate = DateTime.Now;
            DateTime dateTime = metroDateTime1.Value;
            metroDateTime2.MinDate = dateTime;
        }
    }
}
