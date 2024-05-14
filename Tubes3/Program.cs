using System.Drawing;
using System.Text;
using System.Diagnostics;
public class ImageReader {
    public static string ConvertImageToBinary(string imagePath) {
        using (Bitmap bmp = new Bitmap(imagePath)) {
            StringBuilder binary = new StringBuilder();
            for (int y = 0; y < bmp.Height; y++) {
                for (int x = 0; x < bmp.Width; x++) {
                    Color pixel = bmp.GetPixel(x, y);
                    int gray = (int)((pixel.R * 0.3) + (pixel.G * 0.59) + (pixel.B * 0.11));
                    binary.Append(gray > 128 ? '1' : '0');
                }
            }

            return binary.ToString();
        }
    }
}

class BinaryToAsciiConverter {
    public static string ConvertBinaryToAscii(string binaryString) {
        StringBuilder ascii = new StringBuilder();
        for (int i = 0; i < binaryString.Length; i += 8) {
            string byteString = binaryString.Substring(i, 8);
            ascii.Append((char)Convert.ToInt32(byteString, 2));
        }

        return ascii.ToString();
    }
}

class KnuthMorrisPratt {
    public static List<int> Search(string pattern, string text) {
        int patternLength = pattern.Length;
        int textLength = text.Length;
        List<int> result = new List<int>();
        int[] lps = generateLPSArray(pattern, patternLength);

        int patternIdx = 0; 
        int textIdx = 0;
        while ((textLength - textIdx) >= (patternLength - patternIdx)) {
            if (pattern[patternIdx] == text[textIdx]) {
                patternIdx++;
                textIdx++;
            }
            if (patternIdx == patternLength) {
                result.Add(textIdx - patternIdx);
                patternIdx = lps[patternIdx - 1];
            }

            else if (textIdx < textLength && pattern[patternIdx] != text[textIdx]) {
                if (patternIdx != 0) {
                    patternIdx = lps[patternIdx - 1];
                } else {
                    textIdx = textIdx + 1;
                }
            }
        }

        return result;
    }
 
    static int[] generateLPSArray(string pat, int patternLength) {
        int len = 0;
        int i = 1;
        int[] lps = new int[patternLength];
        lps[0] = 0;
 
        while (i < patternLength) {
            if (pat[i] == pat[len]) {
                len++;
                lps[i] = len;
                i++;
            } else {
                if (len != 0) {
                    len = lps[len - 1];
                } else {
                    lps[i] = len;
                    i++;
                }
            }
        }

        return lps;
    }
}

class BoyerMoore {
    static int max(int a, int b) {
        if (a > b) {
            return a;
        } else {
            return b;
        }
    }
    static int[] generateBadCharArray(char[] str, int size) {
        int[] badChar = new int[256];
        int i;
        for (i = 0; i < 256; i++) {
            badChar[i] = -1;
        }

        for (i = 0; i < size; i++) {
            if ((int)str[i] >= 0 && (int)str[i] < 256) {
                badChar[(int)str[i]] = i;
            } else {
                Console.WriteLine("Unexpected character code: " + (int)str[i] + " at position " + i);
            }
        }

        return badChar;
    }
 
    public static List<int> Search(string pattern, string text) {
        char[] textCharArray = text.ToCharArray();
        char[] patternCharArray = pattern.ToCharArray();
        int patternLength = patternCharArray.Length;
        int textLength = textCharArray.Length;

        List<int> result = new List<int>();
        
        int[] badChar = generateBadCharArray(patternCharArray, patternLength);
        int shift = 0;
        while (shift <= (textLength - patternLength)) {
            int idx = patternLength - 1;
            while (idx >= 0 && patternCharArray[idx] == textCharArray[shift + idx]) {
                idx--;
            }
            if (idx < 0) {
                result.Add(shift);
                if (shift + patternLength < textLength) {
                    shift += patternLength - badChar[textCharArray[shift + patternLength]];
                } else {
                    shift += 1;
                }
            } else {
                shift += max(1, idx - badChar[textCharArray[shift + idx]]);
            }
        }
        
        return result;
    }
}

class LevenshteinDistance {
    public static int Get(string text1, string text2) {
        int text1Length = text1.Length;
        int text2Length = text2.Length;
        int[,] dp = new int[text1Length + 1, text2Length + 1];

        for (int i = 0; i <= text1Length; i++) {
            for (int j = 0; j <= text2Length; j++) {
                if (i == 0) {
                    dp[i, j] = j;
                } else if (j == 0) {
                    dp[i, j] = i;
                } else {
                    int cost = (text1[i - 1] == text2[j - 1]) ? 0 : 1;
                    dp[i, j] = Math.Min(Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1), dp[i - 1, j - 1] + cost);
                }
            }
        }

        return dp[text1Length, text2Length];
    }
}


class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        string imagePath = "C:/Users/LENOVO/OneDrive/Desktop/test.jpg";
        string binaryString = ImageReader.ConvertImageToBinary(imagePath);
        Console.WriteLine("Binary Output: " + binaryString.Length);
        string asciiString = BinaryToAsciiConverter.ConvertBinaryToAscii(binaryString);

        string outputPath = "output.txt";
        string outputPath2 = "output2.txt";
        File.WriteAllText(outputPath, asciiString);
        File.WriteAllText(outputPath2, binaryString);

        // Console.WriteLine(asciiString);
        string pattern = "ÿÿ·ÿÿðÀ";

        stopwatch.Start();

        List<int> kmpResult = KnuthMorrisPratt.Search(pattern, asciiString);
        Console.WriteLine("KMP found matched: " + kmpResult.Count);
        stopwatch.Stop();
        Console.WriteLine("Execution Time: " + stopwatch.ElapsedMilliseconds + " ms");

        stopwatch.Start();

        List<int> bmResult = BoyerMoore.Search(pattern, asciiString);
        Console.WriteLine("BM found matched: " + bmResult.Count);
        stopwatch.Stop();
        Console.WriteLine("Execution Time: " + stopwatch.ElapsedMilliseconds + " ms");


        string text1 = "abcde";
        string text2 = "ace";
        Console.WriteLine("LCS Length: " + LevenshteinDistance.Get(text1, text2));
    }
}