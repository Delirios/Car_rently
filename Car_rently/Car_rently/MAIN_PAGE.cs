using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_rently
{
    public partial class MAIN_PAGE : Form
    {
        public MAIN_PAGE()
        {
            InitializeComponent();
        }
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

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



        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        List<string> list = new List<string>();
        //int count = -1;
        private void MAIN_PAGE_Load(object sender, EventArgs e)
        {
            string sql = "SELECT TOP(1) WITH TIES * FROM cars_brand ORDER BY ROW_NUMBER()OVER(PARTITION BY brand_name ORDER BY Id_brand); ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd1 = new SqlCommand(sql, connection);
                DataTable tbl1 = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(tbl1);
                metroComboBox1.DataSource = tbl1;
                metroComboBox1.DisplayMember = "brand_name";// столбец для отображения
                metroComboBox1.ValueMember = "Id_brand";//столбец с id
                //metroComboBox1.SelectedIndex = -1;
            }
            
        }
        int count = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            int[] arr = textBox5.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(k => int.Parse(k.Trim())).ToArray();
            //if (count < 0) { count++; }
            if (count < arr.Length)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandText = "(SELECT * FROM cars WHERE Id_car = " + arr[count] + ")";
                    SqlDataReader thisReader = command.ExecuteReader();
                    while (thisReader.Read())
                    {
                        //textBox5.Text += thisReader["Id_car"].ToString() + " ";
                        textBox1.Text = thisReader["car_model"].ToString();
                        textBox2.Text = thisReader["year"].ToString();
                        textBox3.Text = thisReader["cost"].ToString();
                        textBox4.Text = thisReader["price"].ToString();
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
                count++;
            }
            if (count == arr.Length) { count = 0; };



        }
        private void button1_Click(object sender, EventArgs e)
        {
            int[] arr = textBox5.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(k => int.Parse(k.Trim())).ToArray();
            //if (count < 0) { count++; }
            if (count < arr.Length)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandText = "(SELECT * FROM cars WHERE Id_car = " + arr[count] + ")";
                    SqlDataReader thisReader = command.ExecuteReader();
                    while (thisReader.Read())
                    {
                        //textBox5.Text += thisReader["Id_car"].ToString() + " ";
                        textBox1.Text = thisReader["car_model"].ToString();
                        textBox2.Text = thisReader["year"].ToString();
                        textBox3.Text = thisReader["cost"].ToString();
                        textBox4.Text = thisReader["price"].ToString();
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
                count--;
            }
            if (count < 0) { count = arr.Length-1; };



        }

        public Image ByteArrayToImage(byte[] inputArray)
        {
            var memoryStream = new MemoryStream(inputArray);
            return Image.FromStream(memoryStream);
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {/*/
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string sql1 = "SELECT car_model FROM cars JOIN cars_brand cb ON cars.Id_brand = cb.Id_brand WHERE cb.brand_name =  '" + metroComboBox1.Text + "'";
                SqlCommand command = new SqlCommand(sql1, connection);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(dataReader.GetString(0));
                }
            }
            /*/
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                textBox5.Clear();
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM cars JOIN cars_brand cb ON cars.Id_brand = cb.Id_brand WHERE cb.brand_name =  '" + metroComboBox1.Text + "'";
                SqlDataReader thisReader = command.ExecuteReader();
                while (thisReader.Read())
                {
                    textBox5.Text += thisReader["Id_car"].ToString()+ " "; 
                    textBox1.Text = thisReader["car_model"].ToString();
                    textBox2.Text = thisReader["year"].ToString();
                    textBox3.Text = thisReader["cost"].ToString();
                    textBox4.Text = thisReader["price"].ToString();
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

        }


    }
}
