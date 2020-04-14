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
    public partial class ORDER_PAGE : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public string Last_name
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        public string First_name
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }

        public string Patronymic
        {
            get { return label3.Text; }
            set { label3.Text = value; }
        }
        public string Brand
        {
            get { return label4.Text; }
            set { label4.Text = value; }
        }
        public string Model
        {
            get { return label5.Text; }
            set { label5.Text = value; }
        }
        public string Price
        {
            get { return label7.Text; }
            set { label7.Text = value; }
        }
        private string e_mail;
        public string E_mail
        {
            get { return e_mail; }
            set { e_mail = value; }
        }
        private int id_car;
        public int Id_car
        {
            get {return id_car; }
            set { id_car = value; }
        }

        public ORDER_PAGE()
        {
            InitializeComponent();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandText = "(SELECT Id_client FROM client WHERE E_mail like '" + e_mail + "')";
                    int Id_client = Convert.ToInt32(command.ExecuteScalar());
                    int Id_discount = 0;
                    try
                    {
                        command.CommandText = "(SELECT Id_discount FROM discounts WHERE discount_name like'" + metroComboBox1.Text + "')";
                        Id_discount = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch
                    {
                    }

                    if (Convert.ToInt32(Id_discount) != 0)
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO rent (Id_client, Id_car, Id_discount, lease_date, return_date, rental_days, total_amount) values (@Id_client,@Id_car,@Id_discount,@lease_date,@return_date,@rental_days,@total_amount)";


                        command.Parameters.Add("@Id_client", SqlDbType.Int);
                        command.Parameters.Add("@Id_car", SqlDbType.Int);
                        command.Parameters.Add("@Id_discount", SqlDbType.Int);
                        command.Parameters.Add("@lease_date", SqlDbType.Date);
                        command.Parameters.Add("@return_date", SqlDbType.Date);
                        command.Parameters.Add("@rental_days", SqlDbType.Int);
                        command.Parameters.Add("@total_amount", SqlDbType.Float);

                        // передаем данные в команду через параметры
                        command.Parameters["@Id_client"].Value = Id_client;
                        command.Parameters["@Id_car"].Value = id_car;
                        command.Parameters["@Id_discount"].Value = Id_discount;
                        command.Parameters["@lease_date"].Value = metroDateTime1.Value;
                        command.Parameters["@return_date"].Value = metroDateTime2.Value;
                        command.Parameters["@rental_days"].Value = label15.Text;
                        command.Parameters["@total_amount"].Value = label20.Text;
                        command.ExecuteNonQuery();
                        MessageBox.Show("Авто заброньвано!");
                    }
                    else
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO rent (Id_client, Id_car,lease_date, return_date, rental_days, total_amount) values (@Id_client,@Id_car,@lease_date,@return_date,@rental_days,@total_amount)";

                        command.Parameters.Add("@Id_client", SqlDbType.Int);
                        command.Parameters.Add("@Id_car", SqlDbType.Int);
                        command.Parameters.Add("@lease_date", SqlDbType.Date);
                        command.Parameters.Add("@return_date", SqlDbType.Date);
                        command.Parameters.Add("@rental_days", SqlDbType.Int);
                        command.Parameters.Add("@total_amount", SqlDbType.Float);

                        // передаем данные в команду через параметры
                        command.Parameters["@Id_client"].Value = Id_client;
                        command.Parameters["@Id_car"].Value = id_car;
                        command.Parameters["@lease_date"].Value = metroDateTime1.Value;
                        command.Parameters["@return_date"].Value = metroDateTime2.Value;
                        command.Parameters["@rental_days"].Value = label15.Text;
                        command.Parameters["@total_amount"].Value = label20.Text;
                        command.ExecuteNonQuery();
                        MessageBox.Show("Авто заброньвано!");
                    }
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Некоректний ввід!");
            }

        }

            private void metroDateTime2_ValueChanged(object sender, EventArgs e)
        {
            DateTime start = metroDateTime1.Value;
            DateTime end = metroDateTime2.Value;
            TimeSpan result = (end - start).Duration();
            label15.Text = ($"{result.Days + 1}");

            int first = int.Parse($"{result.Days+1}");
            int second = int.Parse(label7.Text);
            int price = first * second;
            label6.Text = price.ToString();
            try
            {
                if (label18.Text == "0")
                {
                    label20.Text = label6.Text;
                }
                if (label20.Text != "")
                {
                    float percent = Convert.ToInt32(label18.Text);
                    float price_per_day = Convert.ToInt32(label6.Text);
                    float total_result = price * (percent / 100);
                    float total_price = price_per_day - total_result;
                    label20.Text = total_price.ToString();
                }
            }
            catch
            {

            }
            
        }

        
        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            metroDateTime1.MinDate = DateTime.Now;
            DateTime dateTime = metroDateTime1.Value;
            metroDateTime2.MinDate = dateTime;

            DateTime start = metroDateTime1.Value;
            DateTime end = metroDateTime2.Value;
            TimeSpan result = (end - start).Duration();
            label15.Text = ($"{result.Days + 1}");

            int first = int.Parse($"{result.Days + 1}");
            int second = int.Parse(label7.Text);
            int price = first * second;
            label6.Text = price.ToString();
            try
            {
                if (label18.Text == "0")
                {
                    label20.Text = label6.Text;
                }
                if (label20.Text != "")
                {
                    float percent = Convert.ToInt32(label18.Text);
                    float price_per_day = Convert.ToInt32(label6.Text);
                    float total_result = price * (percent / 100);
                    float total_price = price_per_day - total_result;
                    label20.Text = total_price.ToString();
                }
            }
            catch
            {

            }
        }

        private void ORDER_PAGE_Load(object sender, EventArgs e)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT COUNT(Id_rent_penalty) FROM rent_penalty rp JOIN rent r ON rp.Id_rent = r.Id_rent JOIN client c ON r.Id_client = c.Id_client WHERE c.E_mail = '" + e_mail + "'";
                count =Convert.ToInt32( command.ExecuteScalar());
            }
            if (count >= 5)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "SELECT * FROM discounts;";
                    SqlCommand cmd1 = new SqlCommand(sql, connection);
                    DataTable tbl1 = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(tbl1);
                    metroComboBox1.DataSource = tbl1;

                    metroComboBox1.DisplayMember = "discount_name";// столбец для отображения
                    metroComboBox1.ValueMember = "Id_discount";//столбец с id
                    metroComboBox1.SelectedIndex = -1;
                    label18.Text = "0";

                }
            }
            else
            {
                label26.Text = ("Замовте 5 авто, щоб отримати знижку постійного клієнта");
            }
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBox1.SelectedIndex != -1)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT discount_percent FROM discounts WHERE discount_name = '" + metroComboBox1.Text + "'";
                    SqlDataReader thisReader = command.ExecuteReader();
                    while (thisReader.Read())
                    {
                        label18.Text = thisReader["discount_percent"].ToString();

                    }
                }
            }
            /*/
            else
            {
                label18.Text = "";
            }/*/
            if (label18.Text != "")
            {
                try
                {
                    float percent = Convert.ToInt32(label18.Text);
                    float price_per_day = Convert.ToInt32(label6.Text);
                    float total_result = price_per_day * (percent  / 100);
                    float total_price = price_per_day - total_result; 
                    label20.Text = total_price.ToString();
                }
                catch
                {
                    //wMessageBox.Show("Не всі поля заповнені");
                }
            }





        }

        private void label19_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label22_Click(object sender, EventArgs e)
        {

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
    }
}
