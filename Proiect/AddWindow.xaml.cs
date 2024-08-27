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
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }
        public string NumeClient { get; private set; }
        public int Suma { get; private set; }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate and store the input data
            NumeClient = NumeClientTextBox.Text;
            if (int.TryParse(SumaTextBox.Text, out int suma))
            {
                Suma = suma;
                this.DialogResult = true; // This will close the window and return true to the main window
            }
            else
            {
                System.Windows.MessageBox.Show("Please enter a valid number for Suma.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // This will close the window and return false to the main window
        }
    }
}
