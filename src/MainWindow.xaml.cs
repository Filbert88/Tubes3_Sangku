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
using Models;
using Services;
using Database;
using System.Threading.Tasks; // For asynchronous programming

namespace FingerprintApp
{
    public partial class MainWindow : Window
    {
        string? filePath;
        public MainWindow()
        {
            DatabaseConfig.SetConnection(
                server: "127.0.0.1",
                user: "root",
                database: "stima3",
                password: "filbert21"
            );
            InitializeComponent();
        }
        private void fileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.BMP)|*.BMP";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                inputImage.Source = new BitmapImage(new Uri(filePath));
            }
        }

        public class ResultItem
        {
            public string? Label { get; set; }
            public string? Value { get; set; }
        }

        private SearchResult GetDummySearchResult(string algorithm)
        {
            return new SearchResult
            {
                algorithm = algorithm,
                similarity = 99,
                execTime = 123,
                biodata = new Biodata
                {
                    NIK = "123456789",
                    NamaAlay = "John Doe",
                    TempatLahir = "Jakarta",
                    TanggalLahir = "23/12/2024",
                    JenisKelamin = "Male",
                    GolonganDarah = "O",
                    Alamat = "Jalan Sangkuriang No 11 Samping Dago Suites",
                    Agama = "None",
                    StatusPerkawinan = "Married",
                    Pekerjaan = "Software Developer",
                    Kewarganegaraan = "Indonesia"
                }
            };
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e) // Note the async keyword
        {
            if (inputImage.Source == null || filePath ==  null)
            {
                MessageBox.Show("Please select an image first.", "No Image Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            loadingPanel.Visibility = Visibility.Visible;
            ClearResults();

            string algorithm = toggleBM.IsChecked == true ? "BM" : "KMP";
            Console.WriteLine("[DEBUG] Search button clicked");
            Console.WriteLine("[DEBUG] Selected file path: " + filePath);
            Console.WriteLine("[DEBUG] Selected algorithm: " + algorithm);

            await Task.Delay(3000);

            try
            {
                // Run the search operation asynchronously
                SearchResult searchResult = await Task.Run(() => GetDummySearchResult(algorithm));

                // Display the results in a message box
                if (searchResult != null && searchResult.biodata != null)
                {
                    var biodata = searchResult.biodata;
                    string resultMessage = $"Algorithm Used: {searchResult.algorithm}\n" +
                                           $"Similarity: {searchResult.similarity}%\n" +
                                           $"Execution Time: {searchResult.execTime} ms\n\n" +
                                           $"Biodata:\n" +
                                           $"NIK: {biodata.NIK}\n" +
                                           $"Nama Alay: {biodata.NamaAlay}\n" +
                                           $"Tempat Lahir: {biodata.TempatLahir}\n" +
                                           $"Tanggal Lahir: {biodata.TanggalLahir}\n" +
                                           $"Jenis Kelamin: {biodata.JenisKelamin}\n" +
                                           $"Golongan Darah: {biodata.GolonganDarah}\n" +
                                           $"Alamat: {biodata.Alamat}\n" +
                                           $"Agama: {biodata.Agama}\n" +
                                           $"Status Perkawinan: {biodata.StatusPerkawinan}\n" +
                                           $"Pekerjaan: {biodata.Pekerjaan}\n" +
                                           $"Kewarganegaraan: {biodata.Kewarganegaraan}";
                    resultsList.Items.Add(new ResultItem { Label = "Algorithm Used", Value = searchResult.algorithm });
                    resultsList.Items.Add(new ResultItem { Label = "Similarity", Value = $"{searchResult.similarity}%" });
                    resultsList.Items.Add(new ResultItem { Label = "Execution Time", Value = $"{searchResult.execTime} ms" });
                    resultsList.Items.Add(new ResultItem { Label = "NIK", Value = $"{biodata.NIK}" });
                    resultsList.Items.Add(new ResultItem { Label = "Nama", Value = $"{biodata.NamaAlay}" });
                    resultsList.Items.Add(new ResultItem { Label = "Tempat Lahir", Value = $"{biodata.TempatLahir}" });
                    resultsList.Items.Add(new ResultItem { Label = "Tanggal Lahir", Value = $"{biodata.TanggalLahir}" });
                    resultsList.Items.Add(new ResultItem { Label = "Jenis Kelamin", Value = $"{biodata.JenisKelamin}" });
                    resultsList.Items.Add(new ResultItem { Label = "Golongan Darah", Value = $"{biodata.GolonganDarah}" });
                    resultsList.Items.Add(new ResultItem { Label = "Alamat", Value = $"{biodata.Alamat}" });
                    resultsList.Items.Add(new ResultItem { Label = "Agama", Value = $"{biodata.Agama}" });
                    resultsList.Items.Add(new ResultItem { Label = "Status Perkawinan", Value = $"{biodata.StatusPerkawinan}" });
                    resultsList.Items.Add(new ResultItem { Label = "Pekerjaan", Value = $"{biodata.Pekerjaan}" });
                    resultsList.Items.Add(new ResultItem { Label = "Kewarganegaraan", Value = $"{biodata.Kewarganegaraan}" });
                    // resultsList.Items.Add($"NIK: {biodata.NIK}");
                    // resultsList.Items.Add($"Nama: {biodata.NamaAlay}");
                    // resultsList.Items.Add($"Tempat Lahir: {biodata.TempatLahir}");
                    // resultsList.Items.Add($"Tanggal Lahir: {biodata.TanggalLahir}");
                    // resultsList.Items.Add($"Jenis Kelamin: {biodata.JenisKelamin}");
                    // resultsList.Items.Add($"Golongan Darah: {biodata.GolonganDarah}");
                    // resultsList.Items.Add($"Alamat: {biodata.Alamat}");
                    // resultsList.Items.Add($"Agama: {biodata.Agama}");
                    // resultsList.Items.Add($"Status Perkawinan: {biodata.StatusPerkawinan}");
                    // resultsList.Items.Add($"Pekerjaan: {biodata.Pekerjaan}");
                    // resultsList.Items.Add($"Kewarganegaraan: {biodata.Kewarganegaraan}");
                    placeholderText.Visibility = Visibility.Collapsed;
                    resultsList.Visibility = Visibility.Visible;

                    // MessageBox.Show(resultMessage, "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    placeholderText.Visibility = Visibility.Visible;
            resultsList.Visibility = Visibility.Collapsed;
                    MessageBox.Show("No matching fingerprint found.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during the search process: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                placeholderText.Visibility = Visibility.Visible;
        resultsList.Visibility = Visibility.Collapsed;
            }
            finally
            {
                loadingPanel.Visibility = Visibility.Collapsed;
            }
        }

        // Update UI when toggling algorithms
        private void toggleBM_Checked(object sender, RoutedEventArgs e)
        {
            if (toggleBM.IsChecked == true)
            {
                ClearResults();   
                // MessageBox.Show("Using Boyer-Moore algorithm.");
            }
        }

        private void toggleKMP_Checked(object sender, RoutedEventArgs e)
        {
            if (toggleKMP.IsChecked == true)
            {
                ClearResults();
                // MessageBox.Show("Using Knuth-Morris-Pratt algorithm.");
            }
        }
        private void ClearResults()
        {
            resultsList.Items.Clear();
            placeholderText.Visibility = Visibility.Visible;  // Show the placeholder when results are cleared
            resultsList.Visibility = Visibility.Collapsed;  // Hide the results list
        }
    }
}