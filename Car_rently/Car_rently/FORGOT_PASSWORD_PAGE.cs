using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_rently
{
    public partial class FORGOT_PASSWORD_PAGE : Form
    {
        public FORGOT_PASSWORD_PAGE()
        {
            InitializeComponent();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            //picmail.BackgroundImage = Properties.Resources.email;
            panel3.BackColor = Color.FromArgb(78, 184, 206);
            textBox3.ForeColor = Color.FromArgb(78, 184, 206);
        }

        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string password = "";

        #region ЗАКРИТТЯ ФОРМИ
        private void label2_Click(object sender, EventArgs e)
        {
            Form start_page = Application.OpenForms[0];
            start_page.Show();
            this.Close();
        }
        #endregion

        #region ВІДПРАВЛЕННЯ ЛИСТА НА ПОШТУ
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                string E_mail = textBox3.Text + metroComboBox1.Text;
                command.CommandText = "(select password from client where E_mail = @E_mail) ";
                command.Parameters.AddWithValue("@E_mail", E_mail);

                SqlDataReader thisReader = command.ExecuteReader();
                while (thisReader.Read())
                {
                    password = thisReader["password"].ToString();
                }
                connection.Close();
                connection.Open();

                command.CommandText = "(SELECT * FROM client WHERE E_mail =  @E_mail)";
                command.Parameters.AddWithValue("@E_mail", E_mail);
                int i = Convert.ToInt32(command.ExecuteScalar());
                if (i != 0)
                {
                    try
                    {
                        if (metroComboBox1.SelectedIndex == 0)
                        {
                            //Gmail

                            using (MailMessage mm = new MailMessage("Car_rently" + "<" + "Ваша пошта" + ">", E_mail))
                            {
                                mm.Subject = "Ваш пароль";
                                mm.Body = password;
                                using (SmtpClient sc = new SmtpClient("smtp.gmail.com", 587))
                                {
                                    sc.EnableSsl = true;
                                    sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    sc.UseDefaultCredentials = false;
                                    sc.Timeout = 30000;
                                    sc.Credentials = new NetworkCredential("Ваша пошта", "Ваш пароль");
                                    sc.Send(mm);
                                }
                            }
                        }



                        else if (metroComboBox1.SelectedIndex == 1)
                        {
                            //Gmail

                            using (MailMessage mm = new MailMessage("Car_rently" + "<" + "Ваша пошта" + ">", E_mail))
                            {
                                mm.Subject = "Ваш пароль";
                                mm.Body = password;
                                using (SmtpClient sc = new SmtpClient("smtp.mail.ru", 25))
                                {
                                    sc.EnableSsl = true;
                                    sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    sc.UseDefaultCredentials = false;
                                    sc.Timeout = 30000;
                                    sc.Credentials = new NetworkCredential("Ваша пошта", "Ваш пароль");
                                    sc.Send(mm);
                                }
                            }
                        }

                        else if (metroComboBox1.SelectedIndex == 2)
                        {
                            //Gmail

                            using (MailMessage mm = new MailMessage("Car_rently" + "<" + "Ваша пошта" + ">", E_mail))
                            {
                                mm.Subject = "Ваш пароль";
                                mm.Body = password;
                                using (SmtpClient sc = new SmtpClient("smtp.yandex.ru", 25))
                                {
                                    sc.EnableSsl = true;
                                    sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                                    sc.UseDefaultCredentials = false;
                                    sc.Timeout = 30000;
                                    sc.Credentials = new NetworkCredential("Ваша пошта", "Ваш пароль");
                                    sc.Send(mm);
                                }
                            }
                        }

                    }
                    catch
                    {
                        MessageBox.Show("Не всі поля заповненні!");
                    }

                }
                else { MessageBox.Show("Немає такої пошти в базі!"); }
            }
        }
        #endregion
    }
}
