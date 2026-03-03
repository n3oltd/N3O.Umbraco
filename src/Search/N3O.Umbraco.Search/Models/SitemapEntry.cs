using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Search.Models;

public class SitemapEntry : Value {
    public SitemapEntry(string url,
                        string changeFrequency,
                        float priority,
                        LocalDate lastModified,
                        IReadOnlyDictionary<string, string> cultureVariantUrls) {
        Url = url;
        ChangeFrequency = changeFrequency;
        Priority = priority;
        LastModified = lastModified;
        CultureVariantUrls = cultureVariantUrls;
    }

    public string Url { get; }
    public string ChangeFrequency { get; }
    public float Priority { get; }
    public LocalDate LastModified { get; }
    public IReadOnlyDictionary<string, string> CultureVariantUrls { get; }
}
