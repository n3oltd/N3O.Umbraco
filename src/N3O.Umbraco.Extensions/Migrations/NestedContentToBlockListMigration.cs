using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence;

namespace N3O.Umbraco.Migrations;

// Migrates all Nested Content property data and data types to Block List.
// Run this once when upgrading from Umbraco 13 to 17 on existing databases.
// PREREQUISITES:
//   1. All content types using Nested Content must already have a corresponding Block List
//      data type configured with the same element types.
//   2. Back up the database before running.
//   3. Test on a database copy first.
public class NestedContentToBlockListMigration : AsyncMigrationBase {
    public NestedContentToBlockListMigration(IMigrationContext context) : base(context) { }

    protected override Task MigrateAsync() {
        Logger.LogInformation("Starting Nested Content → Block List data migration");

        var db = Context.Database;

        // Step 1: Find all data types using the NestedContent editor
        var nestedDataTypes = db.Fetch<DataTypeRow>(
            "SELECT nodeId AS Id, config FROM umbracoDataType WHERE propertyEditorAlias = 'Umbraco.NestedContent'");

        if (!nestedDataTypes.Any()) {
            Logger.LogInformation("No Nested Content data types found — migration not required");
            return Task.CompletedTask;
        }

        Logger.LogInformation("Found {Count} Nested Content data types to migrate", nestedDataTypes.Count);

        // Build a mapping: dataTypeId → list of content-type aliases defined in its config
        var dataTypeAliasMap = new Dictionary<int, List<string>>();
        foreach (var dt in nestedDataTypes) {
            try {
                var config = JObject.Parse(dt.Config ?? "{}");
                var contentTypes = config["contentTypes"] as JArray ?? [];
                dataTypeAliasMap[dt.Id] = contentTypes.Select(x => (string) x["ncAlias"]).Where(a => a != null).ToList();
            } catch (Exception ex) {
                Logger.LogWarning(ex, "Could not parse config for data type {Id}", dt.Id);
                dataTypeAliasMap[dt.Id] = new List<string>();
            }
        }

        // Step 2: Collect content-type key lookups (alias → key)
        var allContentTypeAliases = dataTypeAliasMap.Values.SelectMany(x => x).Distinct().ToList();
        var contentTypeKeys = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);

        if (allContentTypeAliases.Any()) {
            var placeholders = string.Join(",", allContentTypeAliases.Select((_, i) => $"@{i}"));
            var rows = db.Fetch<ContentTypeKeyRow>(
                $"SELECT ct.alias AS Alias, n.uniqueId AS [Key] " +
                $"FROM cmsContentType ct INNER JOIN umbracoNode n ON n.id = ct.nodeId " +
                $"WHERE ct.alias IN ({placeholders})",
                allContentTypeAliases.Cast<object>().ToArray());

            foreach (var row in rows) {
                contentTypeKeys[row.Alias] = row.Key;
            }
        }

        // Steps 3 & 4 run in a single transaction so a structural failure cannot leave data types
        // flipped to Block List while their stored values are still Nested Content JSON.
        using var transaction = db.GetTransaction();

        // Step 3: Migrate each Nested Content data type → Block List
        foreach (var dt in nestedDataTypes) {
            try {
                var aliases = dataTypeAliasMap[dt.Id];
                var blocks = aliases
                    .Where(a => contentTypeKeys.ContainsKey(a))
                    .Select(a => new {
                        contentElementTypeKey = contentTypeKeys[a],
                        settingsElementTypeKey = (Guid?) null,
                        label = (string) null,
                        labelTemplate = (string) null,
                        editorSize = "medium",
                        forceHideContentEditorInOverlay = false,
                        thumbnail = (string) null,
                        iconColor = (string) null,
                        backgroundColor = (string) null,
                        stylesheet = (string) null
                    })
                    .ToList();

                var blockListConfig = new JObject {
                    ["blocks"] = JArray.FromObject(blocks),
                    ["validationLimit"] = JObject.FromObject(new { min = (int?) null, max = (int?) null }),
                    ["useLiveEditing"] = false,
                    ["useInlineEditingAsDefault"] = false
                };

                db.Execute(
                    "UPDATE umbracoDataType SET propertyEditorAlias = 'Umbraco.BlockList', propertyEditorUiAlias = 'Umb.PropertyEditorUi.BlockList', config = @0 WHERE nodeId = @1",
                    JsonConvert.SerializeObject(blockListConfig),
                    dt.Id);

                Logger.LogInformation("Migrated data type {Id} ({Aliases}) to Block List",
                    dt.Id, string.Join(", ", aliases));
            } catch (Exception ex) {
                Logger.LogError(ex, "Failed to migrate data type {Id}", dt.Id);
            }
        }

        // Step 4: Migrate property values
        var dataTypeIds = nestedDataTypes.Select(d => d.Id).ToList();

        // Find property types that reference those data types
        var propTypePlaceholders = string.Join(",", dataTypeIds.Select((_, i) => $"@{i}"));
        var propertyTypes = db.Fetch<PropertyTypeRow>(
            $"SELECT id, dataTypeId FROM cmsPropertyType WHERE dataTypeId IN ({propTypePlaceholders})",
            dataTypeIds.Cast<object>().ToArray());

        if (!propertyTypes.Any()) {
            Logger.LogInformation("No property values to migrate");

            transaction.Complete();

            return Task.CompletedTask;
        }

        var propTypePlaceholders2 = string.Join(",", propertyTypes.Select((_, i) => $"@{i}"));
        var propValues = db.Fetch<PropertyDataRow>(
            $"SELECT id, propertyTypeId, textValue FROM umbracoPropertyData WHERE propertyTypeId IN ({propTypePlaceholders2}) AND textValue IS NOT NULL AND textValue != ''",
            propertyTypes.Select(p => (object) p.Id).ToArray());

        Logger.LogInformation("Migrating {Count} property values", propValues.Count);

        var dataTypeById = nestedDataTypes.ToDictionary(d => d.Id);
        var propTypeDataTypeMap = propertyTypes.ToDictionary(p => p.Id, p => p.DataTypeId);

        var migrated = 0;
        var failed = 0;

        foreach (var pv in propValues) {
            try {
                if (string.IsNullOrWhiteSpace(pv.TextValue)) continue;

                if (!propTypeDataTypeMap.TryGetValue(pv.PropertyTypeId, out var dtId)) continue;
                if (!dataTypeById.TryGetValue(dtId, out var dt)) continue;

                var aliases = dataTypeAliasMap[dtId];
                var transformed = TransformNestedContentToBlockList(pv.TextValue, contentTypeKeys, aliases);

                if (transformed != null) {
                    db.Execute("UPDATE umbracoPropertyData SET textValue = @0 WHERE id = @1", transformed, pv.Id);
                    migrated++;
                }
            } catch (Exception ex) {
                Logger.LogWarning(ex, "Failed to migrate property value {Id}", pv.Id);
                failed++;
            }
        }

        Logger.LogInformation("Property value migration complete: {Migrated} migrated, {Failed} failed",
            migrated, failed);

        transaction.Complete();

        return Task.CompletedTask;
    }

    // Converts Nested Content JSON array to Block List JSON object.
    // Nested Content format:  [{"key":"guid","ncContentTypeAlias":"alias","name":"x","prop":"val",...}]
    // Block List format:      {"layout":{"Umbraco.BlockList":[{"contentKey":"guid"}]},"contentData":[...],"settingsData":[]}
    private static string TransformNestedContentToBlockList(
        string nestedJson,
        Dictionary<string, Guid> contentTypeKeys,
        List<string> knownAliases) {
        JToken parsed;
        try {
            parsed = JToken.Parse(nestedJson);
        } catch {
            return null;
        }

        if (parsed is not JArray items || !items.Any()) {
            return JsonConvert.SerializeObject(new JObject {
                ["layout"] = new JObject {
                    ["Umbraco.BlockList"] = new JArray()
                },
                ["contentData"] = new JArray(),
                ["settingsData"] = new JArray()
            });
        }

        var layoutItems = new JArray();
        var contentData = new JArray();

        foreach (var item in items) {
            var key = (string) item["key"];
            var alias = (string) item["ncContentTypeAlias"];

            if (!Guid.TryParse(key, out var keyGuid)) {
                keyGuid = Guid.NewGuid();
            }

            if (!contentTypeKeys.TryGetValue(alias ?? "", out var contentTypeKey)) {
                continue;
            }

            layoutItems.Add(new JObject { ["contentKey"] = keyGuid });

            var contentEntry = new JObject {
                ["key"] = keyGuid,
                ["contentTypeKey"] = contentTypeKey,
                ["contentTypeAlias"] = alias
            };

            // Copy all properties except the NC-specific ones
            foreach (var prop in item.Children<JProperty>()) {
                if (prop.Name != "key" && prop.Name != "ncContentTypeAlias" && prop.Name != "name") {
                    contentEntry[prop.Name] = prop.Value;
                }
            }

            contentData.Add(contentEntry);
        }

        var blockListValue = new JObject {
            ["layout"] = new JObject {
                ["Umbraco.BlockList"] = layoutItems
            },
            ["contentData"] = contentData,
            ["settingsData"] = new JArray()
        };

        return JsonConvert.SerializeObject(blockListValue);
    }

    // DTO classes for NPoco queries
    private class DataTypeRow {
        public int Id { get; set; }
        public string Config { get; set; }
    }

    private class ContentTypeKeyRow {
        public string Alias { get; set; }
        public Guid Key { get; set; }
    }

    private class PropertyTypeRow {
        public int Id { get; set; }
        public int DataTypeId { get; set; }
    }

    private class PropertyDataRow {
        public int Id { get; set; }
        public int PropertyTypeId { get; set; }
        public string TextValue { get; set; }
    }
}
