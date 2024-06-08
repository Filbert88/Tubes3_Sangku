using System.Text;

namespace Encryption
{
    public class SimpleXTEA
    {
        private const int NumRounds = 32;
        private const uint Delta = 0x9E3779B9;

        public static string Encrypt(string text, uint[] key)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(text);
            int paddedLength = (plainBytes.Length + 7) / 8 * 8;
            byte[] data = new byte[paddedLength];
            Array.Copy(plainBytes, data, plainBytes.Length);
            int n = data.Length / 8;
            byte[] encrypted = new byte[data.Length];

            for (int i = 0; i < n; i++)
            {
                uint v0 = BitConverter.ToUInt32(data, i * 8);
                uint v1 = BitConverter.ToUInt32(data, i * 8 + 4);
                uint sum = 0;

                for (int round = 0; round < NumRounds; round++)
                {
                    v0 += ((v1 << 4 ^ v1 >> 5) + v1) ^ (sum + key[sum & 3]);
                    sum += Delta;
                    v1 += ((v0 << 4 ^ v0 >> 5) + v0) ^ (sum + key[sum >> 11 & 3]);
                }

                Array.Copy(BitConverter.GetBytes(v0), 0, encrypted, i * 8, 4);
                Array.Copy(BitConverter.GetBytes(v1), 0, encrypted, i * 8 + 4, 4);
            }

            return BitConverter.ToString(encrypted).Replace("-", "");
        }

        public static string Decrypt(string text, uint[] key)
        {
            byte[] data = HexStringToByteArray(text);
            int n = data.Length / 8;
            byte[] decrypted = new byte[data.Length];

            for (int i = 0; i < n; i++)
            {
                uint v0 = BitConverter.ToUInt32(data, i * 8);
                uint v1 = BitConverter.ToUInt32(data, i * 8 + 4);
                uint sum = unchecked(Delta * NumRounds);

                for (int round = 0; round < NumRounds; round++)
                {
                    v1 -= ((v0 << 4 ^ v0 >> 5) + v0) ^ (sum + key[sum >> 11 & 3]);
                    sum -= Delta;
                    v0 -= ((v1 << 4 ^ v1 >> 5) + v1) ^ (sum + key[sum & 3]);
                }

                Array.Copy(BitConverter.GetBytes(v0), 0, decrypted, i * 8, 4);
                Array.Copy(BitConverter.GetBytes(v1), 0, decrypted, i * 8 + 4, 4);
            }

            return Encoding.UTF8.GetString(decrypted).TrimEnd('\0');
        }

        private static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}