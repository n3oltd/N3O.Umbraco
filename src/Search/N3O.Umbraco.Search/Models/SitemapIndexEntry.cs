using NodaTime;

namespace N3O.Umbraco.Search.Models;

public class SitemapIndexEntry : Value {
    public SitemapIndexEntry(string location, LocalDate? lastModified) {
        Location = location;
        LastModified = lastModified;
    }

    public string Location { get; }
    public LocalDate? LastModified { get; }
}
