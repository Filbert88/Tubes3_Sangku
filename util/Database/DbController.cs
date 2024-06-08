using MySql.Data.MySqlClient;
using Models;
using Encryption;

namespace Database
{
    public class DbController
    {
        private static uint[] encryptionKey = { 0x01234567, 0x89ABCDEF, 0xFEDCBA98, 0x76543210 };

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
                    Console.WriteLine($"[DEBUG]Encrypt Biodata Error: {ex.Message}");
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
                    Console.WriteLine($"[DEBUG] Encrypt Fingerprint Error: {ex.Message}");
                }
            }
        }

        public static void EncryptAndSaveData()
        {
            EncryptAndSaveBiodata();
            EncryptAndSaveFingerprint();
        }

        public static void DecryptAndSaveBiodata()
        {
            List<Biodata> biodataList = GetBiodataList();

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    foreach (var biodata in biodataList)
                    {
                        string decryptedNIK = SimpleXTEA.Decrypt(biodata.NIK, encryptionKey);
                        string decryptedNama = SimpleXTEA.Decrypt(biodata.NamaAlay, encryptionKey);
                        string decryptedTempatLahir = SimpleXTEA.Decrypt(biodata.TempatLahir, encryptionKey);
                        string decryptedJenisKelamin = SimpleXTEA.Decrypt(biodata.JenisKelamin, encryptionKey);
                        string decryptedGolonganDarah = SimpleXTEA.Decrypt(biodata.GolonganDarah, encryptionKey);
                        string decryptedAlamat = SimpleXTEA.Decrypt(biodata.Alamat, encryptionKey);
                        string decryptedAgama = SimpleXTEA.Decrypt(biodata.Agama, encryptionKey);
                        string decryptedStatusPerkawinan = SimpleXTEA.Decrypt(biodata.StatusPerkawinan, encryptionKey);
                        string decryptedPekerjaan = SimpleXTEA.Decrypt(biodata.Pekerjaan, encryptionKey);
                        string decryptedKewarganegaraan = SimpleXTEA.Decrypt(biodata.Kewarganegaraan, encryptionKey);

                        string sql = "UPDATE biodata SET NIK = @NIK, nama = @Nama, tempat_lahir = @TempatLahir, jenis_kelamin = @JenisKelamin, golongan_darah = @GolonganDarah, alamat = @Alamat, agama = @Agama, status_perkawinan = @StatusPerkawinan, pekerjaan = @Pekerjaan, kewarganegaraan = @Kewarganegaraan WHERE NIK = @OriginalNIK";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@NIK", decryptedNIK);
                            cmd.Parameters.AddWithValue("@Nama", decryptedNama);
                            cmd.Parameters.AddWithValue("@TempatLahir", decryptedTempatLahir);
                            cmd.Parameters.AddWithValue("@JenisKelamin", decryptedJenisKelamin);
                            cmd.Parameters.AddWithValue("@GolonganDarah", decryptedGolonganDarah);
                            cmd.Parameters.AddWithValue("@Alamat", decryptedAlamat);
                            cmd.Parameters.AddWithValue("@Agama", decryptedAgama);
                            cmd.Parameters.AddWithValue("@StatusPerkawinan", decryptedStatusPerkawinan);
                            cmd.Parameters.AddWithValue("@Pekerjaan", decryptedPekerjaan);
                            cmd.Parameters.AddWithValue("@Kewarganegaraan", decryptedKewarganegaraan);
                            cmd.Parameters.AddWithValue("@OriginalNIK", biodata.NIK);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Decrypt Biodata Error: {ex.Message}");
                }
            }
        }

        public static void DecryptAndSaveFingerprint()
        {
            List<FingerprintPath> fingerprintList = GetFingerprintList();

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    foreach (var fingerprint in fingerprintList)
                    {
                        string decryptedImagePath = SimpleXTEA.Decrypt(fingerprint.ImagePath, encryptionKey);
                        string decryptedNama = SimpleXTEA.Decrypt(fingerprint.Nama, encryptionKey);

                        string sql1 = "DELETE FROM sidik_jari WHERE nama = @OriginalNama";
                        using (MySqlCommand cmd = new MySqlCommand(sql1, conn))
                        {
                            cmd.Parameters.AddWithValue("@OriginalNama", fingerprint.Nama);
                            cmd.ExecuteNonQuery();
                        }

                        string sql2 = "INSERT INTO sidik_jari (berkas_citra, nama) VALUES (@ImagePath, @Nama)";
                        using (MySqlCommand cmd = new MySqlCommand(sql2, conn))
                        {
                            cmd.Parameters.AddWithValue("@ImagePath", decryptedImagePath);
                            cmd.Parameters.AddWithValue("@Nama", decryptedNama);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Decrypt Fingerprint Error: {ex.Message}");
                }
            }
        }

        public static void DecryptAndSaveData()
        {
            DecryptAndSaveBiodata();
            DecryptAndSaveFingerprint();
        }

        public static void UpdateFingerprintPaths(string newPath)
        {
            List<FingerprintPath> fingerprintList = GetFingerprintList();

            using (MySqlConnection conn = new MySqlConnection(DatabaseConfig.ConnectionString))
            {
                try
                {
                    conn.Open();
                    foreach (var fingerprint in fingerprintList)
                    {
                        string filename = System.IO.Path.GetFileName(fingerprint.ImagePath);
                        string updatedPath = System.IO.Path.Combine(newPath, filename);

                        string sql = "UPDATE sidik_jari SET berkas_citra = @NewPath WHERE berkas_citra = @OriginalPath";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@NewPath", updatedPath);
                            cmd.Parameters.AddWithValue("@OriginalPath", fingerprint.ImagePath);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] Update Fingerprint Paths Error: {ex.Message}");
                }
            }

        }
    }
}