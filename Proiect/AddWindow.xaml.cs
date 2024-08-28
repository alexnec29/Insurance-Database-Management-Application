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
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

        public DateTime DataExpirarePolita { get; private set; }
        public string NumarInmatriculare { get; private set; }
        public string SerieSasiu { get; private set; }
        public string CnpCui { get; private set; }
        public string Nume { get; private set; }
        public string Prenume { get; private set; }
        public string Judet { get; private set; }
        public string Localitate { get; private set; }
        public string Adresa { get; private set; }
        public DateTime DataObtinerePermis { get; private set; }
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
        public DateTime DataPrimeiInmatriculari { get; private set; }
        public DateTime DataExpirareITP { get; private set; }
        public string NumarTelefon { get; private set; }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Parse and validate all the input fields
                DataExpirarePolita = DateTime.Parse(DataExpirarePolitaTextBox.Text);
                NumarInmatriculare = NumarInmatriculareTextBox.Text;
                SerieSasiu = SerieSasiuTextBox.Text;
                CnpCui = CnpCuiTextBox.Text;
                Nume = NumeTextBox.Text;
                Prenume = PrenumeTextBox.Text;
                Judet = JudetTextBox.Text;
                Localitate = LocalitateTextBox.Text;
                Adresa = AdresaTextBox.Text;
                DataObtinerePermis = DateTime.Parse(DataObtinerePermisTextBox.Text);
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
                DataPrimeiInmatriculari = DateTime.Parse(DataPrimeiInmatriculariTextBox.Text);
                DataExpirareITP = DateTime.Parse(DataExpirareITPTextBox.Text);
                NumarTelefon = NumarTelefonTextBox.Text;

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