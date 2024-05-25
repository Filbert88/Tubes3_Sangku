using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Models;
using Services;
using Database;

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
                password: "YenaMaria"
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


        private async void searchButton_Click(object sender, RoutedEventArgs e)
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
                SearchResult searchResult = await Task.Run(() => Searcher.GetResult(filePath, algorithm));

                // Clear the results list
                resultsList.Items.Clear();

                // Display the results in the list
                if (searchResult != null)
                // Display the results in a message box
                if (searchResult != null && searchResult.biodata != null)
                {
                    var biodata = searchResult.biodata;
                    // string resultMessage = $"Algorithm Used: {searchResult.algorithm}\n" +
                    //                        $"Similarity: {searchResult.similarity}%\n" +
                    //                        $"Execution Time: {searchResult.execTime} ms\n\n" +
                    //                        $"Biodata:\n" +
                    //                        $"NIK: {biodata.NIK}\n" +
                    //                        $"Nama Alay: {biodata.NamaAlay}\n" +
                    //                        $"Tempat Lahir: {biodata.TempatLahir}\n" +
                    //                        $"Tanggal Lahir: {biodata.TanggalLahir}\n" +
                    //                        $"Jenis Kelamin: {biodata.JenisKelamin}\n" +
                    //                        $"Golongan Darah: {biodata.GolonganDarah}\n" +
                    //                        $"Alamat: {biodata.Alamat}\n" +
                    //                        $"Agama: {biodata.Agama}\n" +
                    //                        $"Status Perkawinan: {biodata.StatusPerkawinan}\n" +
                    //                        $"Pekerjaan: {biodata.Pekerjaan}\n" +
                    //                        $"Kewarganegaraan: {biodata.Kewarganegaraan}";
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
                    placeholderText.Visibility = Visibility.Collapsed;
                    resultsList.Visibility = Visibility.Visible;

                    // MessageBox.Show(resultMessage, "Search Result", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    placeholderText.Visibility = Visibility.Visible;
            resultsList.Visibility = Visibility.Collapsed;
                    resultsList.Items.Add(new TextBlock { Text = "No matching fingerprint found." });
                    resultImage.Source = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during the search process: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                resultImage.Source = null;
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
