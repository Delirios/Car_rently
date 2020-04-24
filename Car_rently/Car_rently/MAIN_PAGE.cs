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
    
        public string E_mail
        {
            get { return label7.Text; }
            set { label7.Text = value; }
        }

        List<string> list = new List<string>();
        int id_client = 0;
        int count = 0;
        static string commandstring = "select distinct brand_name from available_cars_view; ";
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        SqlDataAdapter adapter = new SqlDataAdapter(commandstring, connectionString); //створюємо екземпляр класу адаптер
        DataSet dataset = new DataSet(); // створюємо датасет(копія бази даних)

        public MAIN_PAGE()
        {
            InitializeComponent();
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

        #region КНОПКА ЗАКРИТТЯ ПРОГРАМИ
        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region МЕТОД LOAD
        private void MAIN_PAGE_Load(object sender, EventArgs e)
        {
            string E_mail = label7.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();
                adapter.Fill(dataset);
                metroComboBox1.DataSource = dataset.Tables[0];
                metroComboBox1.DisplayMember = "brand_name";// стовпець для відображення
                                                            
                command.CommandText = "select Id_client from client where E_mail = @E_mail";
                command.Parameters.AddWithValue("@E_mail", E_mail);
                id_client = Convert.ToInt32(command.ExecuteScalar());
            }
            
        }
        #endregion

        #region КНОПКА НАСТУПНЕ АВТО
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
                        textBox6.Text = thisReader["Id_car"].ToString();
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
        #endregion

        #region КНОПКА ПОПЕРЕДНЄ АВТО
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
                        textBox6.Text = thisReader["Id_car"].ToString();
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
        #endregion

        #region КНОПКА ВИЙТИ З АКАУНТУ
        private void button4_Click(object sender, EventArgs e)
        {
            Form start_page = Application.OpenForms[0];
            start_page.Show();
            this.Close();
        }
        #endregion

        #region КНОПКА ОСОБИСТИЙ КАБІНЕТ
        private void button5_Click(object sender, EventArgs e)
        {
            PERSONAL_OFFICE personal_office = new PERSONAL_OFFICE();
            personal_office.Id_client = id_client;
            personal_office.Closed += (s, a) => this.Show();
            this.Hide();
            personal_office.ShowDialog();

        }
        #endregion

        #region ВИВІД ДАННИХ ПРИ ВИБОРІ З КОМБОБОКСУ
        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                textBox5.Clear();
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM cars JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand WHERE cars_brand.brand_name =  @brand_name";
                command.Parameters.AddWithValue("@brand_name", metroComboBox1.Text);
                SqlDataReader thisReader = command.ExecuteReader();
                while (thisReader.Read())
                {
                    textBox5.Text += thisReader["Id_car"].ToString()+ " ";
                    textBox6.Text = thisReader["Id_car"].ToString();
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
        #endregion

        #region ЗАВАНТАЖЕННЯ ДАНИХ НА СТОРІНКУ ЗАМОВЛЕННЯ
        private void button3_Click(object sender, EventArgs e)
        {

            ORDER_PAGE order_page = new ORDER_PAGE();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();
               

                command.CommandText = "SELECT count(*) FROM cars JOIN rent ON cars.Id_car = rent.Id_car JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand lEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent WHERE Id_client = @Id_client AND rent_penalty.Id_rent Is NULL ";
                command.Parameters.AddWithValue("@Id_client", id_client);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                {
                    textBox5.Clear();
                    command.CommandText = "SELECT First_name, Last_name, Patronymic FROM client WHERE E_mail = @E_mail";
                    command.Parameters.AddWithValue("@E_mail", E_mail);
                    SqlDataReader thisReader = command.ExecuteReader();
                    while (thisReader.Read())
                    {
                        order_page.First_name = thisReader["First_name"].ToString();
                        order_page.Last_name = thisReader["Last_name"].ToString();
                        order_page.Patronymic = thisReader["Patronymic"].ToString();
                    }
                    order_page.Brand = metroComboBox1.Text;
                    order_page.Model = textBox1.Text;
                    order_page.Price = textBox4.Text;
                    order_page.E_mail = label7.Text;
                    order_page.Id_car = Convert.ToInt32(textBox6.Text);

                    order_page.Show();
                }
                else
                {
                    MessageBox.Show("У вас є незавершене замовлення!  Щоб оформити нове замовлення, закрийте попереднє!");
                }
            }
        }
        #endregion

    }
}
