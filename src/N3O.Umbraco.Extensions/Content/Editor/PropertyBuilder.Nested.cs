using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

// TODO Migration Review: Nested Content removed in Umbraco 14. Callers must migrate to BlockListPropertyBuilder.
[Obsolete("Nested Content was removed in Umbraco 14. Use BlockListPropertyBuilder instead.", error: true)]
public class NestedPropertyBuilder : PropertyBuilder {
    public NestedPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }

    public IContentBuilder Add(string contentTypeAlias, Guid? customKey = null, int? order = null) {
        throw new NotSupportedException("Nested Content was removed in Umbraco 14. Migrate callers to Block List.");
    }

    public override (object, IPropertyType) Build(string propertyAlias, string parentContentTypeAlias) {
        throw new NotSupportedException("Nested Content was removed in Umbraco 14. Migrate callers to Block List.");
    }
}
