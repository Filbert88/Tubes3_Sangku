using Encryption;
using Database;

class Program
{
    public static void Main(string[] args)
    {
        DatabaseConfig.SetConnection(
            server: "localhost",
            user: "root",
            database: "stima3",
            password: ""
        );

        // DbController.EncryptAndSaveData();
        // Console.WriteLine("Encrypted.");

        // DbController.DecryptAndSaveData();
        // Console.WriteLine("Decrypted.");

        // DbController.UpdateFingerprintPaths("../hello/x/y/");
        // Console.WriteLine("Updated.");
    }
}