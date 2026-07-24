using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using static Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Blocks.Extensions;

public static class BlockItemDataExtensions {
    public static void FormatBlockData(this List<BlockItemData> blockData) {
        foreach (var contentData in blockData.OrEmpty()) {
            foreach (var value in contentData.Values.OrEmpty()) {
                var editorAlias = value.PropertyType?.PropertyEditorAlias;

                if (editorAlias == Aliases.ContentPicker) {
                    if (Guid.TryParse(value.Value?.ToString(), out var parsedGuid)) {
                        value.Value = Udi.Create("document", parsedGuid).UriValue.ToString();
                    }
                } else if (editorAlias == Aliases.MultipleTextstring) {
                    if (value.Value is JArray asArray) {
                        var array = asArray.OfType<JObject>()
                                           .Where(x => x["value"] != null)
                                           .Select(x => x["value"]!.Value<string>());

                        value.Value = string.Join("\r\n", array);
                    }
                } else if (value.Value is JObject jsonObject) {
                    value.Value = JsonConvert.SerializeObject(jsonObject);
                } else if (value.Value is List<string> list) {
                    value.Value = JsonConvert.SerializeObject(list);
                }
            }
        }
    }
}
