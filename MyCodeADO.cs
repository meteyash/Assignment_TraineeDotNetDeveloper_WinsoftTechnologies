using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EmployeeManagementApp
{
    public partial class MainForm : Form
    {
        private string connectionString = "ConnectionString";

        private SqlConnection connection;

        public MainForm()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
            InitializeDatabase();
            LoadData();
        }
        private void InitializeDatabase()
        {
            try
            {
                connection.Open();
                SqlCommand createDbCommand = new SqlCommand("CREATE DATABASE IF NOT EXISTS EmployeeDB", connection);
                createDbCommand.ExecuteNonQuery();

                connection.ChangeDatabase("EmployeeDB");

                SqlCommand createTableCommand = new SqlCommand(
                    "CREATE TABLE IF NOT EXISTS Employee (" +
                    "Id INT PRIMARY KEY IDENTITY(1,1)," +
                    "Name NVARCHAR(50)," +
                    "Age INT," +
                    "Salary DECIMAL(18,2))", connection);

                createTableCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void LoadData()
        {
            try
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Employee", connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void UpdateEmployee(int id, string name, int age, decimal salary)
        {
            try
            {
                connection.Open();


                SqlCommand updateCommand = new SqlCommand("UpdateEmployee", connection);
                updateCommand.CommandType = CommandType.StoredProcedure;
                updateCommand.Parameters.AddWithValue("@Id", id);
                updateCommand.Parameters.AddWithValue("@Name", name);
                updateCommand.Parameters.AddWithValue("@Age", age);
                updateCommand.Parameters.AddWithValue("@Salary", salary);

                updateCommand.ExecuteNonQuery();

                LoadData(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating employee: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}
