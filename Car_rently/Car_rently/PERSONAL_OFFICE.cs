using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_rently
{
    public partial class PERSONAL_OFFICE : Form
    {
        public PERSONAL_OFFICE()
        {
            InitializeComponent();
        }
        int id_client;
        public int Id_client
        {
            get { return id_client; }
            set { id_client = value; }
        }


        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {            
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                try
                {
                    if (textBox1.Text != "")
                    {
                        command.CommandText = "UPDATE client  SET E_mail = '" + textBox1.Text + "' WHERE Id_client = '" + id_client + "';";
                        command.ExecuteNonQuery();
                        MessageBox.Show("Пошту змінено. Перезайдіть в аккаунт");
                        this.Hide();
                        START_PAGE start_page = new START_PAGE();
                        start_page.ShowDialog();
                    }
                    if (textBox2.Text != "")
                    {
                        command.CommandText = "UPDATE client  SET Phone = '" + Convert.ToInt32(textBox2.Text) + "' WHERE Id_client = '" + id_client + "';";
                        command.ExecuteNonQuery();
                        MessageBox.Show("Збережено!");
                    }
                    if (textBox3.Text != "")
                    {
                        command.CommandText = "UPDATE client  SET Password = '" + textBox3.Text + "' WHERE Id_client = '" + id_client + "';";
                        command.ExecuteNonQuery();
                        MessageBox.Show("Збережено!");
                    }

                }
                catch {
                    MessageBox.Show("sdsd");
                }

                
            }
        }
        


        #region ПЕРЕТЯГУВАННЯ ФОРМИ
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                (sender as Control).Capture = false;//picturebox не ловит событие
                Message m = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.DefWndProc(ref m);

            }

        }
        #endregion

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide() ;
            MAIN_PAGE main_page = new MAIN_PAGE();
            main_page.ShowDialog();
        }
    }
}
