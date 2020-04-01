using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
                catch
                {
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
            this.Close();
            
        }

        private void PERSONAL_OFFICE_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "(SELECT rent.Id_rent FROM rent LEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent  WHERE Id_client = '" + id_client + "' AND rent_penalty.Id_rent IS  NULL )";
                int i = Convert.ToInt32(command.ExecuteScalar());
                try
                {
                    if (i != 0)
                    {
                        command.CommandText = "SELECT TOP 1 brand_name, car_model,lease_date, return_date, total_amount,picture FROM rent JOIN cars ON rent.Id_car = cars.Id_car JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand WHERE Id_client = '" + id_client + "' ORDER BY Id_rent DESC";
                        SqlDataReader thisReader = command.ExecuteReader();
                        while (thisReader.Read())
                        {
                            label5.Text = thisReader["brand_name"].ToString();
                            label6.Text = thisReader["car_model"].ToString();
                            label7.Text = Convert.ToDateTime(thisReader["lease_date"]).ToShortDateString();
                            label8.Text = Convert.ToDateTime(thisReader["return_date"]).ToShortDateString();
                            label10.Text = thisReader["total_amount"].ToString();
                            byte[] picbyte = thisReader["picture"] as byte[] ?? null;
                            if (picbyte != null)
                            {
                                MemoryStream mstream = new MemoryStream(picbyte);
                                pictureBox1.Image = System.Drawing.Image.FromStream(mstream);
                                {
                                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(mstream);
                                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                }
                            }
                        }
                    }
                    else
                    {
                        label5.Text = "Пусто";
                        label6.Text = "Пусто";
                        label7.Text = "Пусто";
                        label8.Text = "Пусто";
                        label10.Text = "Пусто";
                    }
                }
                catch { MessageBox.Show("eror"); }

            }

            string sql = "SELECT brand_name, car_model,lease_date, return_date, rental_days, total_amount FROM cars JOIN rent ON cars.Id_car = rent.Id_car JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand lEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent WHERE Id_client = '39' AND rent_penalty.Id_rent IS not NULL ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные
                metroGrid1.DataSource = ds.Tables[0];
            }
        }
    }
}
