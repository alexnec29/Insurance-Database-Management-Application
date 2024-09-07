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
using System.Data;



namespace Proiect
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=locuinte.db;Version=3;";
        private int selectedId;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                CreateDatabase();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Initialization error: {ex.Message}");
            }
        }

        private void CreateDatabase()
        {
            string locuinteConnectionString = "Data Source=locuinte.db;Version=3;";
            using (var connection = new SQLiteConnection(locuinteConnectionString))
            {
                connection.Open();
                string sqlLocuinte = @"CREATE TABLE IF NOT EXISTS Locuinte (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Data_Expirarii DATE NULL,
                        CNP_CUI TEXT NULL,
                        Nume TEXT NULL,
                        Prenume TEXT NULL,
                        Adresa_Asigurata TEXT NULL,
                        Adresa_De_Domiciliu TEXT NULL,
                        NumarTelefon TEXT NULL,
                        Suprafata_m2 INTEGER NULL,
                        An_Constructie INTEGER NULL,
                        Nr_Camere INTEGER NULL,
                        Nr_Etaje INTEGER NULL,
                        Nr_Cladiri_La_Aceeasi_Adresa INTEGER NULL,
                        Material_Constructie TEXT NULL
                      );";
                using (var command = new SQLiteCommand(sqlLocuinte, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            string rcaConnectionString = "Data Source=rca.db;Version=3;";
            using (var connection = new SQLiteConnection(rcaConnectionString))
            {
                connection.Open();
                string sqlRca = @"CREATE TABLE IF NOT EXISTS RCA (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Data_Expirare_Polita DATE NULL,
                            Numar_Inmatriculare TEXT NULL,
                            Serie_Sasiu TEXT NULL,
                            CNP_CUI TEXT NULL,
                            Nume TEXT NULL,
                            Prenume TEXT NULL,
                            Judet TEXT NULL,
                            Localitate TEXT NULL,
                            Adresa TEXT NULL,
                            Data_Obtinere_Permis DATE NULL,
                            Marca TEXT NULL,
                            Capacitate_Cilindrica INT NULL,
                            Nr_locuri SMALLINT NULL, 
                            Masa_Maxima_Autorizata INT NULL,
                            Putere_Motor_kW SMALLINT NULL,
                            Tip_Combustibil TEXT NULL,
                            Model TEXT NULL,
                            Serie_CIV TEXT NULL,
                            An_Fabricatie SMALLINT NULL,
                            Nr_Km INT NULL,
                            Data_Primei_Inmatriculari DATE NULL,
                            Data_Expirare_ITP DATE NULL,
                            NumarTelefon TEXT NULL
                        );";
                using (var command = new SQLiteCommand(sqlRca, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            string conducatoriAutoConnectionString = "Data Source=rca.db;Version=3;";
            using (var connection = new SQLiteConnection(conducatoriAutoConnectionString))
            {
                connection.Open();
                string sqlConducatoriAuto = @"CREATE TABLE IF NOT EXISTS conducatoriAuto (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    NumarTelefon TEXT NULL,
                    Nume TEXT NULL,
                    Prenume TEXT NULL,
                    CNP_CUI TEXT NULL,
                    SerieCI TEXT NULL,
                    NumarCI TEXT NULL,
                    rcaID INT,
                    FOREIGN KEY (rcaID) REFERENCES RCA(ID)
                 );";
                using (var command = new SQLiteCommand(sqlConducatoriAuto, connection))
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
                if (connectionString == "Data Source=locuinte.db;Version=3;")
                {
                    selectQuery = "SELECT ID, Data_Expirarii, Nume, Prenume, Adresa_De_Domiciliu FROM Locuinte";
                }
                else if (connectionString == "Data Source=rca.db;Version=3;")
                {
                    selectQuery = "SELECT ID, Data_Expirare_Polita, Numar_Inmatriculare, Nume, Prenume FROM RCA";
                }

                SQLiteCommand command = new SQLiteCommand(selectQuery, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (connectionString == "Data Source=locuinte.db;Version=3;")
                {
                    dataTable.Columns["Data_Expirarii"].ColumnName = "Data Expirarii";
                    dataTable.Columns["Adresa_De_Domiciliu"].ColumnName = "Adresa de Domiciliu";
                }
                else if (connectionString == "Data Source=rca.db;Version=3;")
                {
                    dataTable.Columns["Data_Expirare_Polita"].ColumnName = "Data Expirarii Politei";
                    dataTable.Columns["Numar_Inmatriculare"].ColumnName = "Numar Inmatriculare";
                }

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

            RcaButton.Style = (Style)FindResource("HighlightedButtonStyle");
            LocuinteButton.ClearValue(System.Windows.Controls.Button.StyleProperty);
        }
        private void LocuinteButton_Click(object sender, RoutedEventArgs e)
        {
            connectionString = "Data Source=locuinte.db;Version=3;";
            LoadData();

            LocuinteButton.Style = (Style)FindResource("HighlightedButtonStyle");
            RcaButton.ClearValue(System.Windows.Controls.Button.StyleProperty);
        }

        private void AdaugaButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (connectionString == "Data Source=rca.db;Version=3;")
                {
                    AddWindow addWindow = new AddWindow();
                    if (addWindow.ShowDialog() == true)
                    {
                        LoadData();
                    }
                }
                else if (connectionString == "Data Source=locuinte.db;Version=3;")
                {
                    AddWindow2 addWindow2 = new AddWindow2();
                    if (addWindow2.ShowDialog() == true)
                    {
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void EditeazaButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId > 0)
            {
                if (connectionString == "Data Source=rca.db;Version=3;")
                {
                    AddWindow editWindow = new AddWindow(selectedId);
                    if (editWindow.ShowDialog() == true)
                    {
                        LoadData();
                    }
                }
                else if (connectionString == "Data Source=locuinte.db;Version=3;")
                {
                    AddWindow2 editWindow2 = new AddWindow2(selectedId);
                    if (editWindow2.ShowDialog() == true)
                    {
                        LoadData();
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a record to edit.");
            }
        }

        private void CautaButton_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow(connectionString);

            if (searchWindow.ShowDialog() == true)
            {
                DataGrid.ItemsSource = searchWindow.SearchResult.DefaultView;

                if (connectionString == "Data Source=locuinte.db;Version=3;")
                {
                    DataGrid.Columns[1].Header = "Data Expirarii";
                    DataGrid.Columns[4].Header = "Adresa de Domiciliu";
                }
                else if (connectionString == "Data Source=rca.db;Version=3;")
                {
                    DataGrid.Columns[1].Header = "Data Expirarii Politei";
                    DataGrid.Columns[2].Header = "Numar Inmatriculare";
                }
            }
        }

        private void StergeButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId > 0)
            {
                try
                {
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        string deleteQuery = "";
                        connection.Open();
                        if (connectionString == "Data Source=locuinte.db;Version=3;") deleteQuery = "DELETE FROM Locuinte WHERE ID = @ID";
                        else if (connectionString == "Data Source=rca.db;Version=3;") deleteQuery = "DELETE FROM RCA WHERE ID = @ID";
                        using (var command = new SQLiteCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", selectedId);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                System.Windows.MessageBox.Show("Data deleted successfully.");
                                LoadData(); // Reload data to reflect changes
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Failed to delete data.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a row to delete.");
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                var selectedRow = DataGrid.SelectedItem as DataRowView;

                if (selectedRow != null && selectedRow["ID"] != DBNull.Value)
                {
                    selectedId = Convert.ToInt32(selectedRow["ID"]);
                }
                else
                {
                    selectedId = -1;
                }
            }
        }

    }
}