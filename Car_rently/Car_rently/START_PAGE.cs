using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Car_rently
{
    public partial class START_PAGE : Form
    {
        public START_PAGE()
        {
            InitializeComponent();

        }
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;





        private void button3_Click(object sender, EventArgs e)
        {
            SIGN_UP sign_up = new SIGN_UP();

           
            this.Hide();
            sign_up.ShowDialog();


        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel3.BackColor = Color.FromArgb(78, 184, 206);
            textBox3.ForeColor = Color.FromArgb(78, 184, 206);

            panel2.BackColor = Color.WhiteSmoke;
            textBox2.ForeColor = Color.WhiteSmoke;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            panel2.BackColor = Color.FromArgb(78, 184, 206);
            textBox2.ForeColor = Color.FromArgb(78, 184, 206);

            textBox2.PasswordChar = '*';

            panel3.BackColor = Color.WhiteSmoke;
            textBox3.ForeColor = Color.WhiteSmoke;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ADMIN_START_PAGE admin_page = new ADMIN_START_PAGE();
            //admin_page.Closed += (s, a) => this.Show();
            this.Hide();
            admin_page.ShowDialog();
            //this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                MAIN_PAGE main_page = new MAIN_PAGE();
                //string sql = "select * from client WHERE client.E_mail = '" + textBox3.Text + "'";
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "(SELECT * FROM client WHERE E_mail = '"+ textBox3.Text +"' AND Password = '" + textBox2.Text + "')";
                int i = Convert.ToInt32(command.ExecuteScalar());
                if (i != 0 )
                {
                    main_page.E_mail = textBox3.Text;
                    this.Hide();
                    main_page.ShowDialog();


                }
                else
                {
                    MessageBox.Show("Невірний логін або пароль");
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FORGOT_PASSWORD_PAGE forgot_password = new FORGOT_PASSWORD_PAGE();


            this.Hide();
            forgot_password.ShowDialog();
        }
    }
}
