﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

        byte[] salt;

        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        static string commandstring = "SELECT DISTINCT brand_name, car_model,lease_date, return_date, rental_days, total_amount FROM cars JOIN rent ON cars.Id_car = rent.Id_car JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand lEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent WHERE Id_client = @id AND rent_penalty.Id_rent IS not NULL ; ";
        SqlDataAdapter adapter = new SqlDataAdapter(commandstring, connectionString); //створюємо екземпляр класу адаптер
        DataSet dataset = new DataSet(); // створюємо датасет(копія бази даних)

        #region ЗАВАНТАЖЕННЯ ФОРМИ
        private void PERSONAL_OFFICE_Load(object sender, EventArgs e)
        {
            adapter.SelectCommand.Parameters.AddWithValue("@id", id_client);
            adapter.Fill(dataset);
            metroGrid1.DataSource = dataset.Tables[0];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "(SELECT rent.Id_rent FROM rent LEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent WHERE Id_client = @Id_client AND rent_penalty.Id_rent IS NULL)";
                command.Parameters.AddWithValue("@Id_client", id_client);

                int i = Convert.ToInt32(command.ExecuteScalar());
                command.Parameters.Clear();

                if (i != 0)
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "show_order_to_customer";
                        command.Parameters.AddWithValue("@Id_client", id_client);
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
        }
        #endregion

        #region ВНЕСЕННЯ ЗМІН ДО БАЗИ
        private void button2_Click(object sender, EventArgs e)
        {
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(textBox3.Text, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                try
                {
                    if (textBox1.Text != "")
                    {
                        command.CommandText = "UPDATE client  SET E_mail = @E_mail WHERE Id_client = @Id_client;";
                        command.Parameters.AddWithValue("@E_mail", textBox1.Text);
                        command.Parameters.AddWithValue("@Id_client", id_client);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Пошту змінено. Перезайдіть в аккаунт");
                        this.Hide();
                        START_PAGE start_page = new START_PAGE();
                        start_page.ShowDialog();
                    }
                    if (textBox2.Text != "")
                    {
                        command.CommandText = "UPDATE client SET Phone = @Phone WHERE Id_client = @Id_client;";
                        command.Parameters.AddWithValue("@Phone", Convert.ToInt32(textBox2.Text));
                        command.Parameters.AddWithValue("@Id_client", id_client);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Збережено!");
                    }
                    if (textBox3.Text != "")
                    {
                        command.CommandText = "UPDATE client  SET Password = @Password WHERE Id_client = @Id_client;";
                        command.Parameters.AddWithValue("@Password", savedPasswordHash);
                        command.Parameters.AddWithValue("@Id_client", id_client);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Збережено!");
                    }
                }
                catch
                {
                    MessageBox.Show("Не заповненно жодного поля!");
                }
            }
        }
        #endregion

        #region ПЕРЕТЯГУВАННЯ ФОРМИ
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                (sender as Control).Capture = false;//picturebox не відловлює подію
                Message m = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.DefWndProc(ref m);

            }

        }
        #endregion

        #region ЗАКРИТТЯ ФОРМИ
        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        #endregion

    }

}
