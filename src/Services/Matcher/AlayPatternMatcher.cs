using System.Text.RegularExpressions;

namespace Services.Matcher
{
    class AlayPatternMatcher
    {
        public static bool IsMatch(string original, string alay)
        {
            string regex = BuildRegexPattern(original);
            return MatchPattern(alay, regex);
        }

        public static bool MatchPattern(string str, string regex)
        {
            return Regex.IsMatch(str, regex, RegexOptions.IgnoreCase);
        }

        public static string BuildRegexPattern(string original)
        {
            string[] words = original.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string pattern = "^";

            for (int i = 0; i < words.Length; i++)
            {
                if (i > 0)
                {
                    pattern += "\\s*";
                }

                string wordPattern = "";
                string word = words[i];
                foreach (char ch in word)
                {
                    if ("aeiouAEIOU".Contains(ch))
                    {
                        string lowerChar = char.ToLower(ch).ToString();
                        string alayVariant = lowerChar switch
                        {
                            "a" => "[aA4]",
                            "e" => "[eE3]",
                            "i" => "[iI1]",
                            "o" => "[oO0]",
                            "u" => "[uvUV]",
                            _ => lowerChar
                        };
                        wordPattern += $"({alayVariant})?";
                    }
                    else
                    {
                        string alayVariant = char.ToLower(ch) switch
                        {
                            'b' => "[bB8]",
                            'g' => "[gG69]",
                            's' => "[sS5]",
                            't' => "[tT7]",
                            _ => $"[{char.ToLower(ch)}{char.ToUpper(ch)}]"
                        };
                        wordPattern += $"({alayVariant})?";
                    }
                }

                pattern += $"({wordPattern})";
            }

            pattern += "$";

            return pattern;
        }

        // Not used
        static string buildPattern(string original)
        {
            string[] words = original.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string pattern = "^";

            for (int i = 0; i < words.Length; i++)
            {
                if (i > 0)
                {
                    pattern += "\\s*";
                }

                string word = words[i];
                foreach (char ch in word)
                {
                    if ("aeiouAEIOU".Contains(ch))
                    {
                        string lowerChar = char.ToLower(ch).ToString();
                        string alayVariant = lowerChar switch
                        {
                            "a" => "[aA4]",
                            "e" => "[eE3]",
                            "i" => "[iI1]",
                            "o" => "[oO0]",
                            "u" => "[uU]",
                            _ => lowerChar
                        };
                        pattern += $"({alayVariant})?";
                    }
                    else
                    {
                        string alayVariant = char.ToLower(ch) switch
                        {
                            'g' => "[gG6]",
                            's' => "[sS5]",
                            't' => "[tT7]",
                            _ => $"[{char.ToLower(ch)}{char.ToUpper(ch)}]"
                        };
                        pattern += $"({alayVariant})?";
                    }
                }
            }

            pattern += "$";
            Console.WriteLine("pattern: " + pattern);

            return pattern;
        }
    }
}