namespace Services.Matcher
{
    class LevenshteinDistance
    {
        public static int Get(string text1, string text2)
        {
            int text1Length = text1.Length;
            int text2Length = text2.Length;
            int[,] dp = new int[text1Length + 1, text2Length + 1];

            for (int i = 0; i <= text1Length; i++)
            {
                for (int j = 0; j <= text2Length; j++)
                {
                    if (i == 0)
                    {
                        dp[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        dp[i, j] = i;
                    }
                    else
                    {
                        int cost = (text1[i - 1] == text2[j - 1]) ? 0 : 1;
                        dp[i, j] = Math.Min(Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1), dp[i - 1, j - 1] + cost);
                    }
                }
            }

            return dp[text1Length, text2Length];
        }
    }
}