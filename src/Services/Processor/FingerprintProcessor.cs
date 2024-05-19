using Models;

namespace Services.Processor
{
    public static class FingerprintProcessor
    {
        public static List<Fingerprint> Process(List<FingerprintPath> fingerprintPaths)
        {
            List<Fingerprint> processed = new List<Fingerprint>();

            foreach (var fingerprintPath in fingerprintPaths)
            {
                processed.Add(new Fingerprint
                {
                    PixelAscii = ImageProcessor.ReadAllPixelToAscii(fingerprintPath.ImagePath),
                    Nama = fingerprintPath.Nama
                });
            }

            return processed;
        }
    }
}