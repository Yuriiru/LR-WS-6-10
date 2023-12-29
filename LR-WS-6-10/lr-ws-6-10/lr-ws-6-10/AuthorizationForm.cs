using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace lr_ws_6_10
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
        }

        private const string ConnectionString = @"Data Source=Krimzon; Initial Catalog=avt; Integrated Security=True";
        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            bool loginSuccessful= AuthenticateUser(username, password);

            if (loginSuccessful)
            {
                MessageBox.Show("Авторизация успешна.");
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.");
            }
        }

        public static string HashPassword(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private static bool AuthenticateUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT пароль, роль FROM avt_reg WHERE логин = @Username";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["пароль"].ToString();
                                string role = reader["роль"].ToString();

                                if (hashedPassword == storedPassword)
                                {
                                    switch (role)
                                    {
                                        case "Абонент":
                                            
                                            Abonent abonent = new Abonent();
                                            abonent.Show();
                                            break;
                                        case "Администратор":
                                            

                                            Admin admin = new Admin();
                                            admin.Show();
                                            break;
                                        case "Оператор":
                                            
                                            OperatorForm operatorForm =new OperatorForm();
                                            operatorForm.Show();
                                            break;
                                    }

                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при авторизации пользователя: " + ex.Message);
            }

            return false;
        }

        private void AuthorizationForm_Load(object sender, EventArgs e)
        {

        }
    }

}
