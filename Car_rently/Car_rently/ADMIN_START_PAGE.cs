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
    public partial class ADMIN_START_PAGE : Form
    {
        public ADMIN_START_PAGE()
        {
            InitializeComponent();
        }

        
        private void label2_Click(object sender, EventArgs e)
        {
            Form start_page = Application.OpenForms[0];
            start_page.Show();
            this.Close();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel1.BackColor = Color.FromArgb(78, 184, 206);
            textBox1.ForeColor = Color.FromArgb(78, 184, 206);

            panel2.BackColor = Color.WhiteSmoke;
            textBox2.ForeColor = Color.WhiteSmoke;

        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel2.BackColor = Color.FromArgb(78, 184, 206);
            textBox2.ForeColor = Color.FromArgb(78, 184, 206);

            panel1.BackColor = Color.WhiteSmoke;
            textBox1.ForeColor = Color.WhiteSmoke;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == "1" && textBox2.Text == "1") //"если" в поле1 значение "user" и если в поле2 значение "pass" P.S user - логин ; pass - пароль!!!
            {
                ADMIN_PAGE admin_page = new ADMIN_PAGE();
                admin_page.Show();
                this.Close();

            }
            else //иначе
            {
                MessageBox.Show("Неверный пароль или логин", "Ошибка"); //вылазит ошибка о неверном пароле!
            }
        }
    }
}
