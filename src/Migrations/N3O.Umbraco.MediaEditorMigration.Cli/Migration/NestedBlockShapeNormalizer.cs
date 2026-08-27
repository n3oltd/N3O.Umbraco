using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.MediaEditorMigration.Cli;

// Rewrites Block List / Block Grid values still in the Umbraco 13 "udi" shape into the v14+ key-based shape.
//
// Umbraco's own 13->17 upgrade rewrites the block values of properties it owns but never traverses INTO
// another editor's value, so a Block List nested inside a Perplex ContentBlocks value keeps the legacy shape
// permanently: properties keyed by alias, a lower-case "layout", and a "udi" per contentData entry instead of
// a "key". v17 still reads it, so the content renders, but the un-upgraded shape may not survive a future major.
//
// Runs last, after the media passes: the target shape is only knowable post-upgrade, and each value entry
// needs an "editorAlias", resolved from the LIVE umbracoDataType rows so the aliases written are the v17 ones
// rather than stale v13 names. Note the target "Layout" is capitalised and retains contentUdi alongside the
// new contentKey, and gains an "expose" array — that is what Umbraco produces, so it is reproduced exactly.
public sealed class NestedBlockShapeNormalizer {
    private static readonly HashSet<string> EntryReservedKeys =
        new(StringComparer.OrdinalIgnoreCase) { "contentTypeKey", "udi", "key", "values" };

    private readonly SqlConnection _cn;
    private readonly SqlTransaction _tx;
    private readonly bool _verbose;
    private readonly Dictionary<(Guid, string), string> _editorAliases = new();

    public NestedBlockShapeNormalizer(SqlConnection cn, SqlTransaction tx, bool verbose) {
        _cn = cn;
        _tx = tx;
        _verbose = verbose;
    }

    public bool Run(RunTotals totals) {
        LoadEditorAliases();

        var rows = Db.Query(_cn,
                            _tx,
                            "SELECT pd.id, pd.textValue, pt.Alias, cv.nodeId, n.text " +
                            "FROM umbracoPropertyData pd " +
                            "INNER JOIN cmsPropertyType pt ON pt.id = pd.propertyTypeId " +
                            "LEFT JOIN umbracoContentVersion cv ON cv.id = pd.versionId " +
                            "LEFT JOIN umbracoNode n ON n.id = cv.nodeId " +
                            "WHERE pd.textValue IS NOT NULL AND pd.textValue <> '' " +
                            "AND CHARINDEX('contentData', pd.textValue) > 0",
                            r => new ShapeRow {
                                Id = r.GetInt32(0),
                                TextValue = r.GetString(1),
                                PropertyAlias = r.IsDBNull(2) ? null : r.GetString(2),
                                NodeId = r.IsDBNull(3) ? (int?) null : r.GetInt32(3),
                                NodeName = r.IsDBNull(4) ? null : r.GetString(4)
                            });

        if (rows.Count == 0) {
            Log.Info("No block values found — nothing to normalise.");

            return true;
        }

        foreach (var row in rows) {
            if (!NormalizeRow(row, totals)) {
                return false;
            }
        }

        return true;
    }

    // (element content-type key, property alias) -> the property's CURRENT editor alias.
    private void LoadEditorAliases() {
        var rows = Db.Query(_cn,
                            _tx,
                            "SELECT LOWER(CONVERT(NVARCHAR(50), n.uniqueId)), pt.Alias, dt.propertyEditorAlias " +
                            "FROM cmsPropertyType pt " +
                            "INNER JOIN cmsContentType ct ON ct.nodeId = pt.contentTypeId " +
                            "INNER JOIN umbracoNode n ON n.id = ct.nodeId " +
                            "INNER JOIN umbracoDataType dt ON dt.nodeId = pt.dataTypeId",
                            r => (Key: r.GetString(0), Alias: r.GetString(1), Editor: r.GetString(2)));

        foreach (var row in rows) {
            if (Guid.TryParse(row.Key, out var contentTypeKey)) {
                _editorAliases[(contentTypeKey, row.Alias)] = row.Editor;
            }
        }

        Log.Verbose(_verbose, $"Loaded {_editorAliases.Count} property editor alias(es) for block normalisation.");
    }

    private bool NormalizeRow(ShapeRow row, RunTotals totals) {
        var header = $"block value id {row.Id} | node {row.NodeDescription} | property '{row.PropertyAlias}'";

        try {
            JToken parsed;

            try {
                parsed = JToken.Parse(row.TextValue);
            } catch {
                return true;
            }

            var converted = 0;
            var result = Walk(parsed, ref converted);

            if (converted > 0) {
                Db.Execute(_cn,
                           _tx,
                           "UPDATE umbracoPropertyData SET textValue = @value WHERE id = @id",
                           ("@value", JsonConvert.SerializeObject(result)),
                           ("@id", row.Id));

                totals.LegacyBlockShapesNormalized += converted;

                Log.Verbose(_verbose,
                            $"Block value {row.Id} (node {row.NodeDescription}, '{row.PropertyAlias}'): " +
                            $"{converted} legacy block value(s) normalised.");
            }

            return true;
        } catch (Exception ex) {
            Log.Item(header, new List<string> { $"FAILED to normalise — {ex.Message}" });
            totals.ValuesFailed++;

            return false;
        }
    }

    private JToken Walk(JToken token, ref int converted) {
        if (token is JObject obj) {
            if (IsLegacyBlockValue(obj)) {
                converted++;

                return Normalize(obj, ref converted);
            }

            foreach (var property in obj.Properties().ToList()) {
                var inner = converted;
                property.Value = Walk(property.Value, ref inner);
                converted = inner;
            }

            return obj;
        }

        if (token is JArray array) {
            for (var i = 0; i < array.Count; i++) {
                var inner = converted;
                array[i] = Walk(array[i], ref inner);
                converted = inner;
            }

            return array;
        }

        if (token is JValue { Type: JTokenType.String } value && value.Value is string text) {
            var trimmed = text.TrimStart();

            if (trimmed.Length == 0 || trimmed[0] != '{' || !text.Contains("contentData")) {
                return token;
            }

            JToken inner;

            try {
                inner = JToken.Parse(text);
            } catch {
                return token;
            }

            var before = converted;
            var walked = Walk(inner, ref converted);

            return converted == before ? token : JsonConvert.SerializeObject(walked);
        }

        return token;
    }

    // Legacy means: a block value whose contentData entries carry "udi" and no "values" array. A value already
    // upgraded by Umbraco has "key" + "values", so it is left untouched.
    private static bool IsLegacyBlockValue(JObject obj) {
        if (obj["contentData"] is not JArray contentData) {
            return false;
        }

        return contentData.OfType<JObject>().Any(x => x["udi"] != null && x["values"] is not JArray);
    }

    private JObject Normalize(JObject legacy, ref int converted) {
        var contentKeys = new List<Guid>();

        var contentData = NormalizeEntries(legacy["contentData"] as JArray, contentKeys, ref converted);
        var settingsData = NormalizeEntries(legacy["settingsData"] as JArray, new List<Guid>(), ref converted);

        var expose = new JArray();

        foreach (var key in contentKeys) {
            expose.Add(new JObject {
                ["contentKey"] = key.ToString(),
                ["culture"] = JValue.CreateNull(),
                ["segment"] = JValue.CreateNull()
            });
        }

        var result = new JObject {
            ["contentData"] = contentData,
            ["settingsData"] = settingsData,
            ["expose"] = expose,
            ["Layout"] = NormalizeLayout(legacy["layout"] ?? legacy["Layout"])
        };

        return result;
    }

    private JArray NormalizeEntries(JArray entries, List<Guid> keys, ref int converted) {
        var output = new JArray();

        if (entries == null) {
            return output;
        }

        foreach (var entry in entries.OfType<JObject>()) {
            var key = GuidFromUdi(entry["udi"]) ?? TryGuid(entry["key"]) ?? Guid.NewGuid();
            keys.Add(key);

            var contentTypeKey = TryGuid(entry["contentTypeKey"]);
            var values = new JArray();

            // Already-upgraded entries can appear alongside legacy ones; carry their values across untouched.
            if (entry["values"] is JArray existing) {
                foreach (var item in existing) {
                    var inner = converted;
                    values.Add(Walk(item, ref inner));
                    converted = inner;
                }
            }

            foreach (var property in entry.Properties()) {
                if (EntryReservedKeys.Contains(property.Name)) {
                    continue;
                }

                string editorAlias = null;

                if (contentTypeKey.HasValue
                    && _editorAliases.TryGetValue((contentTypeKey.Value, property.Name), out var resolved)) {
                    editorAlias = resolved;
                }

                var inner = converted;
                var propertyValue = Walk(property.Value.DeepClone(), ref inner);
                converted = inner;

                values.Add(new JObject {
                    ["editorAlias"] = editorAlias == null ? JValue.CreateNull() : new JValue(editorAlias),
                    ["culture"] = JValue.CreateNull(),
                    ["segment"] = JValue.CreateNull(),
                    ["alias"] = property.Name,
                    ["value"] = propertyValue
                });
            }

            output.Add(new JObject {
                ["contentTypeKey"] = contentTypeKey?.ToString() ?? (string) entry["contentTypeKey"],
                ["key"] = key.ToString(),
                ["values"] = values
            });
        }

        return output;
    }

    // Keeps contentUdi (Umbraco's own upgrade does) and adds the key-based fields alongside it.
    private static JObject NormalizeLayout(JToken layout) {
        var output = new JObject();

        if (layout is not JObject layoutObj) {
            return output;
        }

        foreach (var editor in layoutObj.Properties()) {
            var items = new JArray();

            if (editor.Value is JArray entries) {
                foreach (var item in entries.OfType<JObject>()) {
                    var contentKey = GuidFromUdi(item["contentUdi"]) ?? TryGuid(item["contentKey"]);
                    var settingsKey = GuidFromUdi(item["settingsUdi"]) ?? TryGuid(item["settingsKey"]);

                    items.Add(new JObject {
                        ["contentUdi"] = item["contentUdi"]?.DeepClone()
                                         ?? (contentKey.HasValue
                                             ? new JValue($"umb://element/{contentKey.Value:N}")
                                             : JValue.CreateNull()),
                        ["settingsUdi"] = item["settingsUdi"]?.DeepClone() ?? JValue.CreateNull(),
                        ["contentKey"] = contentKey.HasValue ? new JValue(contentKey.Value.ToString()) : JValue.CreateNull(),
                        ["settingsKey"] = settingsKey.HasValue ? new JValue(settingsKey.Value.ToString()) : JValue.CreateNull()
                    });
                }
            }

            output[editor.Name] = items;
        }

        return output;
    }

    private static Guid? GuidFromUdi(JToken token) {
        if (token?.Type != JTokenType.String) {
            return null;
        }

        var text = (string) token;
        var separator = text.LastIndexOf('/');

        if (separator < 0 || separator == text.Length - 1) {
            return null;
        }

        return Guid.TryParse(text.Substring(separator + 1), out var guid) ? guid : null;
    }

    private static Guid? TryGuid(JToken token) {
        if (token?.Type == JTokenType.String && Guid.TryParse((string) token, out var guid)) {
            return guid;
        }

        return null;
    }

    private sealed class ShapeRow {
        public int Id { get; set; }
        public string TextValue { get; set; }
        public string PropertyAlias { get; set; }
        public int? NodeId { get; set; }
        public string NodeName { get; set; }

        public string NodeDescription => NodeId.HasValue ? $"{NodeId} \"{NodeName}\"" : "(unknown)";
    }
}
