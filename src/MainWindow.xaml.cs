using System.IO;
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
                server: "mysql-clouddb-moontrainid-a5ed.f.aivencloud.com",
                user: "avnadmin",
                database: "stima3",
                password: "AVNS_Zgr-PR3FZxLojDhMyBk",
                port: 11441
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
            if (inputImage.Source == null || filePath == null)
            {
                MessageBox.Show("Please select an image first.", "No Image Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            loadingPanel.Visibility = Visibility.Visible;
            ClearResults();

            searchTimeLabel.Content = "Waktu Pencarian: - ms";
            matchPercentageLabel.Content = "Persentase Kecocokkan: -%";
            algorithmUsedLabel.Content = "Algoritma Digunakan: -";
            resultImage.Source = null;

            string algorithm = toggleBM.IsChecked == true ? "BM" : "KMP";
            Console.WriteLine("[DEBUG] Search button clicked");
            Console.WriteLine("[DEBUG] Selected file path: " + filePath);
            Console.WriteLine("[DEBUG] Selected algorithm: " + algorithm);

            await Task.Delay(3000);

            try
            {
                SearchResult searchResult = await Task.Run(() => Searcher.GetResult(filePath, algorithm));
                resultsList.Items.Clear();  
                if (searchResult.biodata != null)
                {
                    var biodata = searchResult.biodata;
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
                    searchTimeLabel.Content = $"Waktu Pencarian: {searchResult.execTime} ms";
                    matchPercentageLabel.Content = $"Persentase Kecocokkan: {searchResult.similarity}%";
                    algorithmUsedLabel.Content = $"Algoritma Digunakan: {searchResult.algorithm}";

                    if (!string.IsNullOrEmpty(searchResult.imagePath))
                    {
                        resultsList.Items.Add(new TextBlock { Text = $"Image Path: {searchResult.imagePath}" });

                        string imagePath = searchResult.imagePath;
                        if (!Path.IsPathRooted(imagePath))
                        {
                            string projectDir = AppDomain.CurrentDomain.BaseDirectory;
                            string testDir = Path.GetFullPath(Path.Combine(projectDir, "..", "..", "..", "..", "test"));
                            imagePath = Path.GetFullPath(Path.Combine(testDir, imagePath));
                        }

                        Console.WriteLine("[DEBUG] Image path: " + imagePath); 

                        Dispatcher.Invoke(() =>
                        {
                            resultImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
                        });
                    }
                    else
                    {
                        MessageBox.Show("Kosong", "Kosong", MessageBoxButton.OK, MessageBoxImage.Warning);
                        resultImage.Source = null;
                        resultsList.Items.Add(new TextBlock { Text = "No image path available." });
                    }

                    placeholderText.Visibility = Visibility.Collapsed;
                    resultsList.Visibility = Visibility.Visible;
                    }
                else
                {
                    resultsList.Items.Clear();
                    placeholderText.Visibility = Visibility.Collapsed;
                    resultsList.Items.Add(new ResultItem { Label = "", Value = "No matching fingerprint found." });
                    resultsList.Visibility = Visibility.Visible;
                    searchTimeLabel.Content = $"Waktu Pencarian: {searchResult.execTime} ms";
                    matchPercentageLabel.Content = $"Persentase Kecocokkan: 0%";
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

        private void toggleBM_Checked(object sender, RoutedEventArgs e)
        {
            if (toggleBM.IsChecked == true)
            {
                ClearResults();
            }
        }

        private void toggleKMP_Checked(object sender, RoutedEventArgs e)
        {
            if (toggleKMP.IsChecked == true)
            {
                ClearResults();
            }
        }

        private void ClearResults()
        {
            resultsList.Items.Clear();
            placeholderText.Visibility = Visibility.Visible; 
            resultsList.Visibility = Visibility.Collapsed;  
        }
    }
}
