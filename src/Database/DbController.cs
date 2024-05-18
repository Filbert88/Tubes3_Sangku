using MySql.Data.MySqlClient;
using Models;

namespace Database
{
    public class DbController
    {
        public static List<Biodata> GetBiodataList()
        {
            List<Biodata> biodataList = new List<Biodata>();

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("[DEBUG] Successfully connected to the database.");

                    string sql = "SELECT * FROM biodata";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Biodata biodata = new Biodata
                                {
                                    NIK = reader["NIK"].ToString() ?? string.Empty,
                                    Nama = reader["nama"].ToString() ?? string.Empty,
                                    TempatLahir = reader["tempat_lahir"].ToString() ?? string.Empty,
                                    TanggalLahir = Convert.ToDateTime(reader["tanggal_lahir"]).ToString("dd-MM-yyyy") ?? string.Empty,
                                    JenisKelamin = reader["jenis_kelamin"].ToString() ?? string.Empty,
                                    GolonganDarah = reader["golongan_darah"].ToString() ?? string.Empty,
                                    Alamat = reader["alamat"].ToString() ?? string.Empty,
                                    Agama = reader["agama"].ToString() ?? string.Empty,
                                    StatusPerkawinan = reader["status_perkawinan"].ToString() ?? string.Empty,
                                    Pekerjaan = reader["pekerjaan"].ToString() ?? string.Empty,
                                    Kewarganegaraan = reader["kewarganegaraan"].ToString() ?? string.Empty
                                };
                                biodataList.Add(biodata);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Error: {ex.Message}");
                }
            }

            return biodataList;
        }

        public static List<FingerprintPath> GetFingerprintList()
        {
            List<FingerprintPath> fingerprintList = new List<FingerprintPath>();

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("[DEBUG] Successfully connected to the database.");

                    string sql = "SELECT * FROM sidik_jari";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FingerprintPath fingerprint = new FingerprintPath
                                {
                                    ImagePath = reader["berkas_citra"].ToString() ?? string.Empty,
                                    NamaAlay = reader["nama"].ToString() ?? string.Empty
                                };
                                fingerprintList.Add(fingerprint);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Error: {ex.Message}");
                }
            }

            return fingerprintList;
        }
    }
}
