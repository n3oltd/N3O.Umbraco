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
            var newPropertyValues = new Dictionary<string, object>();

            foreach (var propertyData in contentData.PropertyValues) {
                if (propertyData.Value.PropertyType.PropertyEditorAlias == Aliases.ContentPicker) {
                    if (Guid.TryParse(propertyData.Value.Value?.ToString(), out var parsedGuid)) {
                        var strUdi = Udi.Create("document", parsedGuid).UriValue.ToString();

                        newPropertyValues.Add(propertyData.Key, strUdi);
                    }

                    newPropertyValues.Add(propertyData.Key, propertyData.Value.Value);
                } else if (propertyData.Value.PropertyType.PropertyEditorAlias == Aliases.MultipleTextstring) {
                    if (propertyData.Value.Value is JArray asArray) {
                        var array = asArray.OfType<JObject>().Where(x => x["value"] != null).Select(x => x["value"]!.Value<string>());

                        newPropertyValues.Add(propertyData.Key, string.Join("\r\n", array));
                    }
                } else if (propertyData.Value.Value is JObject jsonObject) {
                    var strValue = JsonConvert.SerializeObject(jsonObject);
                    
                    newPropertyValues.Add(propertyData.Key, strValue);
                } else if (propertyData.Value.Value is List<string> list) {
                    var strValue = JsonConvert.SerializeObject(list);
                    
                    newPropertyValues.Add(propertyData.Key, strValue);
                } else {
                    newPropertyValues.Add(propertyData.Key, propertyData.Value.Value);
                }
            }

            contentData.RawPropertyValues = newPropertyValues;
        }
    }
}