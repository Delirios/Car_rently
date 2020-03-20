using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Car_rently
{
    public partial class SIGN_UP : Form
    {
        public SIGN_UP()
        {
            InitializeComponent();

            
        }


        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel4.BackColor = Color.FromArgb(78, 184, 206);
            textBox4.ForeColor = Color.FromArgb(78, 184, 206);

            panel1.BackColor = Color.WhiteSmoke;
            textBox1.ForeColor = Color.WhiteSmoke;

            panel2.BackColor = Color.WhiteSmoke;
            textBox2.ForeColor = Color.WhiteSmoke;

            panel3.BackColor = Color.WhiteSmoke;
            textBox3.ForeColor = Color.WhiteSmoke;

            panel5.BackColor = Color.WhiteSmoke;
            textBox5.ForeColor = Color.WhiteSmoke;

            panel6.BackColor = Color.WhiteSmoke;
            textBox6.ForeColor = Color.WhiteSmoke;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel1.BackColor = Color.FromArgb(78, 184, 206);
            textBox1.ForeColor = Color.FromArgb(78, 184, 206);

            panel2.BackColor = Color.WhiteSmoke;
            textBox2.ForeColor = Color.WhiteSmoke;

            panel3.BackColor = Color.WhiteSmoke;
            textBox3.ForeColor = Color.WhiteSmoke;

            panel4.BackColor = Color.WhiteSmoke;
            textBox4.ForeColor = Color.WhiteSmoke;

            panel5.BackColor = Color.WhiteSmoke;
            textBox5.ForeColor = Color.WhiteSmoke;

            panel6.BackColor = Color.WhiteSmoke;
            textBox6.ForeColor = Color.WhiteSmoke;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel2.BackColor = Color.FromArgb(78, 184, 206);
            textBox2.ForeColor = Color.FromArgb(78, 184, 206);

            panel1.BackColor = Color.WhiteSmoke;
            textBox1.ForeColor = Color.WhiteSmoke;

            panel3.BackColor = Color.WhiteSmoke;
            textBox3.ForeColor = Color.WhiteSmoke;

            panel4.BackColor = Color.WhiteSmoke;
            textBox4.ForeColor = Color.WhiteSmoke;

            panel5.BackColor = Color.WhiteSmoke;
            textBox5.ForeColor = Color.WhiteSmoke;

            panel6.BackColor = Color.WhiteSmoke;
            textBox6.ForeColor = Color.WhiteSmoke;

            textBox2.PasswordChar = '*';
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel3.BackColor = Color.FromArgb(78, 184, 206);
            textBox3.ForeColor = Color.FromArgb(78, 184, 206);

            panel1.BackColor = Color.WhiteSmoke;
            textBox1.ForeColor = Color.WhiteSmoke;

            panel2.BackColor = Color.WhiteSmoke;
            textBox2.ForeColor = Color.WhiteSmoke;

            panel4.BackColor = Color.WhiteSmoke;
            textBox4.ForeColor = Color.WhiteSmoke;

            panel5.BackColor = Color.WhiteSmoke;
            textBox5.ForeColor = Color.WhiteSmoke;

            panel6.BackColor = Color.WhiteSmoke;
            textBox6.ForeColor = Color.WhiteSmoke;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*/
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=car_rently;Integrated Security=True";

            string sql = " if not exists(select * from client WHERE client.E_mail = '" + textBox3.Text + "') INSERT INTO car_rently.dbo.client (First_name,Last_name,Patronymic,E_mail,Phone,Password) VALUES ('" + textBox4.Text + "','" + textBox1.Text + "','" + textBox5.Text + "','" + textBox3.Text + "','" + textBox6.Text + "','" + textBox2.Text + "')";
            //string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlExpression = "INSERT INTO car_rently.dbo.client (First_name,Last_name,Patronymic,E_mail,Phone,Password) VALUES ('" + textBox4.Text + "','" + textBox1.Text + "','" + textBox5.Text + "','" + textBox3.Text + "','" + textBox6.Text + "','" + textBox2.Text + "')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                int number = command.ExecuteNonQuery();
                //Console.WriteLine("Добавлено объектов: {0}", number);

    /*/

            string sql = "select * from client WHERE client.E_mail = '" + textBox3.Text + "'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                int i = Convert.ToInt32(command.ExecuteScalar());
                if (i == 0)
                {

                    string sqlExpression = "INSERT INTO car_rently.dbo.client (First_name,Last_name,Patronymic,E_mail,Phone,Password) VALUES ('" + textBox4.Text + "','" + textBox1.Text + "','" + textBox5.Text + "','" + textBox3.Text + "','" + textBox6.Text + "','" + textBox2.Text + "')";
                    command = new SqlCommand(sqlExpression, connection); 
                    
                    command.ExecuteNonQuery();
                    MessageBox.Show("Ви успішно зареєструвалися", "Реєстрація");

                    this.Hide();


                }
                else
                {
                    
                    MessageBox.Show("Користувач з такою поштою уже існує", "Реєстрація");

                }
            }
               
            //new MESSAGE_BOX().ShowDialog();
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel5.BackColor = Color.FromArgb(78, 184, 206);
            textBox5.ForeColor = Color.FromArgb(78, 184, 206);

            panel1.BackColor = Color.WhiteSmoke;
            textBox1.ForeColor = Color.WhiteSmoke;

            panel2.BackColor = Color.WhiteSmoke;
            textBox2.ForeColor = Color.WhiteSmoke;

            panel4.BackColor = Color.WhiteSmoke;
            textBox4.ForeColor = Color.WhiteSmoke;

            panel3.BackColor = Color.WhiteSmoke;
            textBox3.ForeColor = Color.WhiteSmoke;

            panel6.BackColor = Color.WhiteSmoke;
            textBox6.ForeColor = Color.WhiteSmoke;
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            textBox6.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel6.BackColor = Color.FromArgb(78, 184, 206);
            textBox6.ForeColor = Color.FromArgb(78, 184, 206);

            panel1.BackColor = Color.WhiteSmoke;
            textBox1.ForeColor = Color.WhiteSmoke;

            panel2.BackColor = Color.WhiteSmoke;
            textBox2.ForeColor = Color.WhiteSmoke;

            panel4.BackColor = Color.WhiteSmoke;
            textBox4.ForeColor = Color.WhiteSmoke;

            panel3.BackColor = Color.WhiteSmoke;
            textBox3.ForeColor = Color.WhiteSmoke;

            panel5.BackColor = Color.WhiteSmoke;
            textBox5.ForeColor = Color.WhiteSmoke;


        }
    }
}
