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

        private const string ConnectionString = @"Data Source=DESKTOP-09DA1D0; Initial Catalog=users-lr-ws; Integrated Security=True";
        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            bool loginSuccessful = AuthenticateUser(username, password);

            if (loginSuccessful)
            {
                MessageBox.Show("Авторизация успешна.");
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.");
            }
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString("X2"));
                }

                return stringBuilder.ToString();
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

                    string query = "SELECT Password, Role FROM Users WHERE Username = @Username";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["Password"].ToString();
                                string role = reader["Role"].ToString();

                                if (hashedPassword == storedPassword)
                                {
                                    switch (role)
                                    {
                                        case "Абонент":
                                            // Логика для роли Абонент
                                            break;
                                        case "Администратор":
                                            // Логика для роли Администратор
                                            break;
                                        case "Оператор":
                                            // Логика для роли Оператор
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
    }

}
