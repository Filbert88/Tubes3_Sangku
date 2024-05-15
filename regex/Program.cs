using System;
using System.Text.RegularExpressions;

class Program{
    static void Main(){
        string original = "Indonesia Raya";

        string combinedPattern = BuildPattern1(original);

        string[] testCases = {
            "bintanG DwI mArthen",
            "B1nt4n6 Dw1 M4rthen",
            "Bntng Dw Mrthen",
            "b1ntN6 Dw mrthn",
            "ez",
            "ofeifoiewfhviwvnwv",
            "B1ntang",
            "BiNtANg",
            "Bntg",
            "b1ntNG",
            "1nd0n3514 R4y4",
            "1ndonesia Raya",
            "Ind0nesia Rya",
            "1ndo R4ya",
            "Indonesia",
            "Indonesi4 Ray4",
            "aaah",
            "IndoNesia Raya",
            "InDoNeSiA rAyA",
            "1ndon3sia R4ya",
            "1nd0n351a R4y4",
            "Indo Raya",
            "Ind Raya",
            "1ndonesia R4ya",
            "Ind0 R4ya",
            "1ndo Ray4",
            "Ind0n3sia Rya",
            "1ndonesia",
            "Indones1a",
            "Indon3siaRaya",
            "Indonesia R",
            "I R",
            "1 R",
            "aah",
            "xyz"
        };

        foreach (var testCase in testCases){
            if (Regex.IsMatch(testCase, combinedPattern, RegexOptions.IgnoreCase)) {
                Console.WriteLine($"Match found: {testCase}");
            } else{
                Console.WriteLine($"No match found: {testCase}");
            }
        }
    }

    static string BuildPattern(string original){
        string[] words = original.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        string pattern = "^"; 

        for (int i = 0; i < words.Length; i++) {
            if (i > 0) {
                pattern += "\\s*"; 
            }

            string word = words[i];
            foreach (char ch in word) {
                if ("aeiouAEIOU".Contains(ch)){
                    string lowerChar = char.ToLower(ch).ToString();
                    string alayVariant = lowerChar switch {
                        "a" => "[aA4]",
                        "e" => "[eE3]",
                        "i" => "[iI1]",
                        "o" => "[oO0]",
                        "u" => "[uU]",
                        _ => lowerChar
                    };
                    pattern += $"({alayVariant})?";
                } else {
                    string alayVariant = char.ToLower(ch) switch {
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

    static string BuildPattern1(string original){
        string[] words = original.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        string pattern = "^"; 

        for (int i = 0; i < words.Length; i++) {
            if (i > 0) {
                pattern += "\\s*";
            }

            string wordPattern = "";
            string word = words[i];
            foreach (char ch in word) {
                if ("aeiouAEIOU".Contains(ch)){
                    string lowerChar = char.ToLower(ch).ToString();
                    string alayVariant = lowerChar switch {
                        "a" => "[aA4]",
                        "e" => "[eE3]",
                        "i" => "[iI1]",
                        "o" => "[oO0]",
                        "u" => "[uU]",
                        _ => lowerChar
                    };
                    wordPattern += $"({alayVariant})?";
                } else {
                    string alayVariant = char.ToLower(ch) switch {
                        'g' => "[gG6]",
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
        Console.WriteLine("pattern: " + pattern);

        return pattern;
    }
}
