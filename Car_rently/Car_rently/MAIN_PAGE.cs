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

        private void MAIN_PAGE_Load(object sender, EventArgs e)
        {
            string sql = "SELECT TOP(1) WITH TIES *FROM cars_brand ORDER BY ROW_NUMBER()OVER(PARTITION BY brand_name ORDER BY Id_brand); ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd1 = new SqlCommand(sql, connection);
                DataTable tbl1 = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(tbl1);
                metroComboBox1.DataSource = tbl1;
                metroComboBox1.DisplayMember = "brand_name";// столбец для отображения
                metroComboBox1.ValueMember = "Id_brand";//столбец с id
                metroComboBox1.SelectedIndex = -1;
            }
        }

    }
}
