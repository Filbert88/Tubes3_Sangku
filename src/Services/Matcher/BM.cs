namespace Services.Matcher
{
    class BoyerMoore
    {
        static int max(int a, int b)
        {
            if (a > b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
        static int[] generateBadCharArray(char[] str, int size)
        {
            int[] badChar = new int[256];
            int i;
            for (i = 0; i < 256; i++)
            {
                badChar[i] = -1;
            }

            for (i = 0; i < size; i++)
            {
                if ((int)str[i] >= 0 && (int)str[i] < 256)
                {
                    badChar[(int)str[i]] = i;
                }
                else
                {
                    Console.WriteLine("Unexpected character code: " + (int)str[i] + " at position " + i);
                }
            }

            return badChar;
        }

        public static List<int> Search(string pattern, string text)
        {
            char[] textCharArray = text.ToCharArray();
            char[] patternCharArray = pattern.ToCharArray();
            int patternLength = patternCharArray.Length;
            int textLength = textCharArray.Length;

            List<int> result = new List<int>();

            int[] badChar = generateBadCharArray(patternCharArray, patternLength);
            int shift = 0;
            while (shift <= (textLength - patternLength))
            {
                int idx = patternLength - 1;
                while (idx >= 0 && patternCharArray[idx] == textCharArray[shift + idx])
                {
                    idx--;
                }
                if (idx < 0)
                {
                    result.Add(shift);
                    if (shift + patternLength < textLength)
                    {
                        shift += patternLength - badChar[textCharArray[shift + patternLength]];
                    }
                    else
                    {
                        shift += 1;
                    }
                }
                else
                {
                    shift += max(1, idx - badChar[textCharArray[shift + idx]]);
                }
            }

            return result;
        }
    }
}