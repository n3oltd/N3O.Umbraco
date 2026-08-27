using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace N3O.Umbraco.MediaEditorMigration.Cli;

// Resolves the built-in Image and File media types (their content-type node id + the property-type ids of the
// metadata properties) once, so the MediaNodeFactory can insert property data rows by id.
// Only used by the --target mediapicker route, which is the only one that creates media nodes.
public sealed class MediaTypes {
    public MediaTypeInfo Image { get; private set; }
    public MediaTypeInfo File { get; private set; }

    public static MediaTypes Resolve(SqlConnection cn, SqlTransaction tx) {
        return new MediaTypes {
            Image = ResolveOne(cn, tx, "Image"),
            File = ResolveOne(cn, tx, "File")
        };
    }

    private static MediaTypeInfo ResolveOne(SqlConnection cn, SqlTransaction tx, string alias) {
        // The media type's content-type id is its umbracoNode id.
        var contentTypeId = Db.Scalar<int>(cn, tx,
            "SELECT ct.nodeId FROM cmsContentType ct WHERE ct.alias = @alias", ("@alias", alias));

        if (contentTypeId == 0) {
            throw new InvalidOperationException(
                $"Built-in media type '{alias}' was not found in this database (cmsContentType.alias). " +
                $"Cannot create media nodes without it.");
        }

        var propertyTypeIds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        var rows = Db.Query(cn, tx,
            "SELECT pt.Alias, pt.id FROM cmsPropertyType pt WHERE pt.contentTypeId = @ctId",
            r => new { Alias = r.GetString(0), Id = r.GetInt32(1) },
            ("@ctId", contentTypeId));

        foreach (var row in rows) {
            propertyTypeIds[row.Alias] = row.Id;
        }

        if (!propertyTypeIds.ContainsKey("umbracoFile")) {
            throw new InvalidOperationException(
                $"Media type '{alias}' has no 'umbracoFile' property — unexpected schema; cannot continue.");
        }

        return new MediaTypeInfo {
            Alias = alias,
            ContentTypeId = contentTypeId,
            PropertyTypeIds = propertyTypeIds
        };
    }
}

public sealed class MediaTypeInfo {
    public string Alias { get; set; }
    public int ContentTypeId { get; set; }
    public Dictionary<string, int> PropertyTypeIds { get; set; }

    public int FilePropertyTypeId => PropertyTypeIds["umbracoFile"];

    public bool TryGetPropertyTypeId(string alias, out int id) => PropertyTypeIds.TryGetValue(alias, out id);
}
