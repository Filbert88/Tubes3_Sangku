using System.Diagnostics;
using Database;
using Models;
using Services;


class Program
{
    static void Main(string[] args)
    {
        // Set database connection config
        DatabaseConfig.SetConnection(
            server: "127.0.0.1",
            user: "root",
            database: "stima3",
            password: ""
        );

        // List<Biodata> biodataList = DbController.GetBiodataList();
        // foreach (var biodata in biodataList)
        // {
        //     Console.WriteLine($"{biodata.NIK}, {biodata.Nama}, {biodata.TempatLahir}, {biodata.TanggalLahir}");
        // }

        // List<Fingerprint> fingerprintList = DbController.GetFingerprintList();
        // foreach (var fingerprint in fingerprintList)
        // {
        //     Console.WriteLine($"Berkas Citra: {fingerprint.ImagePath}, Nama: {fingerprint.NamaAlay}");
        // }
        // Stopwatch stopwatch = new Stopwatch();
        // string imagePath = "../test/Real/1__M_Left_little_finger.BMP";
        // string binaryString = ImageProcessor.ConvertImageToBinary(imagePath);
        // Console.WriteLine("Binary Output: " + binaryString.Length);
        // string asciiString = ImageProcessor.ConvertBinaryToAscii(binaryString);

        // string outputPath = "output.txt";
        // string outputPath2 = "output2.txt";
        // File.WriteAllText(outputPath, asciiString);
        // File.WriteAllText(outputPath2, binaryString);

        // Console.WriteLine(asciiString);
        // string pattern = ImageProcessor.ReadBestPixelToAscii(imagePath);
        // string asciiString = ImageProcessor.ReadAllPixelToAscii(imagePath);
        // Console.WriteLine(pattern);
        // Console.WriteLine(asciiString);

        // stopwatch.Start();

        // List<int> kmpResult = KnuthMorrisPratt.Search(pattern, asciiString);
        // Console.WriteLine("KMP found matched: " + kmpResult.Count);
        // stopwatch.Stop();
        // Console.WriteLine("Execution Time: " + stopwatch.ElapsedMilliseconds + " ms");

        // stopwatch.Start();

        // List<int> bmResult = BoyerMoore.Search(pattern, asciiString);
        // Console.WriteLine("BM found matched: " + bmResult.Count);
        // stopwatch.Stop();
        // Console.WriteLine("Execution Time: " + stopwatch.ElapsedMilliseconds + " ms");


        // string text1 = "abcde";
        // string text2 = "ace";
        // Console.WriteLine("LCS Length: " + LevenshteinDistance.Get(text1, text2));

        string imagePath = "../test/Altered/Altered-Hard/1__M_Left_index_finger_CR.BMP";
        string algorithm = "BM"; // "KMP" or "BM"

        var (biodata, usedAlgorithm, similarity, execTime) = Searcher.GetResult(imagePath, algorithm);

        Console.WriteLine($"[RESULT]");
        if (biodata != null)
        {
            Console.WriteLine($"Algorithm     : {usedAlgorithm}");
            Console.WriteLine($"Biodata       : {biodata.Nama}, {biodata.NIK}");
            Console.WriteLine($"Similarity(%) : {similarity}");
            Console.WriteLine($"execTime(ms)  : {execTime}");
        }
        else
        {
            Console.WriteLine("No matching biodata found.");
        }
    }
}