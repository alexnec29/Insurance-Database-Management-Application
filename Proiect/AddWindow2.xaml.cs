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
    public partial class AddWindow2 : Window
    {
        private int? _editId = null;
        public AddWindow2()
        {
            InitializeComponent();
        }
        public AddWindow2(int id)
        {
            InitializeComponent();
            _editId = id;
            LoadDataForEdit(_editId.Value);
        }
        public DateTime? Data_Expirarii { get; private set; }
        public string CNP_CUI { get; private set; }
        public string Nume { get; private set; }
        public string Prenume { get; private set; }
        public string Adresa_Asigurata { get; private set; }
        public string Adresa_De_Domiciliu { get; private set; }
        public string NumarTelefon { get; private set; }
        public int Suprafata_m2 { get; private set; }
        public int An_Constructie { get; private set; }
        public int Nr_Camere { get; private set; }
        public int Nr_Etaje { get; private set; }
        public int Nr_Cladiri_La_Aceeasi_Adresa { get; private set; }
        public string Material_Constructie { get; private set; }

        private void LoadDataForEdit(int id)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=locuinte.db;Version=3;"))
                {
                    connection.Open();
                    string query = "SELECT * FROM Locuinte WHERE ID = @ID";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DataExpirariiDatePicker.SelectedDate = reader["Data_Expirarii"] != DBNull.Value ? DateTime.Parse(reader["Data_Expirarii"].ToString()) : (DateTime?)null;
                                CNP_CUITextBox.Text = reader["CNP_CUI"]?.ToString();
                                NumeTextBox.Text = reader["Nume"]?.ToString();
                                PrenumeTextBox.Text = reader["Prenume"]?.ToString();
                                Adresa_AsigurataTextBox.Text = reader["Adresa_Asigurata"]?.ToString();
                                Adresa_De_DomiciliuTextBox.Text = reader["Adresa_De_Domiciliu"]?.ToString();
                                NumarTelefonTextBox.Text = reader["NumarTelefon"]?.ToString();
                                Suprafata_m2TextBox.Text = reader["Suprafata_m2"]?.ToString();
                                An_ConstructieTextBox.Text = reader["An_Constructie"]?.ToString();
                                Nr_CamereTextBox.Text = reader["Nr_Camere"]?.ToString();
                                Nr_EtajeTextBox.Text = reader["Nr_Etaje"]?.ToString();
                                Nr_Cladiri_La_Aceeasi_AdresaTextBox.Text = reader["Nr_Cladiri_La_Aceeasi_Adresa"]?.ToString();
                                Material_ConstructieTextBox.Text = reader["Material_Constructie"]?.ToString();
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("No record found with the provided ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                this.DialogResult = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = false;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Data_Expirarii = DataExpirariiDatePicker.SelectedDate;
                NumarTelefon = NumarTelefonTextBox.Text;
                CNP_CUI = CNP_CUITextBox.Text;
                Nume = NumeTextBox.Text;
                Prenume = PrenumeTextBox.Text;
                Adresa_Asigurata = Adresa_AsigurataTextBox.Text;
                Adresa_De_Domiciliu = Adresa_De_DomiciliuTextBox.Text;
                Suprafata_m2 = int.Parse(Suprafata_m2TextBox.Text);
                An_Constructie = int.Parse(An_ConstructieTextBox.Text);
                Nr_Camere = int.Parse(Nr_CamereTextBox.Text);
                Nr_Etaje = int.Parse(Nr_EtajeTextBox.Text);
                Nr_Cladiri_La_Aceeasi_Adresa = int.Parse(Nr_Cladiri_La_Aceeasi_AdresaTextBox.Text);
                Material_Constructie = Material_ConstructieTextBox.Text;
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
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=locuinte.db;Version=3;"))
            {
                connection.Open();
                string updateQuery = @"UPDATE Locuinte SET 
                                        Data_Expirarii = @Data_Expirarii,
                                        CNP_CUI = @CNP_CUI,
                                        Nume = @Nume,
                                        Prenume = @Prenume,
                                        Adresa_Asigurata = @Adresa_Asigurata,
                                        Adresa_De_Domiciliu = @Adresa_De_Domiciliu,
                                        NumarTelefon = @NumarTelefon,
                                        Suprafata_m2 = @Suprafata_m2,
                                        An_Constructie = @An_Constructie,
                                        Nr_Camere = @Nr_Camere,
                                        Nr_Etaje = @Nr_Etaje,
                                        Nr_Cladiri_La_Aceeasi_Adresa = @Nr_Cladiri_La_Aceeasi_Adresa,
                                        Material_Constructie = @Material_Constructie
                                    WHERE ID = @ID";

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", _editId.Value);
                    command.Parameters.AddWithValue("@Data_Expirarii", Data_Expirarii);
                    command.Parameters.AddWithValue("@CNP_CUI", CNP_CUI);
                    command.Parameters.AddWithValue("@Nume", Nume);
                    command.Parameters.AddWithValue("@Prenume", Prenume);
                    command.Parameters.AddWithValue("@Adresa_Asigurata", Adresa_Asigurata);
                    command.Parameters.AddWithValue("@Adresa_De_Domiciliu", Adresa_De_Domiciliu);
                    command.Parameters.AddWithValue("@NumarTelefon", NumarTelefon);
                    command.Parameters.AddWithValue("@Suprafata_m2", Suprafata_m2);
                    command.Parameters.AddWithValue("@An_Constructie", An_Constructie);
                    command.Parameters.AddWithValue("@Nr_Camere", Nr_Camere);
                    command.Parameters.AddWithValue("@Nr_Etaje", Nr_Etaje);
                    command.Parameters.AddWithValue("@Nr_Cladiri_La_Aceeasi_Adresa", Nr_Cladiri_La_Aceeasi_Adresa);
                    command.Parameters.AddWithValue("@Material_Constructie", Material_Constructie);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void InsertRecord()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=locuinte.db;Version=3;"))
            {
                connection.Open();
                string insertQuery = @"INSERT INTO Locuinte
                                        (Data_Expirarii, CNP_CUI, Nume, Prenume, Adresa_Asigurata, Adresa_De_Domiciliu, NumarTelefon, Suprafata_m2, An_Constructie, Nr_Camere, Nr_Etaje, Nr_Cladiri_La_Aceeasi_Adresa, Material_Constructie) 
                                    VALUES 
                                        (@Data_Expirarii, @CNP_CUI, @Nume, @Prenume, @Adresa_Asigurata, @Adresa_De_Domiciliu, @NumarTelefon, @Suprafata_m2, @An_Constructie, @Nr_Camere, @Nr_Etaje, @Nr_Cladiri_La_Aceeasi_Adresa, @Material_Constructie)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Data_Expirarii", Data_Expirarii);
                    command.Parameters.AddWithValue("@CNP_CUI", CNP_CUI);
                    command.Parameters.AddWithValue("@Nume", Nume);
                    command.Parameters.AddWithValue("@Prenume", Prenume);
                    command.Parameters.AddWithValue("@Adresa_Asigurata", Adresa_Asigurata);
                    command.Parameters.AddWithValue("@Adresa_De_Domiciliu", Adresa_De_Domiciliu);
                    command.Parameters.AddWithValue("@NumarTelefon", NumarTelefon);
                    command.Parameters.AddWithValue("@Suprafata_m2", Suprafata_m2);
                    command.Parameters.AddWithValue("@An_Constructie", An_Constructie);
                    command.Parameters.AddWithValue("@Nr_Camere", Nr_Camere);
                    command.Parameters.AddWithValue("@Nr_Etaje", Nr_Etaje);
                    command.Parameters.AddWithValue("@Nr_Cladiri_La_Aceeasi_Adresa", Nr_Cladiri_La_Aceeasi_Adresa);
                    command.Parameters.AddWithValue("@Material_Constructie", Material_Constructie);

                    command.ExecuteNonQuery();
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}