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
using System.Security.Cryptography;

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
                string sql = "select * from client WHERE client.E_mail = '" + textBox3.Text + "'";
                connection.Open();

                SqlDataAdapter sda = new SqlDataAdapter(sql,connection);
                DataTable dtbl = new DataTable();

                sda.Fill(dtbl);
                string savedPasswordHash = dtbl.Rows[0][6].ToString();
                byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);

                var pbkdf2 = new Rfc2898DeriveBytes(textBox2.Text, salt, 10000);

                byte[] hash = pbkdf2.GetBytes(20);

                int ok = 1;
                for (int i = 0; i < 20; i++)
                    if (hashBytes[i + 16] != hash[i])
                        ok = 0;



                if (ok == 1 )
                {
                    main_page.E_mail = textBox3.Text;
                    this.Hide();
                    textBox3.Clear();
                    textBox2.Clear();
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
