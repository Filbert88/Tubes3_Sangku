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
using Microsoft.Win32;


namespace FingerprintApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void fileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                inputImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (inputImage.Source == null)
            {
                MessageBox.Show("Please select an image first.", "No Image Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Simulate search process
            System.Threading.Thread.Sleep(500); // Simulate delay

            // Update UI with mock data
            resultImage.Source = inputImage.Source; // Just duplicate the input for demonstration
            searchTimeLabel.Content = $"Waktu Pencarian: 120 ms";
            matchPercentageLabel.Content = $"Persentase Kecocokkan: 95%";

            // Populate results list (mock data)
            resultsList.Items.Add("Result 1");
            resultsList.Items.Add("Result 2");
            resultsList.Items.Add("Result 3");
        }

        // Update UI when toggling algorithms
        private void toggleBM_Checked(object sender, RoutedEventArgs e)
        {
            if (toggleBM.IsChecked == true)
            {
                MessageBox.Show("Using Boyer-Moore algorithm.");
            }
        }

        private void toggleKMP_Checked(object sender, RoutedEventArgs e)
        {
            if (toggleKMP.IsChecked == true)
            {
                MessageBox.Show("Using Knuth-Morris-Pratt algorithm.");
            }
        }
    }
}
