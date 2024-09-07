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
        private int? conducatorAutoID;
        private string connectionString = "Data Source=rca.db;Version=3;";

        public AddConducatorAutoWindow(int rcaID, int? conducatorAutoID = null)
        {
            InitializeComponent();
            this.rcaID = rcaID;
            this.conducatorAutoID = conducatorAutoID;

            if (conducatorAutoID.HasValue)
            {
                LoadConducatorAutoData(conducatorAutoID.Value);
            }
        }

        private void LoadConducatorAutoData(int conducatorAutoID)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT NumarTelefon, Nume, Prenume, CNP_CUI, SerieCI, NumarCI FROM conducatoriAuto WHERE ID = @ID";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", conducatorAutoID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                NumarTelefonTextBox.Text = reader["NumarTelefon"].ToString();
                                NumeTextBox.Text = reader["Nume"].ToString();
                                PrenumeTextBox.Text = reader["Prenume"].ToString();
                                CNPTextBox.Text = reader["CNP_CUI"].ToString();
                                SerieCITextBox.Text = reader["SerieCI"].ToString();
                                NumarCITextBox.Text = reader["NumarCI"].ToString();
                            }
                            else
                            {
                                System.Windows.MessageBox.Show($"No data found for ID: {conducatorAutoID}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred while loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

                            string query;
                            if (conducatorAutoID.HasValue)
                            {
                                // Edit mode, update existing record
                                query = @"UPDATE conducatoriAuto SET 
                                  NumarTelefon = @NumarTelefon, 
                                  Nume = @Nume, 
                                  Prenume = @Prenume, 
                                  CNP_CUI = @CNP_CUI, 
                                  SerieCI = @SerieCI, 
                                  NumarCI = @NumarCI 
                                  WHERE ID = @ID";
                            }
                            else
                            {
                                // Add mode, insert new record
                                query = @"INSERT INTO conducatoriAuto 
                                  (NumarTelefon, Nume, Prenume, CNP_CUI, SerieCI, NumarCI, rcaID) 
                                  VALUES (@NumarTelefon, @Nume, @Prenume, @CNP_CUI, @SerieCI, @NumarCI, @rcaID)";
                            }

                            using (var command = new SQLiteCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@NumarTelefon", numarTelefon);
                                command.Parameters.AddWithValue("@Nume", nume);
                                command.Parameters.AddWithValue("@Prenume", prenume);
                                command.Parameters.AddWithValue("@CNP_CUI", cnpCUI);
                                command.Parameters.AddWithValue("@SerieCI", serieCI);
                                command.Parameters.AddWithValue("@NumarCI", numarCI);

                                if (conducatorAutoID.HasValue)
                                {
                                    // In update mode, pass the conducatorAutoID to the query
                                    command.Parameters.AddWithValue("@ID", conducatorAutoID.Value);
                                }
                                else
                                {
                                    // In insert mode, pass rcaID
                                    command.Parameters.AddWithValue("@rcaID", rcaID);
                                }

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
