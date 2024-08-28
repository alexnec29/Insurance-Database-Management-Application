using System;
using System.Collections.Generic;
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
        public AddWindow2()
        {
            InitializeComponent();
        }

        public DateTime DataExpirarii { get; private set; }
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

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataExpirarii = DateTime.Parse(DataExpirariiTextBox.Text);
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

                this.DialogResult = true; // Close the window and return true to the main window
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Please enter valid data.\nError: {ex.Message}", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Close the window and return false to the main window
        }
    }
}