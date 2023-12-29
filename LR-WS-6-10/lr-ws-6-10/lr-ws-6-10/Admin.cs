using System;
using SD = System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;

namespace lr_ws_6_10
{
    public partial class Admin : Form
    {
        public Admin()
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
            openConnection();

            DateTime dateTime = DateTime.Now;
            string data = dateTime.ToString();
            string description = richTextBox1.Text.ToString();
            string statysValues = comboBox1.SelectedItem.ToString();

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

        private void AdminForm_Load(object sender, EventArgs e)
        {
            openConnection();

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM wuw", sqlConnection);

            SD.DataSet db = new SD.DataSet();

            dataAdapter.Fill(db);

            dataGridView1.DataSource = db.Tables[0];

            closeConnection();
        }

        private void RestoreTableButton_Click(object sender, EventArgs e)
        {
            openConnection();

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM wuw", sqlConnection);

            SD.DataSet db = new SD.DataSet();

            dataAdapter.Fill(db);

            dataGridView1.DataSource = db.Tables[0];

            closeConnection();
        }




        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowIndex = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[rowIndex];

                textBox1.Text = row.Cells["idзаписи"].Value.ToString();
                richTextBox1.Text = row.Cells["описание_проблемы"].Value.ToString();
                comboBox1.SelectedItem = row.Cells["статус"].Value.ToString();
            }
        }

        private void Delete_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить выделенную запись", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            int id = Convert.ToInt32(textBox1.Text);
            SqlCommand s_cmd = sqlConnection.CreateCommand();
            s_cmd.CommandText = $"DELETE FROM wuw WHERE idзаписи = {id}";

            openConnection();
            if (s_cmd.ExecuteNonQuery()>0)
            {
                MessageBox.Show("удаление успешно");
            }
            else
            {
                MessageBox.Show("удаление неудалось");
            }
            closeConnection();

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string statysValues = comboBox1.SelectedItem.ToString();
            string newText = richTextBox1.Text.ToString();
            int id = Convert.ToInt32(textBox1.Text);

            string query = "UPDATE wuw SET описание_проблемы = @описание_проблемы, статус = @статус  WHERE idзаписи = @idзаписи"; 
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.Parameters.AddWithValue("@статус", statysValues);
            command.Parameters.AddWithValue("@описание_проблемы", newText);
            command.Parameters.AddWithValue("@idзаписи", id);

            openConnection();
            
          
            if (command.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("запись обновлена успешно");
            }
            else
            {
                MessageBox.Show("обновление записи неудалось");
            }
            closeConnection();
        }
    }
}
