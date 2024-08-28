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
                        Data_Expirarii DATE,
                        NumarTelefon TEXT,
                        Nume TEXT,
                        Prenume TEXT,
                        CNP_CUI TEXT,
                        Adresa_Asigurata TEXT,
                        Adresa_De_Domiciliu TEXT,
                        Suprafata_m2 INT,  
                        An_Constructie INT,
                        Nr_Camere INT,
                        Nr_Etaje INT,
                        Nr_Cladiri_La_Aceeasi_Adresa INT,
                        Material_Constructie TEXT
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
                            Data_Expirare_Polita DATE,
                            Numar_Inmatriculare TEXT,
                            Serie_Sasiu TEXT,
                            CNP_CUI TEXT,
                            Nume TEXT,
                            Prenume TEXT,
                            Judet TEXT,
                            Localitate TEXT,
                            Adresa TEXT,
                            Data_Obtinere_Permis DATE,
                            Marca TEXT,
                            Capacitate_Cilindrica INT,
                            Nr_locuri SMALLINT, 
                            Masa_Maxima_Autorizata INT,
                            Putere_Motor_kW SMALLINT,
                            Tip_Combustibil TEXT,
                            Model TEXT,
                            Serie_CIV TEXT,
                            An_Fabricatie SMALLINT,
                            Nr_Km INT,
                            Data_Primei_Inmatriculari DATE,
                            Data_Expirare_ITP DATE
                            NumarTelefon TEXT
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
                if (connectionString == "Data Source=locuinte.db;Version=3;") selectQuery = "SELECT ID, Data_Expirarii, Nume, Prenume,Adresa_De_Domiciliu FROM Locuinte";
                else if (connectionString == "Data Source=rca.db;Version=3;") selectQuery = "SELECT ID, Data_Expirare_Polita, Numar_Inmatriculare, Nume, Prenume FROM RCA";
                SQLiteCommand command = new SQLiteCommand(selectQuery, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);

                System.Data.DataTable dataTable = new System.Data.DataTable();
                adapter.Fill(dataTable);

                if (connectionString == "Data Source=locuinte.db;Version=3;")
                {
                    dataTable.Columns["Data_Expirarii"].ColumnName = "Data expirarii";
                    dataTable.Columns["Adresa_De_Domiciliu"].ColumnName = "Adresa de domiciliu";

                }
                else if (connectionString == "Data Source=rca.db;Version=3;")
                {
                    dataTable.Columns["Data_Expirare_Polita"].ColumnName = "Data expirarii politei";
                    dataTable.Columns["Numar_Inmatriculare"].ColumnName = "Numar inmatriculare";
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
            if(connectionString == "Data Source=rca.db;Version=3;")
            {
                AddWindow addWindow = new AddWindow();
                if (addWindow.ShowDialog() == true)
                {
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string insertQuery = @"INSERT INTO RCA (
                                            Data_Expirare_Polita,
                                            Numar_Inmatriculare,
                                            Serie_Sasiu,
                                            CNP_CUI,
                                            Nume,
                                            Prenume,
                                            Judet,
                                            Localitate,
                                            Adresa,
                                            Data_Obtinere_Permis,
                                            Marca,
                                            Capacitate_Cilindrica,
                                            Nr_locuri,
                                            Masa_Maxima_Autorizata,
                                            Putere_Motor_kW,
                                            Tip_Combustibil,
                                            Model,
                                            Serie_CIV,
                                            An_Fabricatie,
                                            Nr_Km,
                                            Data_Primei_Inmatriculari,
                                            Data_Expirare_ITP
                                            NumarTelefon) 
                                        VALUES (
                                            @DataExpirarePolita,
                                            @NumarInmatriculare,
                                            @SerieSasiu,
                                            @CnpCui,
                                            @Nume,
                                            @Prenume,
                                            @Judet,
                                            @Localitate,
                                            @Adresa,
                                            @DataObtinerePermis,
                                            @Marca,
                                            @CapacitateCilindrica,
                                            @NrLocuri,
                                            @MasaMaximaAutorizata,
                                            @PutereMotorKw,
                                            @TipCombustibil,
                                            @Model,
                                            @SerieCiv,
                                            @AnFabricatie,
                                            @NrKm,
                                            @DataPrimeiInmatriculari,
                                            @DataExpirareITP,
                                            @NumarTelefon);";

                            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@DataExpirarePolita", addWindow.DataExpirarePolita);
                                command.Parameters.AddWithValue("@NumarInmatriculare", addWindow.NumarInmatriculare);
                                command.Parameters.AddWithValue("@SerieSasiu", addWindow.SerieSasiu);
                                command.Parameters.AddWithValue("@CnpCui", addWindow.CnpCui);
                                command.Parameters.AddWithValue("@Nume", addWindow.Nume);
                                command.Parameters.AddWithValue("@Prenume", addWindow.Prenume);
                                command.Parameters.AddWithValue("@Judet", addWindow.Judet);
                                command.Parameters.AddWithValue("@Localitate", addWindow.Localitate);
                                command.Parameters.AddWithValue("@Adresa", addWindow.Adresa);
                                command.Parameters.AddWithValue("@DataObtinerePermis", addWindow.DataObtinerePermis);
                                command.Parameters.AddWithValue("@Marca", addWindow.Marca);
                                command.Parameters.AddWithValue("@CapacitateCilindrica", addWindow.CapacitateCilindrica);
                                command.Parameters.AddWithValue("@NrLocuri", addWindow.NrLocuri);
                                command.Parameters.AddWithValue("@MasaMaximaAutorizata", addWindow.MasaMaximaAutorizata);
                                command.Parameters.AddWithValue("@PutereMotorKw", addWindow.PutereMotorKw);
                                command.Parameters.AddWithValue("@TipCombustibil", addWindow.TipCombustibil);
                                command.Parameters.AddWithValue("@Model", addWindow.Model);
                                command.Parameters.AddWithValue("@SerieCiv", addWindow.SerieCiv);
                                command.Parameters.AddWithValue("@AnFabricatie", addWindow.AnFabricatie);
                                command.Parameters.AddWithValue("@NrKm", addWindow.NrKm);
                                command.Parameters.AddWithValue("@DataPrimeiInmatriculari", addWindow.DataPrimeiInmatriculari);
                                command.Parameters.AddWithValue("@DataExpirareITP", addWindow.DataExpirareITP);
                                command.Parameters.AddWithValue("@NumarTelefon", addWindow.NumarTelefon);

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
            else if(connectionString == "Data Source=locuinte.db;Version=3;")
            {
                AddWindow2 addWindow2 = new AddWindow2();
                if (addWindow2.ShowDialog() == true)
                {
                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string insertQuery = @"INSERT INTO Locuinte (
                                            Data_Expirarii,
                                            NumarTelefon,
                                            Nume,
                                            Prenume,
                                            CNP_CUI,
                                            Adresa_Asigurata,
                                            Adresa_De_Domiciliu,
                                            Suprafata_m2,  
                                            An_Constructie,
                                            Nr_Camere,
                                            Nr_Etaje,
                                            Nr_Cladiri_La_Aceeasi_Adresa,
                                            Material_Constructie) 
                                        VALUES (
                                            @Data_Expirarii,
                                            @NumarTelefon,
                                            @Nume,
                                            @Prenume,
                                            @CNP_CUI,
                                            @Adresa_Asigurata,
                                            @Adresa_De_Domiciliu,
                                            @Suprafata_m2,  
                                            @An_Constructie,
                                            @Nr_Camere,
                                            @Nr_Etaje,
                                            @Nr_Cladiri_La_Aceeasi_Adresa,
                                            @Material_Constructie);";

                            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@DataExpirarii", addWindow2.DataExpirarii);
                                command.Parameters.AddWithValue("@NumarTelefon", addWindow2.NumarTelefon);
                                command.Parameters.AddWithValue("@Nume", addWindow2.Nume);
                                command.Parameters.AddWithValue("@CNP_CUI", addWindow2.CNP_CUI);
                                command.Parameters.AddWithValue("@Adresa_Asigurata", addWindow2.Adresa_Asigurata);
                                command.Parameters.AddWithValue("@Adresa_De_Domiciliu", addWindow2.Adresa_De_Domiciliu);
                                command.Parameters.AddWithValue("@Suprafata_m2", addWindow2.Suprafata_m2);
                                command.Parameters.AddWithValue("@An_Constructie", addWindow2.An_Constructie);
                                command.Parameters.AddWithValue("@Nr_Camere", addWindow2.Nr_Camere);
                                command.Parameters.AddWithValue("@Nr_Etaje", addWindow2.Nr_Etaje);
                                command.Parameters.AddWithValue("@Nr_Cladiri_La_Aceeasi_Adresa", addWindow2.Nr_Cladiri_La_Aceeasi_Adresa);
                                command.Parameters.AddWithValue("@Material_Constructie", addWindow2.Material_Constructie);

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
        }

        private void EditeazaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CautaButton_Click(object sender, RoutedEventArgs e)
        {

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

        private void SorteazaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                // Assuming ID is an integer column in the database
                var selectedRow = (DataRowView)DataGrid.SelectedItem;
                selectedId = Convert.ToInt32(selectedRow["ID"]); // Update with the appropriate column name
            }
        }

    }
}