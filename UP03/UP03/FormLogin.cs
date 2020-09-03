using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UP03
{
    public partial class FormLogin : Form
    {
        static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=bakeryDB;Integrated Security=True";
        // Создание подключения
        SqlConnection connection = new SqlConnection(connectionString);
        int times = 0;
        int pause = 0;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPass.Text;
            string sql = "SELECT * FROM users WHERE login=@login" + " AND password=@password AND role='admin'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                // Add parameters to SQL query
                cmd.Parameters.Add("@login", SqlDbType.NVarChar, 50);
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50);
                cmd.Parameters["@login"].Value = login; // set parameters
                cmd.Parameters["@password"].Value = password; // to supplied values
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    FormEmployee formEmployee1 = new FormEmployee();
                    formEmployee1.Show();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                    times++;
                    if(times == 3)
                    {
                        times = 0;
                        pause += 15000;
                        button1.Enabled = false;
                        MessageBox.Show("Следующие попытки через "+ pause/1000 +" секунд.");
                        System.Threading.Thread.Sleep(pause);
                        button1.Enabled = true;
                    }
                }
            }
        }
    }
}
