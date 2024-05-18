namespace Database
{
    public static class DatabaseConfig
    {
        private static string? Server { get; set; }
        private static string? User { get; set; }
        private static string? Database { get; set; }
        private static string? Password { get; set; }
        public static string ConnectionString { get; private set; } = string.Empty;
        public static void SetConnection(string server, string user, string database, string password)
        {
            Server = server;
            User = user;
            Database = database;
            Password = password;
            ConnectionString = $"server={Server};user={User};database={Database};password={Password};";
        }
    }
}