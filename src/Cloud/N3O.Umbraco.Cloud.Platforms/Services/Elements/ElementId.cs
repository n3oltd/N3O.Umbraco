using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Cloud.Platforms;

public static class ElementId {
    public static string Generate(ElementKind kind, EntityId id) {
        return $"{kind.ToEnumString()}/{id}";
    }
    
    public static (ElementKind, EntityId) Parse(string id) {
        var bits = id.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var elementKind = bits[0].ToEnum<ElementKind>().GetValueOrThrow();

        return (elementKind, bits[1]);
    }
}