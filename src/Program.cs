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

        string imagePath = "../test/Altered/Altered-Easy/1__M_Left_index_finger_CR.BMP";
        string algorithm = "BM"; // "KMP" or "BM"

        var (biodata, usedAlgorithm, similarity, execTime) = Searcher.GetResult(imagePath, algorithm);

        Console.WriteLine("[RESULT]");
        if (biodata != null)
        {
            Console.WriteLine($"Algorithm     : {usedAlgorithm}");
            Console.WriteLine($"Biodata       : {biodata.NamaAlay}, {biodata.NIK}"); // NamaAlay already become RealName
            Console.WriteLine($"Similarity(%) : {similarity}");
            Console.WriteLine($"execTime(ms)  : {execTime}");
        }
        else
        {
            Console.WriteLine("No matching biodata found.");
        }
    }
}