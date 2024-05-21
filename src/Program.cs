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

        string imagePath = "../test/Altered/Altered-Easy/560__F_Left_index_finger_Obl.BMP";
        string algorithm = "BM"; // "KMP" or "BM"

        var searchResult = Searcher.GetResult(imagePath, algorithm);

        Console.WriteLine("[RESULT]");
        if (searchResult.biodata != null)
        {
            Console.WriteLine($"Algorithm     : {searchResult.algorithm}");
            Console.WriteLine($"Biodata       : {searchResult.biodata.NamaAlay}, {searchResult.biodata.NIK}"); // NamaAlay already become RealName
            Console.WriteLine($"Similarity(%) : {searchResult.similarity}");
            Console.WriteLine($"ExecTime(ms)  : {searchResult.execTime}");
            Console.WriteLine($"ResultImage   : {searchResult.imagePath}");
        }
        else
        {
            Console.WriteLine("No matching biodata found.");
        }
    }
}