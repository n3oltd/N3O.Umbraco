using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Collections;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Serialization;

namespace N3O.Umbraco.Blocks.Extensions;

public static class BlockValueExtensions {
    private static readonly ConcurrentHashSet<IContentType> ContentTypes = [];

    public static BlockEditorData DeserializeAndClean(this BlockValue blockValue,
                                                      IJsonSerializer jsonSerializer,
                                                      IContentTypeService contentTypeService) {
        var dataConverter = new BlockGridEditorDataConverter(jsonSerializer);
        
        var blockValueAsString = blockValue.ToString();

        if (!blockValueAsString.HasValue()) {
            return null;
        }
        
        if (!blockValueAsString.DetectIsJson()) {
            blockValueAsString = JsonConvert.SerializeObject(blockValue);
        }

        var blockEditorData = dataConverter.Deserialize(blockValueAsString);
        
        return Clean(contentTypeService, blockEditorData);
    }

    private static BlockEditorData Clean(IContentTypeService contentTypeService, BlockEditorData blockEditorData) {
        if (blockEditorData.BlockValue.ContentData.Count == 0) {
            blockEditorData.BlockValue.SettingsData.Clear();

            return null;
        }

        var contentTypePropertyTypes = new Dictionary<string, Dictionary<string, IPropertyType>>();

        var contentTypeKeys = blockEditorData.BlockValue.ContentData
                                             .Select(x => x.ContentTypeKey)
                                             .Union(blockEditorData.BlockValue.SettingsData.Select(x => x.ContentTypeKey))
                                             .Distinct();

        var contentTypesDictionary = GetAllContentTypes(contentTypeService, contentTypeKeys).ToDictionary(x => x.Key);

        foreach (var block in blockEditorData.BlockValue.ContentData.Where(x => blockEditorData.References.Any(r => x.Udi.HasValue() &&
                                                                                                                    r.ContentUdi == x.Udi))) {
            ResolveBlockItemData(block, contentTypePropertyTypes, contentTypesDictionary);
        }

        foreach (var block in blockEditorData.BlockValue.SettingsData.Where(x => blockEditorData.References.Any(r => r.SettingsUdi.HasValue() &&
                                                                                                                     x.Udi.HasValue() &&
                                                                                                                     r.SettingsUdi == x.Udi))) {
            ResolveBlockItemData(block, contentTypePropertyTypes, contentTypesDictionary);
        }

        blockEditorData.BlockValue.ContentData.RemoveAll(x => !x.ContentTypeAlias.HasValue());
        blockEditorData.BlockValue.SettingsData.RemoveAll(x => !x.ContentTypeAlias.HasValue());

        return blockEditorData;
    }

    private static void ResolveBlockItemData(BlockItemData block,
                                             Dictionary<string, Dictionary<string, IPropertyType>> contentTypePropertyTypes, 
                                             IDictionary<Guid, IContentType> contentTypesDictionary) {
        if (!contentTypesDictionary.TryGetValue(block.ContentTypeKey, out var contentType)) {
            return;
        }

        if (!contentTypePropertyTypes.TryGetValue(contentType.Alias, out var propertyTypes)) {
            propertyTypes = contentTypePropertyTypes[contentType.Alias] = contentType.CompositionPropertyTypes.ToDictionary(x => x.Alias, x => x);
        }

        var propValues = new Dictionary<string, BlockItemData.BlockPropertyValue>();

        foreach (var prop in block.RawPropertyValues.ToList()) {
            if (!propertyTypes.TryGetValue(prop.Key, out var propType)) {
                block.RawPropertyValues.Remove(prop.Key);
            } else {
                propValues[prop.Key] = new BlockItemData.BlockPropertyValue(prop.Value, propType);
            }
        }

        block.ContentTypeAlias = contentType.Alias;
        block.PropertyValues = propValues;
    }

    private static IEnumerable<IContentType> GetAllContentTypes(IContentTypeService contentTypeService,
                                                                IEnumerable<Guid> keys) {
        if (!ContentTypes.HasAny()) {
            var contentTypes = contentTypeService.GetAllElementTypes().ToList();

            ContentTypes.AddRangeIfNotExists(contentTypes);
        }

        return ContentTypes.Where(x => keys.Contains(x.Key));
    }
}