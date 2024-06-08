using System.Diagnostics;
using Models;
using Database;
using Services.Processor;
using Services.Matcher;

namespace Services
{
    public class Searcher
    {
        private static (string?, int, string?) SearchByLD(string fullPixelAscii, List<Fingerprint> fingerprints)
        {
            double threshold = 0.6;
            double maxSimilarity = 0;
            Fingerprint result = fingerprints[0];

            foreach (var fingerprint in fingerprints)
            {
                var distance = LevenshteinDistance.Get(fullPixelAscii, fingerprint.PixelAscii);
                double similarity = 1.0 - (double)distance / Math.Max(fullPixelAscii.Length, fingerprint.PixelAscii.Length);
                if (similarity >= maxSimilarity && similarity >= threshold)
                {
                    result = fingerprint;
                    maxSimilarity = similarity;
                }
            }

            int similarityPercentage = (int)Math.Round(maxSimilarity * 100);
            if (similarityPercentage < threshold) {
                return (null, similarityPercentage, null);
            }

            return (result.Nama, similarityPercentage, result.ImagePath);
        }

        private static Biodata GetBiodataByRegexAlayOrLD(string nama)
        {
            var biodatas = DbController.GetDecryptedBiodataList();
            foreach (var biodata in biodatas)
            {
                if (AlayPatternMatcher.IsMatch(nama, biodata.NamaAlay))
                {
                    biodata.NamaAlay = nama;
                    return biodata;
                }
            }

            Console.WriteLine("[DEBUG] Not Found Name by Regex, Searching by LD");

            double maxSimilarity = 0;
            Biodata mostSimilarBiodata = biodatas[0];

            foreach (var biodata in biodatas)
            {
                var distance = LevenshteinDistance.Get(nama, biodata.NamaAlay);
                double similarity = 1.0 - (double)distance / Math.Max(nama.Length, biodata.NamaAlay.Length);
                if (similarity >= maxSimilarity)
                {
                    mostSimilarBiodata = biodata;
                    maxSimilarity = similarity;
                }
            }

            mostSimilarBiodata.NamaAlay = nama;
            return mostSimilarBiodata;
        }

        private static (string?, int, string?) SearchByKMP(string pattern, List<Fingerprint> fingerprints)
        {
            foreach (var fingerprint in fingerprints)
            {
                if (KnuthMorrisPratt.Search(pattern, fingerprint.PixelAscii))
                {
                    return (fingerprint.Nama, 100, fingerprint.ImagePath);
                }
            }
            return (null, 0, null);
        }

        private static (string?, int, string?) SearchByBM(string pattern, List<Fingerprint> fingerprints)
        {
            foreach (var fingerprint in fingerprints)
            {
                if (BoyerMoore.Search(pattern, fingerprint.PixelAscii))
                {
                    return (fingerprint.Nama, 100, fingerprint.ImagePath);
                }
            }
            return (null, 0, null);
        }

        public static SearchResult GetResult(string imagePath, string algo)
        {
            var fullPixelAscii = ImageProcessor.ReadAllPixelToAscii(imagePath);
            var fingerprintPaths = DbController.GetDecryptedFingerprintList();
            var fingerprints = FingerprintProcessor.Process(fingerprintPaths);
            var pattern = ImageProcessor.ReadBestPixelToAscii(imagePath);
            string? namaAlay;
            int percentage;
            string? resultImagePath;
            string? usedAlgorithm = algo;

            Stopwatch stopwatch = new Stopwatch();

            if (algo.Equals("KMP", StringComparison.OrdinalIgnoreCase))
            {
                stopwatch.Start();
                (namaAlay, percentage, resultImagePath) = SearchByKMP(pattern, fingerprints);
                stopwatch.Stop();
                if (namaAlay != null) Console.WriteLine("[DEBUG] Found KMP"); else Console.WriteLine("[DEBUG] Not Found KMP");
                if (namaAlay == null)
                {
                    usedAlgorithm = "LD";
                    stopwatch.Start();
                    (namaAlay, percentage, resultImagePath) = SearchByLD(fullPixelAscii, fingerprints);
                    stopwatch.Stop();
                    if (namaAlay != null) Console.WriteLine("[DEBUG] Found LD"); else Console.WriteLine("[DEBUG] Not Found LD");
                    if (namaAlay == null) return SearchResult.getNotFoundResult(stopwatch.ElapsedMilliseconds);
                }
            }
            else
            {
                stopwatch.Start();
                (namaAlay, percentage, resultImagePath) = SearchByBM(pattern, fingerprints);
                stopwatch.Stop();
                if (namaAlay != null) Console.WriteLine("[DEBUG] Found BM"); else Console.WriteLine("[DEBUG] Not Found BM");
                if (namaAlay == null)
                {
                    usedAlgorithm = "LD";
                    stopwatch.Start();
                    (namaAlay, percentage, resultImagePath) = SearchByLD(fullPixelAscii, fingerprints);
                    stopwatch.Stop();
                    if (namaAlay != null) Console.WriteLine("[DEBUG] Found LD"); else Console.WriteLine("[DEBUG] Not Found LD");
                    if (namaAlay == null) return SearchResult.getNotFoundResult(stopwatch.ElapsedMilliseconds);
                }
            }

            var resultBiodata = GetBiodataByRegexAlayOrLD(namaAlay);
            if (resultBiodata == null) return SearchResult.getNotFoundResult(stopwatch.ElapsedMilliseconds);

            return new SearchResult{
                biodata = resultBiodata,
                algorithm = usedAlgorithm,
                similarity = percentage,
                execTime = stopwatch.ElapsedMilliseconds,
                imagePath =  resultImagePath
            };
        }
    }
}