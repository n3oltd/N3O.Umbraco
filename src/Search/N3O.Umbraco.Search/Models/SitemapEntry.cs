using N3O.Umbraco.Extensions;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Search.Models;

public class SitemapEntry : Value {
    public const string DefaultSection = "content";

    public SitemapEntry(string url,
                        string culture,
                        string section,
                        LocalDate? lastModified,
                        IReadOnlyDictionary<string, string> alternateUrls) {
        Url = url;
        Culture = culture;
        Section = section.HasValue() ? section : DefaultSection;
        LastModified = lastModified;
        AlternateUrls = alternateUrls;
    }

    public string Url { get; }
    public string Culture { get; }
    public string Section { get; }
    public LocalDate? LastModified { get; }
    public IReadOnlyDictionary<string, string> AlternateUrls { get; }
}
