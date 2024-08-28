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
            // Create the Locuinte database and table
            string locuinteConnectionString = "Data Source=locuinte.db;Version=3;";
            using (var connection = new SQLiteConnection(locuinteConnectionString))
            {
                connection.Open();
                string sqlLocuinte = @"CREATE TABLE IF NOT EXISTS Locuinte (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        NumeClient TEXT NOT NULL,
                        Suma DOUBLE NOT NULL
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
                                        Data_Expirare_ITP) 
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
                                        @DataExpirareITP);";

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