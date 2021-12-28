using NodaTime;

namespace N3O.Umbraco.Search.Models {
    public class SitemapEntry : Value {
        public SitemapEntry(string url, string changeFrequency, float priority, LocalDate lastModified) {
            Url = url;
            ChangeFrequency = changeFrequency;
            Priority = priority;
            LastModified = lastModified;
        }

        public string Url { get; }
        public string ChangeFrequency { get; }
        public float Priority { get; }
        public LocalDate LastModified { get; }
    }
}