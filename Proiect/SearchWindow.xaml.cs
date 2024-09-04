using System;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Proiect
{
    public partial class SearchWindow : Window
    {
        private string connectionString;
        public DataTable SearchResult { get; private set; }
        private Dictionary<string, string> fieldDataTypes;

        public SearchWindow(string dbConnectionString)
        {
            InitializeComponent();
            connectionString = dbConnectionString;
            InitializeFieldDataTypes();
            PopulateSearchFields();
        }

        private void InitializeFieldDataTypes()
        {
            fieldDataTypes = new Dictionary<string, string>();

            if (connectionString == "Data Source=locuinte.db;Version=3;")
            {
                fieldDataTypes.Add("Data_Expirarii", "Date");
                fieldDataTypes.Add("NumarTelefon", "Text");
                fieldDataTypes.Add("Nume", "Text");
                fieldDataTypes.Add("Prenume", "Text");
                fieldDataTypes.Add("CNP_CUI", "Text");
                fieldDataTypes.Add("Adresa_Asigurata", "Text");
                fieldDataTypes.Add("Adresa_De_Domiciliu", "Text");
                fieldDataTypes.Add("Suprafata_m2", "Text");
                fieldDataTypes.Add("An_Constructie", "Text");
                fieldDataTypes.Add("Nr_Camere", "Text");
                fieldDataTypes.Add("Nr_Etaje", "Text");
                fieldDataTypes.Add("Nr_Cladiri_La_Aceeasi_Adresa", "Text");
                fieldDataTypes.Add("Material_Constructie", "Text");
            }
            else if (connectionString == "Data Source=rca.db;Version=3;")
            {
                fieldDataTypes.Add("Data_Expirare_Polita", "Date");
                fieldDataTypes.Add("Numar_Inmatriculare", "Text");
                fieldDataTypes.Add("Serie_Sasiu", "Text");
                fieldDataTypes.Add("CNP_CUI", "Text");
                fieldDataTypes.Add("Nume", "Text");
                fieldDataTypes.Add("Prenume", "Text");
                fieldDataTypes.Add("Judet", "Text");
                fieldDataTypes.Add("Localitate", "Text");
                fieldDataTypes.Add("Adresa", "Text");
                fieldDataTypes.Add("Data_Obtinere_Permis", "Date");
                fieldDataTypes.Add("Marca", "Text");
                fieldDataTypes.Add("Capacitate_Cilindrica", "Text");
                fieldDataTypes.Add("Nr_locuri", "Text");
                fieldDataTypes.Add("Masa_Maxima_Autorizata", "Text");
                fieldDataTypes.Add("Putere_Motor_kW", "Text");
                fieldDataTypes.Add("Tip_Combustibil", "Text");
                fieldDataTypes.Add("Model", "Text");
                fieldDataTypes.Add("Serie_CIV", "Text");
                fieldDataTypes.Add("An_Fabricatie", "Text");
                fieldDataTypes.Add("Nr_Km", "Text");
                fieldDataTypes.Add("Data_Primei_Inmatriculari", "Date");
                fieldDataTypes.Add("Data_Expirare_ITP", "Date");
                fieldDataTypes.Add("NumarTelefon", "Text");
            }
        }
        private void PopulateSearchFields()
        {
            foreach (var field in fieldDataTypes.Keys)
            {
                SearchFieldComboBox.Items.Add(field);
            }
            SearchFieldComboBox.SelectedIndex = 0;
        }

        private void SearchFieldComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedField = SearchFieldComboBox.SelectedItem.ToString();
            if (fieldDataTypes[selectedField] == "Date")
            {
                DateRangePanel.Visibility = Visibility.Visible;
                SearchValueTextBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                DateRangePanel.Visibility = Visibility.Collapsed;
                SearchValueTextBox.Visibility = Visibility.Visible;
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectedField = SearchFieldComboBox.SelectedItem.ToString();
                string query = "";

                if (fieldDataTypes[selectedField] == "Date")
                {
                    DateTime? startDate = StartDatePicker.SelectedDate;
                    DateTime? endDate = EndDatePicker.SelectedDate;

                    if (startDate.HasValue && endDate.HasValue)
                    {
                        if (connectionString == "Data Source=locuinte.db;Version=3;")
                        {
                            query = $"SELECT ID, Data_Expirarii, Nume, Prenume, Adresa_De_Domiciliu FROM Locuinte WHERE {selectedField} BETWEEN @StartDate AND @EndDate";
                        }
                        else if (connectionString == "Data Source=rca.db;Version=3;")
                        {
                            query = $"SELECT ID, Data_Expirare_Polita, Numar_Inmatriculare, Nume, Prenume FROM RCA WHERE {selectedField} BETWEEN @StartDate AND @EndDate";
                        }

                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@StartDate", startDate.Value);
                            command.Parameters.AddWithValue("@EndDate", endDate.Value);
                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                            SearchResult = new DataTable();
                            adapter.Fill(SearchResult);
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Please select valid dates.");
                        return;
                    }
                }
                else
                {
                    string searchValue = SearchValueTextBox.Text;
                    if (connectionString == "Data Source=locuinte.db;Version=3;")
                    {
                        query = $"SELECT ID, Data_Expirarii, Nume, Prenume, Adresa_De_Domiciliu FROM Locuinte WHERE {selectedField} LIKE @SearchValue";
                    }
                    else if (connectionString == "Data Source=rca.db;Version=3;")
                    {
                        query = $"SELECT ID, Data_Expirare_Polita, Numar_Inmatriculare, Nume, Prenume FROM RCA WHERE {selectedField} LIKE @SearchValue";
                    }

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%");
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                        SearchResult = new DataTable();
                        adapter.Fill(SearchResult);
                    }
                }

                if (connectionString == "Data Source=locuinte.db;Version=3;")
                {
                    SearchResult.Columns["Data_Expirarii"].ColumnName = "Data Expirarii";
                    SearchResult.Columns["Adresa_De_Domiciliu"].ColumnName = "Adresa de Domiciliu";
                }
                else if (connectionString == "Data Source=rca.db;Version=3;")
                {
                    SearchResult.Columns["Data_Expirare_Polita"].ColumnName = "Data Expirarii Politei";
                    SearchResult.Columns["Numar_Inmatriculare"].ColumnName = "Numar Inmatriculare";
                }

                if (SearchResult.Rows.Count > 0)
                {
                    DialogResult = true;
                }
                else
                {
                    System.Windows.MessageBox.Show("No records found for the selected criteria.");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
