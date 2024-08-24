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

namespace Proiect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RcaButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("RCA was clicked");
        }
        private void LocuinteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Locuinte was clicked");
        }

        private void AdaugaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditeazaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CautaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SorteazaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StergeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}