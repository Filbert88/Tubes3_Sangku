using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Services.Processor
{
    public class ImageProcessor
    {
        static string ConvertImageToBinary(string imagePath)
        {
            StringBuilder binary = new StringBuilder();

            using (Image<Rgba32> image = Image.Load<Rgba32>(imagePath))
            {
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Rgba32 pixel = image[x, y];
                        int gray = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                        binary.Append(gray > 128 ? '1' : '0'); // Bright = 1, Dark = 0
                    }
                }
            }

            return binary.ToString();
        }
        static string ConvertBinaryToAscii(string binaryString)
        {
            StringBuilder ascii = new StringBuilder();
            for (int i = 0; i < binaryString.Length; i += 8)
            {
                int length = Math.Min(8, binaryString.Length - i);
                string byteString = binaryString.Substring(i, length);
                ascii.Append((char)Convert.ToInt32(byteString, 2));
            }

            return ascii.ToString();
        }

        public static string ReadAllPixelToAscii(string imagePath)
        {
            return ConvertBinaryToAscii(ConvertImageToBinary(imagePath));
        }

        static int CountTransitions(string binaryString)
        {
            int transitions = 0;
            for (int i = 1; i < binaryString.Length; i++)
            {
                if (binaryString[i] != binaryString[i - 1])
                {
                    transitions++;
                }
            }
            return transitions;
        }

        static string FindBestTransitionBlock(string binaryString, int blockSize)
        {
            int maxTransitionCount = 0;
            string bestSubstring = binaryString.Substring(0, Math.Min(binaryString.Length, blockSize));

            for (int i = 0; i <= binaryString.Length - blockSize; i += blockSize)
            {
                int length = Math.Min(blockSize, binaryString.Length - i);
                string substring = binaryString.Substring(i, length);
                int transitionCount = CountTransitions(substring);
                if (transitionCount > maxTransitionCount)
                {
                    maxTransitionCount = transitionCount;
                    bestSubstring = substring;
                }
            }

            return bestSubstring;
        }

        public static string ReadBestPixelToAscii(string imagePath)
        {
            var binaryString = ConvertImageToBinary(imagePath);
            return ConvertBinaryToAscii(FindBestTransitionBlock(binaryString, 32));
        }
    }
}