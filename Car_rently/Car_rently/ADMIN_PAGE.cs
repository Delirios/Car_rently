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

        static string commandstring = "SELECT * FROM type_of_car;" +
            "SELECT * from penalties; " +
            "select * from available_cars_view;" +
            "select * from unavailable_cars_view;" +
            "SELECT * FROM discounts;" +
            "exec current_orders;" +
            "SELECT * FROM client; " +
            "exec rent_history;";

        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public byte[] data = null;

        SqlDataAdapter adapter = new SqlDataAdapter(commandstring, connectionString); //створюємо екземпляр класу адаптер
        DataSet dataset = new DataSet(); // створюємо датасет(копія бази даних)

        #region ВИХІД З ПРОГРАМИ І ОЧИЩЕННЯ ТЕКСТБОКСА
        private void label12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void metroTextBox9_Click(object sender, EventArgs e)
        {
            metroTextBox9.Clear();
        }
        #endregion

        #region ЗАПОВНЕННЯ КОМБОБОКСІВ І ТАБЛИЦЬ
        private void ADMIN_PAGE_Load(object sender, EventArgs e)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter.Fill(dataset);
                metroComboBox1.DataSource = dataset.Tables[0];
                metroComboBox1.DisplayMember = "type_name";// колонка для відображення
                metroComboBox1.ValueMember = "Id_type";// колонка з Id
                metroComboBox1.SelectedIndex = -1;


                checkedListBox1.DataSource = dataset.Tables[1];
                checkedListBox1.DisplayMember = "penalty_name";// колонка для відображення
                checkedListBox1.ValueMember = "Id_penalty";// колонка з Id
                //metroComboBox1.SelectedIndex = -1;
            }
            int[] array = Enumerable.Range(2000, 20).ToArray();
            metroComboBox2.DataSource = array;
            checkedListBox1.CheckOnClick = true;

            DataTable data = new DataTable();
            data = dataset.Tables[1];


            data.PrimaryKey = new DataColumn[] { data.Columns["Id_penalty"] };
            dataGridView2.DataSource = data;// таблиці з запиту по індексах
            dataGridView1.DataSource = dataset.Tables[2];
            dataGridView7.DataSource = dataset.Tables[3];
            dataGridView3.DataSource = dataset.Tables[4];
            dataGridView5.DataSource = dataset.Tables[5];
            dataGridView4.DataSource = dataset.Tables[6];
            dataGridView6.DataSource = dataset.Tables[7];

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

        #region ДОДАВАННЯ ШТРАФІВ
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
                dataset.Clear();
                adapter.Fill(dataset);
            }

        }
        #endregion

        #region ДОДАВАННЯ ЗНИЖОК
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
                dataset.Clear();
                adapter.Fill(dataset);
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
            try
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();
                pictureBox2.Image = Image.FromFile(openFileDialog.FileName);
                ReadImageToBytes(openFileDialog.FileName);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch
            {
                MessageBox.Show("Невірний формат зображення!");
            }

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
                command.CommandText = "(SELECT Id_type FROM type_of_car WHERE type_name like @type_name)";
                command.Parameters.AddWithValue("@type_name", metroComboBox1.Text);
                string type = command.ExecuteScalar().ToString();


                command.CommandText = "(SELECT Id_brand FROM cars_brand WHERE brand_name like @brand_name)";
                command.Parameters.AddWithValue("@brand_name", brand_name);
                int i = Convert.ToInt32(command.ExecuteScalar());
                command.Parameters.Clear();
                if (i == 0)
                {
                    command.CommandText = "insert into cars_brand (brand_name) values(@brand_name)";
                    command.Parameters.AddWithValue("@brand_name", brand_name);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }

                command.CommandText = "(SELECT Id_brand FROM cars_brand WHERE brand_name like @brand_name)";
                command.Parameters.AddWithValue("@brand_name", brand_name);
                string brand = command.ExecuteScalar().ToString();

                command.CommandText = "insert into cars (Id_type, car_model, year, cost, price, picture,Id_brand) values (@type,@model,@year,@cost,@price,@picture,@brand)";




                command.Parameters.Add("@type", SqlDbType.Int);
                command.Parameters.Add("@brand", SqlDbType.Int);
                command.Parameters.Add("@model", SqlDbType.VarChar, 20);
                command.Parameters.Add("@year", SqlDbType.Int);
                command.Parameters.Add("@cost", SqlDbType.Int);
                command.Parameters.Add("@price", SqlDbType.Int);
                command.Parameters.Add("@picture", SqlDbType.VarBinary);

                // Передаємо дані в команду через параметри
                command.Parameters["@type"].Value = type;
                command.Parameters["@brand"].Value = brand;
                command.Parameters["@model"].Value = model;
                command.Parameters["@year"].Value = year;
                command.Parameters["@cost"].Value = cost;
                command.Parameters["@price"].Value = price;
                command.Parameters["@picture"].Value = arr;
                command.ExecuteNonQuery();
                MessageBox.Show("Дані додано!");

                metroTextBox1.Clear();
                metroTextBox2.Clear();
                metroTextBox3.Clear();
                metroTextBox4.Clear();
                pictureBox2.Image = null;

                dataset.Clear();
                adapter.Fill(dataset);

            }

        }
        #endregion

        #region ВСІ ДЕЛІТИ
        private void button8_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    adapter.DeleteCommand = new SqlCommand("DELETE FROM cars WHERE Id_car=@id", connection);
                    int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    adapter.DeleteCommand.Parameters.AddWithValue("@id", id);

                    adapter.DeleteCommand.ExecuteNonQuery();
                    dataset.Clear();
                    adapter.Fill(dataset);
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    adapter.DeleteCommand = new SqlCommand("DELETE FROM penalties WHERE Id_penalty=@id", connection);
                    int id = int.Parse(dataGridView2.CurrentRow.Cells[0].Value.ToString());
                    adapter.DeleteCommand.Parameters.AddWithValue("@id", id);

                    adapter.DeleteCommand.ExecuteNonQuery();
                    dataset.Clear();
                    adapter.Fill(dataset);
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    adapter.DeleteCommand = new SqlCommand("DELETE FROM discounts WHERE Id_discount=@id", connection);
                    int id = int.Parse(dataGridView3.CurrentRow.Cells[0].Value.ToString());
                    adapter.DeleteCommand.Parameters.AddWithValue("@id", id);

                    adapter.DeleteCommand.ExecuteNonQuery();
                    dataset.Clear();
                    adapter.Fill(dataset);
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                    connection.Open();
                    adapter.DeleteCommand = new SqlCommand("DELETE FROM client WHERE id_client = @id", connection);
                    int id = int.Parse(dataGridView4.CurrentRow.Cells[0].Value.ToString());
                    adapter.DeleteCommand.Parameters.AddWithValue("@id", id);

                    adapter.DeleteCommand.ExecuteNonQuery();
                    dataset.Clear();
                    adapter.Fill(dataset);
                    MessageBox.Show("Клієнта видалено");

            }

        }
        #endregion

        #region ВИВЕСТИ ЗАМОВЛЕННЯ ДЛЯ ЗАКРИТТЯ
        private void button14_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "close_order";
                command.Parameters.AddWithValue("@E_mail", metroTextBox9.Text);
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
                if(index == 0)
                {
                    index++;
                }
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

                        command.CommandText = "(SELECT amount_penalty FROM penalties WHERE penalty_name = @penalty_name)";
                        command.Parameters.AddWithValue("@penalty_name", curItemString);

                        SqlDataReader thisReader = command.ExecuteReader();
                        while (thisReader.Read())
                        {

                            summ += Convert.ToInt32(thisReader["amount_penalty"]);

                        }
                    }
                }
                label28.Text = summ.ToString();
            }
            else
            {
                label28.Text = "0";
            }

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
        #endregion

        #region ЗАКРИТИ ЗАМОВЛЕННЯ
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

                    command.CommandText = "(SELECT Id_penalty FROM penalties WHERE penalty_name = @penalty_name)";
                    command.Parameters.AddWithValue("@penalty_name", curItemString);

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
                        command.Parameters.AddWithValue("@Id_penalty", pen);
                        command.Parameters.AddWithValue("@Id_rent", rent);

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                        double amount = Convert.ToDouble(label16.Text);
                        command.CommandText = "UPDATE rent SET total_amount = @total_amount WHERE Id_rent = @Id_rent;";
                        command.Parameters.AddWithValue("@total_amount", Math.Round(amount));
                        command.Parameters.AddWithValue("@Id_rent", rent);

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
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
                    command.Parameters.AddWithValue("@Id_rent", rent);

                    command.ExecuteNonQuery();
                }
            }
            dataset.Clear();
            adapter.Fill(dataset);
            MessageBox.Show("Замовлення закрито!");
            ADMIN_PAGE admin = new ADMIN_PAGE();
            this.Close();
            admin.Show();


        }
        #endregion

        #region ВСІ АПДЕЙТИ
        private void button6_Click(object sender, EventArgs e)
        {
            string sqlExpression = "update_penalties";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // Вказуємо, що команда звертається до процедури
                command.CommandType = CommandType.StoredProcedure;
                int id = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);
                string penalty_name = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                int amount_penalty = Convert.ToInt32(dataGridView2.CurrentRow.Cells[2].Value);
                command.Parameters.AddWithValue("@Id_penalty", id);
                command.Parameters.AddWithValue("@penalty_name", penalty_name);
                command.Parameters.AddWithValue("@amount_penalty", amount_penalty);
                command.ExecuteNonQuery();
                MessageBox.Show("Данні змінено!");
            }
                           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string sqlExpression = "update_discounts";

            if (Convert.ToInt32(dataGridView3.CurrentRow.Cells[2].Value) < 100)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    // Вказуємо, що команда звертається до процедури
                    command.CommandType = CommandType.StoredProcedure;
                    int id = Convert.ToInt32(dataGridView3.CurrentRow.Cells[0].Value);
                    string discount_name = dataGridView3.CurrentRow.Cells[1].Value.ToString();
                    int discount_percent = Convert.ToInt32(dataGridView3.CurrentRow.Cells[2].Value);
                    command.Parameters.AddWithValue("@Id_discount", id);
                    command.Parameters.AddWithValue("@discount_name", discount_name);
                    command.Parameters.AddWithValue("@discount_percent", discount_percent);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данні змінено!");
                }
            }
            else
            {
                MessageBox.Show("Відсоток не може бути 100 і більше!");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string sqlExpression = "update_client";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // Вказуємо, що команда звертається до процедури
                command.CommandType = CommandType.StoredProcedure;
                int id = Convert.ToInt32(dataGridView4.CurrentRow.Cells[0].Value);
                string First_name = dataGridView4.CurrentRow.Cells[1].Value.ToString();
                string Last_name = dataGridView4.CurrentRow.Cells[2].Value.ToString();
                string Patronymic = dataGridView4.CurrentRow.Cells[3].Value.ToString();
                string E_mail = dataGridView4.CurrentRow.Cells[4].Value.ToString();
                string Phone = dataGridView4.CurrentRow.Cells[5].Value.ToString();

                command.Parameters.AddWithValue("@Id_client", id);
                command.Parameters.AddWithValue("@First_name", First_name);
                command.Parameters.AddWithValue("@Last_name", Last_name);
                command.Parameters.AddWithValue("@Patronymic", Patronymic);
                command.Parameters.AddWithValue("@E_mail", E_mail);
                command.Parameters.AddWithValue("@Phone", Phone);
                command.ExecuteNonQuery();
                MessageBox.Show("Данні змінено!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sqlExpression = "update_cars";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // Вказуємо, що команда звертається до процедури
                command.CommandType = CommandType.StoredProcedure;
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                int cost = Convert.ToInt32(dataGridView1.CurrentRow.Cells[5].Value);
                int price = Convert.ToInt32(dataGridView1.CurrentRow.Cells[6].Value);

                command.Parameters.AddWithValue("@Id_car", id);
                command.Parameters.AddWithValue("@cost", cost);
                command.Parameters.AddWithValue("@price", price);
                command.ExecuteNonQuery();
                MessageBox.Show("Данні змінено!");
            }
        }
        #endregion
        
        #region ВІДКРИТЯ ФОРМИ РЕДАГУВАННЯ ЗАМОВЛЕННЯ
        private void button13_Click(object sender, EventArgs e)
        {
            CHANGE_CURRENT_ORDER change_current_order = new CHANGE_CURRENT_ORDER();
            change_current_order.Id_rent = Convert.ToInt32(dataGridView5.CurrentRow.Cells[0].Value);
            change_current_order.Show();

        }
        #endregion


    }
}
