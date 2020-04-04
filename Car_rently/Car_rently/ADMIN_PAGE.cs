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
    public partial class ADMIN_PAGE : Form
    {
        public ADMIN_PAGE()
        {
            InitializeComponent();
        }


        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public byte[] data = null;

        private void label12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region ЗАПОВНЕННЯ КОМБОБОКСІВ
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
            string sql_penalties = "SELECT * from penalties";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd1 = new SqlCommand(sql_penalties, connection);
                DataTable tbl1 = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(tbl1);
                checkedListBox1.DataSource = tbl1;
                checkedListBox1.DisplayMember = "penalty_name";// столбец для отображения
                checkedListBox1.ValueMember = "Id_penalty";//столбец с id
                //metroComboBox1.SelectedIndex = -1;
            }
            int[] array = Enumerable.Range(2000, 20).ToArray();
            metroComboBox2.DataSource = array;
            checkedListBox1.CheckOnClick = true;

        }
        #endregion

        #region ПЕРЕТЯГУВАННЯ ФОРМИ
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "insert into penalties (penalty_name, amount_penalty) values (@penalty_name, @amount_penalty)";
                command.Parameters.Add("@penalty_name", SqlDbType.NVarChar);
                command.Parameters.Add("@amount_penalty", SqlDbType.Int);
                // передаем данные в команду через параметры
                command.Parameters["@penalty_name"].Value = metroTextBox5.Text;
                command.Parameters["@amount_penalty"].Value = Convert.ToInt32(metroTextBox6.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Штраф додано!");
                metroTextBox5.Clear();
                metroTextBox6.Clear();
            }

        }
        #endregion

        #region ЗНИЖКИ
        private void button3_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = "insert into discounts(discount_name, discount_percent) values(@discount_name, @discount_percent)";
                command.Parameters.Add("@discount_name", SqlDbType.NVarChar);
                command.Parameters.Add("@discount_percent", SqlDbType.Int);
                // передаем данные в команду через параметры
                command.Parameters["@discount_name"].Value = metroTextBox7.Text;
                command.Parameters["@discount_percent"].Value = Convert.ToInt32(metroTextBox8.Text);
                command.ExecuteNonQuery();
                MessageBox.Show("Знижку додано!");
                metroTextBox7.Clear();
                metroTextBox8.Clear();
            }
        }
        #endregion

        #region ФОТО В БАЙТИ
        private byte[] ReadImageToBytes(string sPath)
        {
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            data = br.ReadBytes((int)numBytes);
            return data;
        }
        #endregion

        #region ВИВІД ФОТО В PICTUREBOX
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            pictureBox2.Image = Image.FromFile(openFileDialog.FileName);
            ReadImageToBytes(openFileDialog.FileName);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

        }
        #endregion

        #region ДОДАВАННЯ АВТО
        private void button1_Click_1(object sender, EventArgs e)
        {
            Image img = pictureBox2.Image;
            byte[] arr;
            ImageConverter converter = new ImageConverter();
            arr = (byte[])converter.ConvertTo(img, typeof(byte[]));

            string brand_name = metroTextBox1.Text.ToUpper();
            string model = metroTextBox2.Text.ToUpper();
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


                command.CommandText = "(SELECT Id_brand FROM cars_brand WHERE brand_name like '" + metroTextBox1.Text + "')";
                int i = Convert.ToInt32(command.ExecuteScalar());
                if (i == 0)
                {
                    command.CommandText = "insert into cars_brand (brand_name) values(@brand_name)";

                    command.Parameters.Add("@brand_name", SqlDbType.VarChar, 30);
                    command.Parameters["@brand_name"].Value = brand_name;
                    command.ExecuteNonQuery();
                }

                command.CommandText = "(SELECT Id_brand FROM cars_brand WHERE brand_name like '" + metroTextBox1.Text + "')";
                string brand = command.ExecuteScalar().ToString();

                command.CommandText = "insert into cars (Id_type, car_model, year, cost, price, picture,Id_brand) values (@type,@model,@year,@cost,@price,@picture,@brand)";




                command.Parameters.Add("@type", SqlDbType.Int);
                command.Parameters.Add("@brand", SqlDbType.Int);
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
        #endregion

        #region ВСІ СЕЛЕКТИ
        private void button5_Click(object sender, EventArgs e)
        {
            string sql = "SELECT Id_car, type_name,brand_name,car_model, year, cost,price, picture FROM cars JOIN type_of_car ON cars.Id_type = type_of_car.Id_type JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM penalties";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные
                dataGridView2.DataSource = ds.Tables[0];
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM discounts";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные
                dataGridView3.DataSource = ds.Tables[0];
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string sql = "SELECT rent.Id_rent, E_mail, brand_name, car_model,lease_date, return_date, rental_days, total_amount FROM cars JOIN rent ON cars.Id_car = rent.Id_car JOIN client  ON rent.Id_client = client.Id_client JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand lEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent WHERE rent_penalty.Id_rent IS NULL";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные
                dataGridView5.DataSource = ds.Tables[0];
            }

        }
        private void button11_Click(object sender, EventArgs e)
        {

            string sql = "SELECT * FROM client";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные
                dataGridView4.DataSource = ds.Tables[0];
            }

        }

        #endregion

        #region ВСІ ДЕЛІТИ
        private void button8_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("DELETE FROM cars WHERE id_car=@id", con);
                int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                com.Parameters.AddWithValue("@id", id);
                con.Open(); //Открываем подключение
                try
                {
                    com.ExecuteNonQuery();
                    MessageBox.Show("Автомобіль видалено");
                }
                catch
                {
                    MessageBox.Show("Видалити не вдалось!");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("DELETE FROM penalties WHERE id_penalty=@id", con);
                int id = int.Parse(dataGridView2.CurrentRow.Cells[0].Value.ToString());
                com.Parameters.AddWithValue("@id", id);
                con.Open(); //Открываем подключение
                try
                {
                    com.ExecuteNonQuery();
                    MessageBox.Show("Штраф видалено");
                }
                catch
                {
                    MessageBox.Show("Видалити не вдалось!");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("DELETE FROM discounts WHERE id_discount=@id", con);
                int id = int.Parse(dataGridView3.CurrentRow.Cells[0].Value.ToString());
                com.Parameters.AddWithValue("@id", id);
                con.Open(); //Открываем подключение
                try
                {
                    com.ExecuteNonQuery();
                    MessageBox.Show("Знижку видалено");
                }
                catch
                {
                    MessageBox.Show("Видалити не вдалось!");
                }
            }
        }


        private void button12_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand com = new SqlCommand("DELETE FROM client WHERE id_client=@id", con);
                int id = int.Parse(dataGridView4.CurrentRow.Cells[0].Value.ToString());
                com.Parameters.AddWithValue("@id", id);
                con.Open(); //Открываем подключение
                try
                {
                    com.ExecuteNonQuery();
                    MessageBox.Show("Клієнта видалено");
                }
                catch
                {
                    MessageBox.Show("Видалити не вдалось!");
                }
            }

        }
        #endregion

        private void button14_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandText = "(SELECT rent.Id_rent, brand_name,picture,car_model,lease_date,return_date,total_amount FROM rent JOIN client ON rent.Id_client = client.Id_client JOIN cars ON rent.Id_car = cars.Id_car lEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent JOIN cars_brand ON cars.Id_brand = cars_brand.Id_brand WHERE E_mail = '" + metroTextBox9.Text + "' AND rent_penalty.Id_rent IS NULL)";
                int i = Convert.ToInt32(command.ExecuteScalar());
                SqlDataReader thisReader = command.ExecuteReader();

                while (thisReader.Read())
                {
                    label30.Text = thisReader["Id_rent"].ToString();
                    label18.Text = thisReader["brand_name"].ToString();
                    label20.Text = thisReader["car_model"].ToString();
                    label23.Text = Convert.ToDateTime(thisReader["lease_date"]).ToShortDateString();
                    label24.Text = Convert.ToDateTime(thisReader["return_date"]).ToShortDateString();
                    label29.Text = thisReader["total_amount"].ToString();
                    byte[] picbyte = thisReader["picture"] as byte[] ?? null;
                    if (picbyte != null)
                    {
                        MemoryStream mstream = new MemoryStream(picbyte);
                        pictureBox3.Image = System.Drawing.Image.FromStream(mstream);
                        {
                            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(mstream);
                            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }


                }
                if (i == 0)
                {
                    MessageBox.Show("Замовлення з таким E_mail не існує!");
                }
            }
                
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                label28.Text = "0";
            }
            label16.Text = label29.Text;


        }     

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            int summ = 0;
            int index = 0;
            foreach (int indexChecked in checkedListBox1.CheckedIndices)
            {
                index += indexChecked;
            }
            if (index != 0)
            {


                foreach (object item in checkedListBox1.CheckedItems)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        connection.Open();

                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;

                        string curItemString = ((DataRowView)item)[checkedListBox1.DisplayMember].ToString();
                        // выполняем действия со строкой
                        command.CommandText = "(SELECT amount_penalty FROM penalties WHERE penalty_name = '" + curItemString + "')";

                        SqlDataReader thisReader = command.ExecuteReader();
                        while (thisReader.Read())
                        {

                            summ += Convert.ToInt32(thisReader["amount_penalty"]);

                        }
                    }
                }
                label28.Text = summ.ToString();
            }
            else { label28.Text = "0"; }
            try
            {

                if (label28.Text != "0")
                {
                    double first = Convert.ToDouble(label29.Text);
                    double second = Convert.ToDouble(label28.Text);
                    double result = first + second;
                    label16.Text = result.ToString();
                }
                else { label16.Text = label29.Text; }
            }
            catch
            {
                label28.Text = "0";
                MessageBox.Show("Введіть E_mail!");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {

            string penalty = "";
            foreach (object item in checkedListBox1.CheckedItems)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    string curItemString = ((DataRowView)item)[checkedListBox1.DisplayMember].ToString();
                    // выполняем действия со строкой
                    command.CommandText = "(SELECT Id_penalty FROM penalties WHERE penalty_name = '" + curItemString + "')";

                    SqlDataReader thisReader = command.ExecuteReader();
                    while (thisReader.Read())
                    {

                        penalty += thisReader["Id_penalty"].ToString() + " ";

                    }
                }

            }
            int rent = Convert.ToInt32(label30.Text);
            int[] arr = penalty.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(k => int.Parse(k.Trim())).ToArray();
            if (arr.Length != 0)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;

                        int pen = arr[i];
                        command.CommandText = "insert into rent_penalty (Id_penalty, Id_rent) values(@Id_penalty,@Id_rent)";




                        command.Parameters.Add("@Id_penalty", SqlDbType.Int);
                        command.Parameters.Add("@Id_rent", SqlDbType.Int);

                        command.Parameters["@Id_penalty"].Value = pen;
                        command.Parameters["@Id_rent"].Value = rent;

                        command.ExecuteNonQuery();
                        double amount = Convert.ToDouble(label16.Text);
                        command.CommandText = "UPDATE rent SET total_amount = '" +Math.Round(amount) + "' WHERE Id_rent = '" + rent + "';";
                        command.ExecuteNonQuery();                        




                    }
                }
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "insert into rent_penalty ( Id_rent) values(@Id_rent)";

                    command.Parameters.Add("@Id_rent", SqlDbType.Int);

                    command.Parameters["@Id_rent"].Value = rent;

                    command.ExecuteNonQuery();




                }
            }

            MessageBox.Show("Замовлення закрито!");
            ADMIN_PAGE admin = new ADMIN_PAGE();
            this.Close();
            admin.Show();


        }

        private void button16_Click(object sender, EventArgs e)
        {
            string sql = "SELECT DISTINCT rent.Id_rent, E_mail, brand_name, car_model,lease_date, return_date, rental_days, total_amount FROM cars JOIN rent ON cars.Id_car = rent.Id_car JOIN client  ON rent.Id_client = client.Id_client JOIN cars_brand  ON cars.Id_brand = cars_brand.Id_brand lEFT OUTER JOIN rent_penalty ON rent.Id_rent = rent_penalty.Id_rent WHERE rent_penalty.Id_rent IS NOT NULL";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                // Отображаем данные
                dataGridView6.DataSource = ds.Tables[0];
            }
        }

        private void metroTextBox9_Click(object sender, EventArgs e)
        {
            metroTextBox9.Clear();
        }
    }
}
