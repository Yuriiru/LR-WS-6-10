using System;
using System.Collections.Generic;
using System.ComponentModel;
using SD = System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lr_ws_6_10
{
    public partial class Abonent : Form
    {
        public Abonent()
        {
            InitializeComponent();
        }

        private SqlConnection sqlConnection = new SqlConnection(@"Data Source=Krimzon; Initial Catalog=avt; Integrated Security=True");
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

        public void openConnection()
        {
            if (sqlConnection.State == SD.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == SD.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        private void Insert_Button_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            string data = dateTime.ToString();
            string description = richTextBox1.Text.ToString();
            string statysValues = comboBox1.SelectedItem.ToString();


            openConnection();

            string commandString = $"insert into wuw(дата_создания, статус, описание_проблемы) values('{data}', '{statysValues}', '{description}')";
            SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);

            if (sqlCommand.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Заявка добавлена", "Успех");
            }
            else
            {
                MessageBox.Show("Заявка не добавлена", "Ошибка");
            }

            closeConnection();
        }
        private void Abonent_Load(object sender, EventArgs e)
        {

        }
    }
}
