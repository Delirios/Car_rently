﻿using System;
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
    public partial class ADMIN_PAGE : Form
    {
        public ADMIN_PAGE()
        {
            InitializeComponent();
        }
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        private void ADMIN_PAGE_Load(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM car_rently.dbo.type_of_car";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd1 = new SqlCommand(sql, connection);
                DataTable tbl1 = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(tbl1);
                metroComboBox1.DataSource = tbl1;
                metroComboBox1.DisplayMember = "type_name";// столбец для отображения
                metroComboBox1.ValueMember = "Id_type";//столбец с id
                metroComboBox1.SelectedIndex = -1;
            }

            int[] array = Enumerable.Range(2000, 20).ToArray();
            metroComboBox2.DataSource = array;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Перетягування форми
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                (sender as Control).Capture = false;//picturebox не ловит событие
                Message m = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.DefWndProc(ref m);
            }
        }
        #endregion



        #region ШТРАФИ
        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "insert into penalties (penalty_name, amount_penalty) values ('" + metroTextBox5.Text + "','" + Convert.ToInt32(metroTextBox8.Text)+"')";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

            }

        }
        #endregion

        private void button3_Click_1(object sender, EventArgs e)
        {
            string sql = "insert into discounts (discount_name, discount_percent) values ('" + metroTextBox7.Text + "','" + Convert.ToInt32(metroTextBox6.Text) + "')";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

            }
        }


        public byte[] data = null;


        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            pictureBox2.Image = Image.FromFile(openFileDialog.FileName);
            ReadImageToBytes(openFileDialog.FileName);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

        }

        private byte[] ReadImageToBytes(string sPath)
        {
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            data = br.ReadBytes((int)numBytes);
            return data;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Image img = pictureBox2.Image;
            byte[] arr;
            ImageConverter converter = new ImageConverter();
            arr = (byte[])converter.ConvertTo(img, typeof(byte[]));

            string brand = metroTextBox1.Text;
            string model = metroTextBox2.Text;
            int year = Convert.ToInt32(metroComboBox2.SelectedValue.ToString());
            int cost = Convert.ToInt32(metroTextBox3.Text);
            int price = Convert.ToInt32(metroTextBox4.Text);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "(SELECT Id_type FROM type_of_car WHERE type_name like '" + metroComboBox1.Text + "')";
                string type = command.ExecuteScalar().ToString();

                command.CommandText = "insert into cars (Id_type, car_brand, car_model, year_of_production, cost, price, picture) values (@type,@brand,@model,@year,@cost,@price,@picture)";
                command.Parameters.Add("@type", SqlDbType.Int);
                command.Parameters.Add("@brand", SqlDbType.NVarChar, 20);
                command.Parameters.Add("@model", SqlDbType.NVarChar, 20);
                command.Parameters.Add("@year", SqlDbType.Int);
                command.Parameters.Add("@cost", SqlDbType.Int);
                command.Parameters.Add("@price", SqlDbType.Int);
                command.Parameters.Add("@picture", SqlDbType.VarBinary);

                // передаем данные в команду через параметры
                command.Parameters["@type"].Value = type;
                command.Parameters["@brand"].Value = brand;
                command.Parameters["@model"].Value = model;
                command.Parameters["@year"].Value = year;
                command.Parameters["@cost"].Value = cost;
                command.Parameters["@price"].Value = price;
                command.Parameters["@picture"].Value = arr;
                command.ExecuteNonQuery();
                MessageBox.Show("Данні додано!");

                metroTextBox1.Clear();
                metroTextBox2.Clear();
                metroTextBox3.Clear();
                metroTextBox4.Clear();

            }
            
        }
    }
}