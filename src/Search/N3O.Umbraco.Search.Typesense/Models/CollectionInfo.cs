using System.Collections.Generic;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Models;

public class CollectionInfo : Value {
    public CollectionInfo(CollectionName name,
                          int version,
                          IEnumerable<string> contentTypeAliases,
                          IEnumerable<Field> fields) {
        Name = name;
        Version = version;
        ContentTypeAliases = contentTypeAliases;
        Fields = fields;
    }

    public CollectionName Name { get; }
    public int Version { get; }
    public IEnumerable<string> ContentTypeAliases { get; }
    public IEnumerable<Field> Fields { get; }
}