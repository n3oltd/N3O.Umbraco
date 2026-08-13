using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Typesense.Attributes;
using System;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Models;

public abstract class SearchDocument : Value {
    [Field("id", FieldType.String, true, true)]
    public string Id { get; set; }

    [Field("content_key", FieldType.String, true, true)]
    public Guid ContentKey { get; set; }

    [Field("culture", FieldType.String, false, true, facet: true)]
    public string Culture { get; set; }

    public static string GetId(Guid contentKey, string culture) {
        return culture.HasValue() ? $"{contentKey}_{culture}" : contentKey.ToString();
    }

    public static bool TryParseId(string id, out Guid contentKey, out string culture) {
        contentKey = Guid.Empty;
        culture = null;

        if (!id.HasValue()) {
            return false;
        }

        var separatorIndex = id.IndexOf('_');
        var contentKeyPart = separatorIndex < 0 ? id : id.Substring(0, separatorIndex);

        if (!Guid.TryParse(contentKeyPart, out contentKey)) {
            return false;
        }

        if (separatorIndex >= 0) {
            culture = id.Substring(separatorIndex + 1);
        }

        return true;
    }
}
