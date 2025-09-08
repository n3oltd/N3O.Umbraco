using System.Collections.Generic;

namespace N3O.Umbraco.Metadata;

public class MetadataEntry : Value {
    public MetadataEntry(string name, string content) {
        Name = name;
        Content = content;
    }

    public string Name { get; }
    public string Content { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Name;
        yield return Content;
    }
}
