namespace Models
{
    public class SearchResult
    {
        public Biodata? biodata { get; set; }
        public string? algorithm { get; set; }
        public required int similarity { get; set; }
        public required double execTime { get; set; }
        public string? imagePath { get; set; }

        public static SearchResult getNotFoundResult(double execTime) {
            return new SearchResult{
                biodata = null,
                algorithm = null,
                similarity = 0,
                execTime = execTime,
                imagePath = null,
            };
        }
    }
}