using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace lr_ws_6_10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string ConnectionString = @"Data Source=Krimzon; Initial Catalog=avt; Integrated Security=True";
        // private SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-09DA1D0; Initial Catalog=users-lr-ws; Integrated Security=True");
        // SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

        private void registrationButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string name = textBox2.Text;
            string lastName = textBox1.Text;
            string confirmPassword = confirmPasswordTextBox.Text;
            string role = "Абонент";
            
            if (password == confirmPassword)
            {
                string hashedPassword = HashPassword(password);

                bool registrationSuccessful = RegisterUser(username, hashedPassword, name, lastName, role);

                if (registrationSuccessful)
                {
                    MessageBox.Show("Регистрация успешно завершена.");
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при регистрации.");
                }
            }
            else
            {
                MessageBox.Show("Пароли не совпадают.");
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

        

        private static bool RegisterUser(string username, string hashedPassword, string name, string lastName, string role)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO avt_reg (логин, пароль, имя, фамилия, роль) VALUES (@Username, @Password, @Name, @LastName, @Role)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Role", role);
                        command.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации пользователя: " + ex.Message);
                return false;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Form f = new AuthorizationForm();
            f.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
