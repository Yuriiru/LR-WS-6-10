using System;
using System.Collections.Generic;
using System.ComponentModel;
using SD = System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lr_ws_6_10
{
    public partial class OperatorForm : Form
    {
        public OperatorForm()
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

        private void OperatorForm_Load(object sender, EventArgs e)
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
    }
}

