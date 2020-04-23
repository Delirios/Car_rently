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
    public partial class CHANGE_CURRENT_ORDER: Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private int id_rent;
        public int Id_rent
        {
            get {return id_rent; }
            set { id_rent = value; }
        }

        public CHANGE_CURRENT_ORDER()
        {
            InitializeComponent();
        }



        private void button3_Click(object sender, EventArgs e)
        {

            string sqlExpression = "update_order";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // Вказуємо, що команда звертається до процедури
                command.CommandType = CommandType.StoredProcedure;
                int id = id_rent;
                int rental_days = Convert.ToInt32(label15.Text);
                int total_amount = Convert.ToInt32(label6.Text);
                command.Parameters.AddWithValue("@Id_rent", id);
                command.Parameters.AddWithValue("@return_date", metroDateTime2.Value);
                command.Parameters.AddWithValue("@rental_days", rental_days);
                command.Parameters.AddWithValue("@total_amount", total_amount);
                command.ExecuteNonQuery();
                MessageBox.Show("Данні змінено!");
            }

        }

        private void metroDateTime2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime start = metroDateTime1.Value;
                DateTime end = metroDateTime2.Value;
                TimeSpan result = (end - start).Duration();
                label15.Text = ($"{result.Days + 1}");

                int first = int.Parse(label15.Text);
                int second = int.Parse(label7.Text);
                int price = first * second;
                label6.Text = price.ToString();
            }
            catch
            {

            }

        }

        

        private void CHANGE_CURRENT_ORDER_Load(object sender, EventArgs e)
        {
            label27.Text = id_rent.ToString();
            string sqlExpression = "show_order";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DateTime dateTime = metroDateTime1.Value;
                metroDateTime2.MinDate = dateTime;
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // Вказуємо, що команда звертається до процедури
                command.CommandType = CommandType.StoredProcedure;
                int id = id_rent;

                command.Parameters.AddWithValue("@Id_rent", id);
                command.ExecuteNonQuery();
                SqlDataReader thisReader = command.ExecuteReader();
                while (thisReader.Read())
                {
                    label1.Text = thisReader["Last_name"].ToString();
                    label2.Text = thisReader["First_name"].ToString();
                    label3.Text = thisReader["Patronymic"].ToString();
                    label4.Text = thisReader["brand_name"].ToString();
                    label5.Text = thisReader["car_model"].ToString();
                    label7.Text = thisReader["price"].ToString();
                    label20.Text = thisReader["total_amount"].ToString();
                    metroDateTime1.Value = DateTime.Parse( thisReader["lease_date"].ToString());
                    metroDateTime2.Value = DateTime.Parse(thisReader["return_date"].ToString());
                    label15.Text = thisReader["rental_days"].ToString();
              
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
