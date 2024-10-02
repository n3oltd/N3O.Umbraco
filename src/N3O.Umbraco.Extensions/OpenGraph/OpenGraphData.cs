using System.Collections.Generic;

namespace N3O.Umbraco.OpenGraph;

public class OpenGraphData : Value {
    public OpenGraphData(string title, string description, string imageUrl) {
        Title = title;
        Description = description;
        ImageUrl = imageUrl;
    }

    public string Title { get; }
    public string Description { get; }
    public string ImageUrl { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Title;
        yield return Description;
        yield return ImageUrl;
    }
}