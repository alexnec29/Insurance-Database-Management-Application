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
        private const string connectionString = "Data Source=asigurari.db;Version=3;";
        public MainWindow()
        {
            InitializeComponent();
            CreateDatabase();
            LoadData();
        }

        private void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = @"CREATE TABLE IF NOT EXISTS Asigurari (
                                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                        NumeClient TEXT NOT NULL,
                                        Suma DOUBLE NOT NULL,
                                        selected BOOL
                                    ); ";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        private void LoadData()
        {
            string connectionString = "Data Source=asigurari.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Asigurari";
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

        }
        private void LocuinteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AdaugaButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            if (addWindow.ShowDialog() == true)
            {
                string connectionString = "Data Source=asigurari.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Use the data entered in AddWindow
                    string insertQuery = "INSERT INTO Asigurari (NumeClient, Suma) VALUES (@NumeClient, @Suma)";
                    SQLiteCommand command = new SQLiteCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@NumeClient", addWindow.NumeClient);
                    command.Parameters.AddWithValue("@Suma", addWindow.Suma);

                    command.ExecuteNonQuery();
                }

                // Reload the data in the main window
                LoadData();
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
            EraseData();
        }

        private void EraseData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM asigurari WHERE @selected=";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void SorteazaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}