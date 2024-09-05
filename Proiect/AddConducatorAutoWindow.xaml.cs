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
    public partial class AddConducatorAutoWindow : Window
    {
        private int rcaID;
        private string connectionString = "Data Source=rca.db;Version=3;";

        public AddConducatorAutoWindow(int rcaID)
        {
            InitializeComponent();
            this.rcaID = rcaID;
        }
        private void EnableForeignKeyConstraints(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
            {
                command.ExecuteNonQuery();
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string numarTelefon = NumarTelefonTextBox.Text;
                string nume = NumeTextBox.Text;
                string prenume = PrenumeTextBox.Text;
                string cnpCUI = CNPTextBox.Text;
                string serieCI = SerieCITextBox.Text;
                string numarCI = NumarCITextBox.Text;

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            EnableForeignKeyConstraints(connection);

                            string query = @"INSERT INTO conducatoriAuto (NumarTelefon, Nume, Prenume, CNP_CUI, SerieCI, NumarCI, rcaID) 
                             VALUES (@NumarTelefon, @Nume, @Prenume, @CNP_CUI, @SerieCI, @NumarCI, @rcaID)";
                            using (var command = new SQLiteCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@NumarTelefon", numarTelefon);
                                command.Parameters.AddWithValue("@Nume", nume);
                                command.Parameters.AddWithValue("@Prenume", prenume);
                                command.Parameters.AddWithValue("@CNP_CUI", cnpCUI);
                                command.Parameters.AddWithValue("@SerieCI", serieCI);
                                command.Parameters.AddWithValue("@NumarCI", numarCI);
                                command.Parameters.AddWithValue("@rcaID", rcaID);
                                command.ExecuteNonQuery();
                            }
                            transaction.Commit();
                            this.DialogResult = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            System.Windows.MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
