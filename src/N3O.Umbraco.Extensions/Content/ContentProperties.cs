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
                             IEnumerable<string> compositionAliases,
                             IEnumerable<ContentProperty> properties,
                             IEnumerable<ElementsProperty> elementsProperties) {
        Id = id;
        ParentId = parentId;
        Level = level;
        ContentTypeAlias = contentTypeAlias;
        CompositionAliases = compositionAliases.OrEmpty().ToList();
        Properties = properties.OrEmpty().ToList();
        ElementsProperties = elementsProperties.OrEmpty().ToList();
    }

    public bool IsComposedOf(string alias) {
        return CompositionAliases.Contains(alias, true);
    }

    public Guid Id { get; }
    public int? ParentId { get; }
    public int Level { get; }
    public string ContentTypeAlias { get; }
    public IReadOnlyList<string> CompositionAliases { get; }
    public IReadOnlyList<ContentProperty> Properties { get; }
    public IReadOnlyList<ElementsProperty> ElementsProperties { get; }
}
