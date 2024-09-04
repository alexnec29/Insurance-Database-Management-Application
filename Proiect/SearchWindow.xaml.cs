using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace Proiect
{
    public partial class SearchWindow : Window
    {
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public DataTable SearchResult { get; private set; }

        private string connectionString;

        public SearchWindow(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            StartDate = StartDatePicker.SelectedDate;
            EndDate = EndDatePicker.SelectedDate;

            if (StartDate.HasValue && EndDate.HasValue)
            {
                PerformSearch();
            }
            else
            {
                System.Windows.MessageBox.Show("Please select both start and end dates.");
            }
        }

        private void PerformSearch()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "";

                if (connectionString == "Data Source=locuinte.db;Version=3;")
                {
                    query = @"SELECT ID, Data_Expirarii, Nume, Prenume, Adresa_De_Domiciliu 
                              FROM Locuinte 
                              WHERE Data_Expirarii BETWEEN @StartDate AND @EndDate";
                }
                else if (connectionString == "Data Source=rca.db;Version=3;")
                {
                    query = @"SELECT ID, Data_Expirare_Polita, Numar_Inmatriculare, Nume, Prenume 
                              FROM RCA 
                              WHERE Data_Expirare_Polita BETWEEN @StartDate AND @EndDate";
                }

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", StartDate.Value);
                    command.Parameters.AddWithValue("@EndDate", EndDate.Value);

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    SearchResult = new DataTable();
                    adapter.Fill(SearchResult);
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
                        System.Windows.MessageBox.Show("No records found for the selected date range.");
                    }
                }
            }
        }
    }
}
