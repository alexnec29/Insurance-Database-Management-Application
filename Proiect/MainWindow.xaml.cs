using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data.SqlClient;



namespace Proiect
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=locuinte.db;Version=3;";
        public MainWindow()
        {
            InitializeComponent();
            CreateDatabase();
        }

        private void CreateDatabase()
        {
            // Create the Locuinte database and table
            string locuinteConnectionString = "Data Source=locuinte.db;Version=3;";
            using (var connection = new SQLiteConnection(locuinteConnectionString))
            {
                connection.Open();
                string sqlLocuinte = @"CREATE TABLE IF NOT EXISTS Locuinte (
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                NumeClient TEXT NOT NULL,
                                Suma DOUBLE NOT NULL,
                                selected BOOL
                              );";
                using (var command = new SQLiteCommand(sqlLocuinte, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Create the RCA database and table
            string rcaConnectionString = "Data Source=rca.db;Version=3;";
            using (var connection = new SQLiteConnection(rcaConnectionString))
            {
                connection.Open();
                string sqlRca = @"CREATE TABLE IF NOT EXISTS RCA (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            NumeClient TEXT NOT NULL,
                            Suma DOUBLE NOT NULL,
                            selected BOOL
                          );";
                using (var command = new SQLiteCommand(sqlRca, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private void LoadData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "";
                if (connectionString == "Data Source=locuinte.db;Version=3;") selectQuery = "SELECT * FROM Locuinte";
                else if (connectionString == "Data Source=rca.db;Version=3;") selectQuery = "SELECT * FROM RCA";
                SQLiteCommand command = new SQLiteCommand(selectQuery, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);

                System.Data.DataTable dataTable = new System.Data.DataTable();
                adapter.Fill(dataTable);

                // Legați DataGrid-ul la datele din tabel
                DataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void RcaButton_Click(object sender, RoutedEventArgs e)
        {
            connectionString = "Data Source=rca.db;Version=3;";
            LoadData();
        }
        private void LocuinteButton_Click(object sender, RoutedEventArgs e)
        {
            connectionString = "Data Source=locuinte.db;Version=3;";
            LoadData();
        }

        private void AdaugaButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string insertQuery = "";

                        // Determine the correct table based on the connection string
                        if (connectionString.Contains("locuinte.db"))
                        {
                            insertQuery = "INSERT INTO Locuinte (NumeClient, Suma) VALUES (@NumeClient, @Suma)";
                        }
                        else if (connectionString.Contains("rca.db"))
                        {
                            insertQuery = "INSERT INTO RCA (NumeClient, Suma) VALUES (@NumeClient, @Suma)";
                        }

                        using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@NumeClient", addWindow.NumeClient);
                            command.Parameters.AddWithValue("@Suma", addWindow.Suma);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                System.Windows.MessageBox.Show("Data added successfully.");
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Failed to add data.");
                            }
                        }
                    }

                    // Reload the data in the main window
                    LoadData();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void EditeazaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CautaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StergeButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SorteazaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}