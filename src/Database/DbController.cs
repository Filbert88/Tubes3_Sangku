using MySql.Data.MySqlClient;
using Models;
using Services.Encryption;

namespace Database
{
    public class DbController
    {
        private static uint[] encryptionKey = { 0x01234567, 0x89ABCDEF, 0xFEDCBA98, 0x76543210 };
        public static List<Biodata> GetDecryptedBiodataList()
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
                        Console.WriteLine("1");
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("2");
                            while (reader.Read())
                            {
                                Console.WriteLine("3");
                                Biodata biodata = new Biodata
                                {
                                    NIK = SimpleXTEA.Decrypt(reader["NIK"].ToString() ?? string.Empty, encryptionKey),
                                    NamaAlay = SimpleXTEA.Decrypt(reader["nama"].ToString() ?? string.Empty, encryptionKey),
                                    TempatLahir = SimpleXTEA.Decrypt(reader["tempat_lahir"].ToString() ?? string.Empty, encryptionKey),
                                    TanggalLahir = Convert.ToDateTime(reader["tanggal_lahir"]).ToString("dd-MM-yyyy") ?? string.Empty,
                                    JenisKelamin = SimpleXTEA.Decrypt(reader["jenis_kelamin"].ToString() ?? string.Empty, encryptionKey),
                                    GolonganDarah = SimpleXTEA.Decrypt(reader["golongan_darah"].ToString() ?? string.Empty, encryptionKey),
                                    Alamat = SimpleXTEA.Decrypt(reader["alamat"].ToString() ?? string.Empty, encryptionKey),
                                    Agama = SimpleXTEA.Decrypt(reader["agama"].ToString() ?? string.Empty, encryptionKey),
                                    StatusPerkawinan = SimpleXTEA.Decrypt(reader["status_perkawinan"].ToString() ?? string.Empty, encryptionKey),
                                    Pekerjaan = SimpleXTEA.Decrypt(reader["pekerjaan"].ToString() ?? string.Empty, encryptionKey),
                                    Kewarganegaraan = SimpleXTEA.Decrypt(reader["kewarganegaraan"].ToString() ?? string.Empty, encryptionKey)
                                };
                                biodataList.Add(biodata);
                                Console.WriteLine(biodata.NamaAlay);
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

        public static List<FingerprintPath> GetDecryptedFingerprintList()
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
                                string encryptedImagePath = reader["berkas_citra"].ToString() ?? string.Empty;
                                string encryptedNama = reader["nama"].ToString() ?? string.Empty;

                                string decryptedImagePath = SimpleXTEA.Decrypt(encryptedImagePath, encryptionKey);
                                string decryptedNama = SimpleXTEA.Decrypt(encryptedNama, encryptionKey);

                                FingerprintPath fingerprint = new FingerprintPath
                                {
                                    ImagePath = decryptedImagePath,
                                    Nama = decryptedNama
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
                                    NamaAlay = reader["nama"].ToString() ?? string.Empty,
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
                                    Nama = reader["nama"].ToString() ?? string.Empty
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

        public static void EncryptAndSaveBiodata()
        {
            List<Biodata> biodataList = GetBiodataList();

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    foreach (var biodata in biodataList)
                    {
                        string encryptedNIK = SimpleXTEA.Encrypt(biodata.NIK, encryptionKey);
                        string encryptedNama = SimpleXTEA.Encrypt(biodata.NamaAlay, encryptionKey);
                        string encryptedTempatLahir = SimpleXTEA.Encrypt(biodata.TempatLahir, encryptionKey);
                        string encryptedJenisKelamin = SimpleXTEA.Encrypt(biodata.JenisKelamin, encryptionKey);
                        string encryptedGolonganDarah = SimpleXTEA.Encrypt(biodata.GolonganDarah, encryptionKey);
                        string encryptedAlamat = SimpleXTEA.Encrypt(biodata.Alamat, encryptionKey);
                        string encryptedAgama = SimpleXTEA.Encrypt(biodata.Agama, encryptionKey);
                        string encryptedStatusPerkawinan = SimpleXTEA.Encrypt(biodata.StatusPerkawinan, encryptionKey);
                        string encryptedPekerjaan = SimpleXTEA.Encrypt(biodata.Pekerjaan, encryptionKey);
                        string encryptedKewarganegaraan = SimpleXTEA.Encrypt(biodata.Kewarganegaraan, encryptionKey);
                        
                        string sql = "UPDATE biodata SET NIK = @NIK, nama = @Nama, tempat_lahir = @TempatLahir, jenis_kelamin = @JenisKelamin, golongan_darah = @GolonganDarah, alamat = @Alamat, agama = @Agama, status_perkawinan = @StatusPerkawinan, pekerjaan = @Pekerjaan, kewarganegaraan = @Kewarganegaraan WHERE NIK = @OriginalNIK";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@NIK", encryptedNIK);
                            cmd.Parameters.AddWithValue("@Nama", encryptedNama);
                            cmd.Parameters.AddWithValue("@TempatLahir", encryptedTempatLahir);
                            cmd.Parameters.AddWithValue("@JenisKelamin", encryptedJenisKelamin);
                            cmd.Parameters.AddWithValue("@GolonganDarah", encryptedGolonganDarah);
                            cmd.Parameters.AddWithValue("@Alamat", encryptedAlamat);
                            cmd.Parameters.AddWithValue("@Agama", encryptedAgama);
                            cmd.Parameters.AddWithValue("@StatusPerkawinan", encryptedStatusPerkawinan);
                            cmd.Parameters.AddWithValue("@Pekerjaan", encryptedPekerjaan);
                            cmd.Parameters.AddWithValue("@Kewarganegaraan", encryptedKewarganegaraan);
                            cmd.Parameters.AddWithValue("@OriginalNIK", biodata.NIK);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Error: {ex.Message}");
                }
            }
        }

        public static void EncryptAndSaveFingerprint()
        {
            List<FingerprintPath> fingerprintList = GetFingerprintList();

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    foreach (var fingerprint in fingerprintList)
                    {
                        string encryptedImagePath = SimpleXTEA.Encrypt(fingerprint.ImagePath, encryptionKey);
                        string encryptedNama = SimpleXTEA.Encrypt(fingerprint.Nama, encryptionKey);

                        // Console.WriteLine($"Encrypted ImagePath: {encryptedImagePath}");
                        // Console.WriteLine($"Decrypted ImagePath: {fingerprint.ImagePath}");
                        // Console.WriteLine($"Encrypted Nama: {encryptedNama}");
                        // Console.WriteLine($"Decrypted Nama: {fingerprint.Nama}");

                        string sql1 = "DELETE FROM sidik_jari WHERE nama = @OriginalNama";
                        using (MySqlCommand cmd = new MySqlCommand(sql1, conn))
                        {
                            cmd.Parameters.AddWithValue("@OriginalNama", fingerprint.Nama);
                            cmd.ExecuteNonQuery();
                        }

                        string sql2 = "INSERT INTO sidik_jari (berkas_citra, nama) VALUES (@ImagePath, @Nama)";
                        using (MySqlCommand cmd = new MySqlCommand(sql2, conn))
                        {
                            cmd.Parameters.AddWithValue("@ImagePath", encryptedImagePath);
                            cmd.Parameters.AddWithValue("@Nama", encryptedNama);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Error: {ex.Message}");
                }
            }
        }

        public static void EncryptAndSaveData()
        {
            EncryptAndSaveBiodata();
            EncryptAndSaveFingerprint();
        }
    }
}