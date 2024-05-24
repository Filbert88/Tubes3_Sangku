using System;
using System.Windows;
using System.Windows.Controls;
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
                // inputImage.Source = new BitmapImage(new Uri(filePath));
            }
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e) // Note the async keyword
        {
            if (inputImage.Source == null)
            {
                MessageBox.Show("Please select an image first.", "No Image Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string algorithm = toggleBM.IsChecked == true ? "BM" : "KMP";
            Console.WriteLine("[DEBUG] Search button clicked");
            Console.WriteLine("[DEBUG] Selected file path: " + filePath);
            Console.WriteLine("[DEBUG] Selected algorithm: " + algorithm);

            try
            {
                // Run the search operation asynchronously
                SearchResult searchResult = await Task.Run(() => Searcher.GetResult(filePath, algorithm));

                // Clear the results list
                resultsList.Items.Clear();

                // Display the results in the list
                if (searchResult != null)
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

                    // Split result message into lines and add each line as a separate item
                    foreach (var line in resultMessage.Split('\n'))
                    {
                        resultsList.Items.Add(new TextBlock { Text = line });
                    }
                }
                else
                {
                    resultsList.Items.Add(new TextBlock { Text = "No matching fingerprint found." });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during the search process: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
