namespace Services.Matcher
{
    class KnuthMorrisPratt
    {
        public static List<int> Search(string pattern, string text)
        {
            int patternLength = pattern.Length;
            int textLength = text.Length;
            List<int> result = new List<int>();
            int[] lps = generateLPSArray(pattern, patternLength);

            int patternIdx = 0;
            int textIdx = 0;
            while ((textLength - textIdx) >= (patternLength - patternIdx))
            {
                if (pattern[patternIdx] == text[textIdx])
                {
                    patternIdx++;
                    textIdx++;
                }
                if (patternIdx == patternLength)
                {
                    result.Add(textIdx - patternIdx);
                    patternIdx = lps[patternIdx - 1];
                }

                else if (textIdx < textLength && pattern[patternIdx] != text[textIdx])
                {
                    if (patternIdx != 0)
                    {
                        patternIdx = lps[patternIdx - 1];
                    }
                    else
                    {
                        textIdx = textIdx + 1;
                    }
                }
            }

            return result;
        }

        private static int[] generateLPSArray(string pat, int patternLength)
        {
            int len = 0;
            int i = 1;
            int[] lps = new int[patternLength];
            lps[0] = 0;

            while (i < patternLength)
            {
                if (pat[i] == pat[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else
                {
                    if (len != 0)
                    {
                        len = lps[len - 1];
                    }
                    else
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }

            return lps;
        }
    }
}