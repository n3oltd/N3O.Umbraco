using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Content;

public class ContentProperties {
    public ContentProperties(Guid id,
                             int? parentId,
                             int level,
                             string contentTypeAlias,
                             IEnumerable<ContentProperty> properties,
                             IEnumerable<NestedContentProperty> nestedContentProperties) {
        Id = id;
        ParentId = parentId;
        Level = level;
        ContentTypeAlias = contentTypeAlias;
        Properties = properties.OrEmpty().ToList();
        NestedContentProperties = nestedContentProperties.OrEmpty().ToList();
    }

    public Guid Id { get; }
    public int? ParentId { get; }
    public int Level { get; }
    public string ContentTypeAlias { get; }
    public IReadOnlyList<ContentProperty> Properties { get; }
    public IReadOnlyList<NestedContentProperty> NestedContentProperties { get; }
}
