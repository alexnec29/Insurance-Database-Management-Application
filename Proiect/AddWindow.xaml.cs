using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Proiect
{
    public partial class AddWindow : Window
    {
        private int? _editId = null;
        private int rcaID;
        private string connectionString = "Data Source=rca.db;Version=3;";
        private List<Driver> drivers;
        public AddWindow()
        {
            InitializeComponent();
            ConducatoriAutoStackPanel.Visibility = Visibility.Collapsed;
        }
        public AddWindow(int id)
        {
            InitializeComponent();
            _editId = id;
            LoadDataForEdit(_editId.Value);
            LoadDrivers();
        }

        public DateTime? DataExpirarePolita { get; set; }
        public string NumarInmatriculare { get; private set; }
        public string SerieSasiu { get; private set; }
        public string CnpCui { get; private set; }
        public string Nume { get; private set; }
        public string Prenume { get; private set; }
        public string Judet { get; private set; }
        public string Localitate { get; private set; }
        public string Adresa { get; private set; }
        public DateTime? DataObtinerePermis { get; private set; }
        public string Marca { get; private set; }
        public int CapacitateCilindrica { get; private set; }
        public int NrLocuri { get; private set; }
        public int MasaMaximaAutorizata { get; private set; }
        public int PutereMotorKw { get; private set; }
        public string TipCombustibil { get; private set; }
        public string Model { get; private set; }
        public string SerieCiv { get; private set; }
        public int AnFabricatie { get; private set; }
        public int NrKm { get; private set; }
        public DateTime? DataPrimeiInmatriculari { get; private set; }
        public DateTime? DataExpirareITP { get; private set; }
        public string NumarTelefon { get; private set; }

        private void LoadDataForEdit(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rca.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT * FROM RCA WHERE ID = @ID";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Assuming the fields are named the same as the database columns
                            DataExpirarePolitaDatePicker.SelectedDate = DateTime.Parse(reader["Data_Expirare_Polita"].ToString());
                            NumarInmatriculareTextBox.Text = reader["Numar_Inmatriculare"].ToString();
                            SerieSasiuTextBox.Text = reader["Serie_Sasiu"].ToString();
                            CnpCuiTextBox.Text = reader["CNP_CUI"].ToString();
                            NumeTextBox.Text = reader["Nume"].ToString();
                            PrenumeTextBox.Text = reader["Prenume"].ToString();
                            JudetTextBox.Text = reader["Judet"].ToString();
                            LocalitateTextBox.Text = reader["Localitate"].ToString();
                            AdresaTextBox.Text = reader["Adresa"].ToString();
                            DataObtinerePermisDatePicker.SelectedDate = DateTime.Parse(reader["Data_Obtinere_Permis"].ToString());
                            MarcaTextBox.Text = reader["Marca"].ToString();
                            CapacitateCilindricaTextBox.Text = reader["Capacitate_Cilindrica"].ToString();
                            NrLocuriTextBox.Text = reader["Nr_locuri"].ToString();
                            MasaMaximaAutorizataTextBox.Text = reader["Masa_Maxima_Autorizata"].ToString();
                            PutereMotorKwTextBox.Text = reader["Putere_Motor_kW"].ToString();
                            TipCombustibilTextBox.Text = reader["Tip_Combustibil"].ToString();
                            ModelTextBox.Text = reader["Model"].ToString();
                            SerieCivTextBox.Text = reader["Serie_CIV"].ToString();
                            AnFabricatieTextBox.Text = reader["An_Fabricatie"].ToString();
                            NrKmTextBox.Text = reader["Nr_Km"].ToString();
                            DataPrimeiInmatriculariDatePicker.SelectedDate = DateTime.Parse(reader["Data_Primei_Inmatriculari"].ToString());
                            DataExpirareITPDatePicker.SelectedDate = DateTime.Parse(reader["Data_Expirare_ITP"].ToString());
                            NumarTelefonTextBox.Text = reader["NumarTelefon"].ToString();
                        }
                    }
                }
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataExpirarePolita = DataExpirarePolitaDatePicker.SelectedDate;
                NumarInmatriculare = NumarInmatriculareTextBox.Text;
                SerieSasiu = SerieSasiuTextBox.Text;
                CnpCui = CnpCuiTextBox.Text;
                Nume = NumeTextBox.Text;
                Prenume = PrenumeTextBox.Text;
                Judet = JudetTextBox.Text;
                Localitate = LocalitateTextBox.Text;
                Adresa = AdresaTextBox.Text;
                DataObtinerePermis = DataObtinerePermisDatePicker.SelectedDate;
                Marca = MarcaTextBox.Text;
                CapacitateCilindrica = int.Parse(CapacitateCilindricaTextBox.Text);
                NrLocuri = int.Parse(NrLocuriTextBox.Text);
                MasaMaximaAutorizata = int.Parse(MasaMaximaAutorizataTextBox.Text);
                PutereMotorKw = int.Parse(PutereMotorKwTextBox.Text);
                TipCombustibil = TipCombustibilTextBox.Text;
                Model = ModelTextBox.Text;
                SerieCiv = SerieCivTextBox.Text;
                AnFabricatie = int.Parse(AnFabricatieTextBox.Text);
                NrKm = int.Parse(NrKmTextBox.Text);
                DataPrimeiInmatriculari = DataPrimeiInmatriculariDatePicker.SelectedDate;
                DataExpirareITP = DataExpirareITPDatePicker.SelectedDate;
                NumarTelefon = NumarTelefonTextBox.Text;

                if (_editId.HasValue)
                {
                    UpdateRecord();
                }
                else
                {
                    InsertRecord();
                }

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Please enter valid data.\nError: {ex.Message}", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateRecord()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rca.db;Version=3;"))
            {
                connection.Open();
                string updateQuery = @"UPDATE RCA SET 
                                        Data_Expirare_Polita = @DataExpirarePolita,
                                        Numar_Inmatriculare = @NumarInmatriculare,
                                        Serie_Sasiu = @SerieSasiu,
                                        CNP_CUI = @CnpCui,
                                        Nume = @Nume,
                                        Prenume = @Prenume,
                                        Judet = @Judet,
                                        Localitate = @Localitate,
                                        Adresa = @Adresa,
                                        Data_Obtinere_Permis = @DataObtinerePermis,
                                        Marca = @Marca,
                                        Capacitate_Cilindrica = @CapacitateCilindrica,
                                        Nr_locuri = @NrLocuri,
                                        Masa_Maxima_Autorizata = @MasaMaximaAutorizata,
                                        Putere_Motor_kW = @PutereMotorKw,
                                        Tip_Combustibil = @TipCombustibil,
                                        Model = @Model,
                                        Serie_CIV = @SerieCiv,
                                        An_Fabricatie = @AnFabricatie,
                                        Nr_Km = @NrKm,
                                        Data_Primei_Inmatriculari = @DataPrimeiInmatriculari,
                                        Data_Expirare_ITP = @DataExpirareITP,
                                        NumarTelefon = @NumarTelefon
                                    WHERE ID = @ID";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", _editId.Value);
                    command.Parameters.AddWithValue("@DataExpirarePolita", DataExpirarePolita);
                    command.Parameters.AddWithValue("@NumarInmatriculare", NumarInmatriculare);
                    command.Parameters.AddWithValue("@SerieSasiu", SerieSasiu);
                    command.Parameters.AddWithValue("@CnpCui", CnpCui);
                    command.Parameters.AddWithValue("@Nume", Nume);
                    command.Parameters.AddWithValue("@Prenume", Prenume);
                    command.Parameters.AddWithValue("@Judet", Judet);
                    command.Parameters.AddWithValue("@Localitate", Localitate);
                    command.Parameters.AddWithValue("@Adresa", Adresa);
                    command.Parameters.AddWithValue("@DataObtinerePermis", DataObtinerePermis);
                    command.Parameters.AddWithValue("@Marca", Marca);
                    command.Parameters.AddWithValue("@CapacitateCilindrica", CapacitateCilindrica);
                    command.Parameters.AddWithValue("@NrLocuri", NrLocuri);
                    command.Parameters.AddWithValue("@MasaMaximaAutorizata", MasaMaximaAutorizata);
                    command.Parameters.AddWithValue("@PutereMotorKw", PutereMotorKw);
                    command.Parameters.AddWithValue("@TipCombustibil", TipCombustibil);
                    command.Parameters.AddWithValue("@Model", Model);
                    command.Parameters.AddWithValue("@SerieCiv", SerieCiv);
                    command.Parameters.AddWithValue("@AnFabricatie", AnFabricatie);
                    command.Parameters.AddWithValue("@NrKm", NrKm);
                    command.Parameters.AddWithValue("@DataPrimeiInmatriculari", DataPrimeiInmatriculari);
                    command.Parameters.AddWithValue("@DataExpirareITP", DataExpirareITP);
                    command.Parameters.AddWithValue("@NumarTelefon", NumarTelefon);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void InsertRecord()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=rca.db;Version=3;"))
            {
                connection.Open();
                string insertQuery = @"INSERT INTO RCA 
                                        (Data_Expirare_Polita, Numar_Inmatriculare, Serie_Sasiu, CNP_CUI, Nume, Prenume, Judet, Localitate, Adresa, Data_Obtinere_Permis, Marca, Capacitate_Cilindrica, Nr_locuri, Masa_Maxima_Autorizata, Putere_Motor_kW, Tip_Combustibil, Model, Serie_CIV, An_Fabricatie, Nr_Km, Data_Primei_Inmatriculari, Data_Expirare_ITP, NumarTelefon) 
                                    VALUES 
                                        (@DataExpirarePolita, @NumarInmatriculare, @SerieSasiu, @CnpCui, @Nume, @Prenume, @Judet, @Localitate, @Adresa, @DataObtinerePermis, @Marca, @CapacitateCilindrica, @NrLocuri, @MasaMaximaAutorizata, @PutereMotorKw, @TipCombustibil, @Model, @SerieCiv, @AnFabricatie, @NrKm, @DataPrimeiInmatriculari, @DataExpirareITP, @NumarTelefon)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@DataExpirarePolita", DataExpirarePolita);
                    command.Parameters.AddWithValue("@NumarInmatriculare", NumarInmatriculare);
                    command.Parameters.AddWithValue("@SerieSasiu", SerieSasiu);
                    command.Parameters.AddWithValue("@CnpCui", CnpCui);
                    command.Parameters.AddWithValue("@Nume", Nume);
                    command.Parameters.AddWithValue("@Prenume", Prenume);
                    command.Parameters.AddWithValue("@Judet", Judet);
                    command.Parameters.AddWithValue("@Localitate", Localitate);
                    command.Parameters.AddWithValue("@Adresa", Adresa);
                    command.Parameters.AddWithValue("@DataObtinerePermis", DataObtinerePermis);
                    command.Parameters.AddWithValue("@Marca", Marca);
                    command.Parameters.AddWithValue("@CapacitateCilindrica", CapacitateCilindrica);
                    command.Parameters.AddWithValue("@NrLocuri", NrLocuri);
                    command.Parameters.AddWithValue("@MasaMaximaAutorizata", MasaMaximaAutorizata);
                    command.Parameters.AddWithValue("@PutereMotorKw", PutereMotorKw);
                    command.Parameters.AddWithValue("@TipCombustibil", TipCombustibil);
                    command.Parameters.AddWithValue("@Model", Model);
                    command.Parameters.AddWithValue("@SerieCiv", SerieCiv);
                    command.Parameters.AddWithValue("@AnFabricatie", AnFabricatie);
                    command.Parameters.AddWithValue("@NrKm", NrKm);
                    command.Parameters.AddWithValue("@DataPrimeiInmatriculari", DataPrimeiInmatriculari);
                    command.Parameters.AddWithValue("@DataExpirareITP", DataExpirareITP);
                    command.Parameters.AddWithValue("@NumarTelefon", NumarTelefon);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void LoadDrivers()
        {
            drivers = new List<Driver>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM conducatoriAuto WHERE rcaID = @rcaID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@rcaID", rcaID);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            drivers.Add(new Driver
                            {
                                ID = reader.GetInt32(0),
                                Nume = reader.GetString(2),
                                Prenume = reader.GetString(3),
                                CNP_CUI = reader.GetString(4),
                            });
                        }
                    }
                }
            }
            ConducatoriAutoListBox.ItemsSource = drivers;
        }

        private void AdaugaConducatorAuto_Click(object sender, RoutedEventArgs e)
        {
            var addConducatorAutoWindow = new AddConducatorAutoWindow(rcaID);
            if (addConducatorAutoWindow.ShowDialog() == true)
            {
                LoadDrivers();
            }
        }

        private void ViewConducatorAuto_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
            //Driver selectedDriver = (Driver)((StackPanel)btn.Parent).DataContext;
            //var viewConducatorAutoWindow = new ViewConducatorAutoWindow(selectedDriver.ID);
            //viewConducatorAutoWindow.ShowDialog();
        }

        private void DeleteConducatorAuto_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            Driver selectedDriver = (Driver)((StackPanel)btn.Parent).DataContext;

            MessageBoxResult result = System.Windows.MessageBox.Show($"Are you sure you want to delete {selectedDriver.Nume} {selectedDriver.Prenume}?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM conducatoriAuto WHERE ID = @driverID";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@driverID", selectedDriver.ID);
                        command.ExecuteNonQuery();
                    }
                }
                LoadDrivers();
            }
        }

        private void ConducatoriAutoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    public class Driver
    {
        public int ID { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string CNP_CUI { get; set; }
    }
}