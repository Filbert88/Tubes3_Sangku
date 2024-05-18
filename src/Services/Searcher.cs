using System.Diagnostics;
using Models;
using Database;
using Services.Processor;
using Services.Matcher;

namespace Services
{
    public class Searcher
    {
        private static (string?, int) SearchByLD(string fullPixelAscii, List<Fingerprint> fingerprints)
        {
            double threshold = 0.6;
            double maxSimilarity = 0;
            string? result = null;

            foreach (var fingerprint in fingerprints)
            {
                var distance = LevenshteinDistance.Get(fullPixelAscii, fingerprint.PixelAscii);
                double similarity = 1.0 - (double)distance / Math.Max(fullPixelAscii.Length, fingerprint.PixelAscii.Length); // Fixed similarity calculation
                if (similarity >= maxSimilarity && similarity >= threshold)
                {
                    result = fingerprint.NamaAlay;
                    maxSimilarity = similarity;
                }
            }

            int similarityPercentage = (int)Math.Round(maxSimilarity * 100);

            return (result, similarityPercentage);
        }

        private static Biodata? GetBiodataByRegexAlay(string namaAlay)
        {
            var biodatas = DbController.GetBiodataList();
            foreach (var biodata in biodatas)
            {
                if (AlayPatternMatcher.IsMatch(biodata.Nama, namaAlay))
                {
                    return biodata;
                }
            }
            return null;
        }

        private static (string?, int) SearchByKMP(string pattern, List<Fingerprint> fingerprints)
        {
            foreach (var fingerprint in fingerprints)
            {
                if (KnuthMorrisPratt.Search(pattern, fingerprint.PixelAscii).Count() > 0)
                {
                    return (fingerprint.NamaAlay, 100);
                }
            }
            return (null, 0);
        }

        private static (string?, int) SearchByBM(string pattern, List<Fingerprint> fingerprints)
        {
            foreach (var fingerprint in fingerprints)
            {
                if (BoyerMoore.Search(pattern, fingerprint.PixelAscii).Count() > 0)
                {
                    return (fingerprint.NamaAlay, 100);
                }
            }
            return (null, 0);
        }

        public static (Biodata?, string?, double, long) GetResult(string imagePath, string algo)
        {
            var fullPixelAscii = ImageProcessor.ReadAllPixelToAscii(imagePath);
            var fingerprintPaths = DbController.GetFingerprintList();
            var fingerprints = FingerprintProcessor.Process(fingerprintPaths);
            var pattern = ImageProcessor.ReadBestPixelToAscii(imagePath);
            string? namaAlay;
            double percentage;
            string? usedAlgorithm = algo;

            Stopwatch stopwatch = new Stopwatch();

            if (algo.Equals("KMP", StringComparison.OrdinalIgnoreCase))
            {
                stopwatch.Start();
                (namaAlay, percentage) = SearchByKMP(pattern, fingerprints);
                stopwatch.Stop();
                if (namaAlay != null) Console.WriteLine("[DEBUG] Found KMP"); else Console.WriteLine("[DEBUG] Not Found KMP");
                if (namaAlay == null)
                {
                    usedAlgorithm = "LD";
                    stopwatch.Start();
                    (namaAlay, percentage) = SearchByLD(fullPixelAscii, fingerprints);
                    stopwatch.Stop();
                    if (namaAlay != null) Console.WriteLine("[DEBUG] Found LD"); else Console.WriteLine("[DEBUG] Not Found LD");
                    if (namaAlay == null) return (null, null, 0, stopwatch.ElapsedMilliseconds);
                }
            }
            else
            {
                stopwatch.Start();
                (namaAlay, percentage) = SearchByBM(pattern, fingerprints);
                stopwatch.Stop();
                if (namaAlay != null) Console.WriteLine("[DEBUG] Found BM"); else Console.WriteLine("[DEBUG] Not Found BM");
                if (namaAlay == null)
                {
                    usedAlgorithm = "LD";
                    stopwatch.Start();
                    (namaAlay, percentage) = SearchByLD(fullPixelAscii, fingerprints);
                    stopwatch.Stop();
                    if (namaAlay != null) Console.WriteLine("[DEBUG] Found LD"); else Console.WriteLine("[DEBUG] Not Found LD");
                    if (namaAlay == null) return (null, null, 0, stopwatch.ElapsedMilliseconds);
                }
            }

            var resultBiodata = GetBiodataByRegexAlay(namaAlay);
            if (resultBiodata != null) Console.WriteLine("[DEBUG] Found Real Name"); else Console.WriteLine("[DEBUG] Not Found Real Name");
            if (resultBiodata == null) return (null, null, 0, stopwatch.ElapsedMilliseconds);

            return (resultBiodata, usedAlgorithm, percentage, stopwatch.ElapsedMilliseconds);
        }
    }
}