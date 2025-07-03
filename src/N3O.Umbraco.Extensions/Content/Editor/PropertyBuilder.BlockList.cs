using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class BlockListPropertyBuilder : PropertyBuilder {
    private readonly List<(string, (IContentBuilder ContentBuilder, Guid Key))> _contentBuilders = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly IContentTypeService _contentTypeService;

    public BlockListPropertyBuilder(IContentTypeService contentTypeService, IServiceProvider serviceProvider)
        : base(contentTypeService) {
        _serviceProvider = serviceProvider;
        _contentTypeService = contentTypeService;
    }

    public IContentBuilder Add(string contentTypeAlias, Guid? customKey = null, int? order = null) {
        var contentBuilder = new ContentBuilder(_serviceProvider, contentTypeAlias);
        var key = customKey ?? Guid.NewGuid();
        
        if (order.HasValue()) {
            _contentBuilders.Insert(order.GetValueOrThrow() - 1, (contentTypeAlias, (contentBuilder, key)));
        } else {
            _contentBuilders.Add((contentTypeAlias, (contentBuilder, key)));
        }

        return contentBuilder;
    }

    public override (object, IPropertyType) Build(string propertyAlias, string parentContentTypeAlias) {
        var blocksList = new JArray();
        
        foreach (var (_, (_, key)) in _contentBuilders) {
            var jObject = new JObject();
            jObject["contentUdi"] = $"umb://element/{key}";

            blocksList.Add(jObject);
        }
        
        var blockItemDatas = new List<BlockItemData>();
        
        foreach (var (contentTypeAlias, (contentBuilder, key)) in _contentBuilders) {
            var contentType = _contentTypeService.Get(contentTypeAlias);

            var blockItemData = new BlockItemData();

            blockItemData.Udi = new GuidUdi("element", key);
            blockItemData.ContentTypeKey = contentType.Key;

            foreach (var entry in JObject.FromObject(contentBuilder.Build())) {
                blockItemData.PropertyValues[entry.Key] = new BlockItemData.BlockPropertyValue(entry.Value,
                                                                                               GetPropertyType(entry.Key, contentTypeAlias));
            }

            blockItemDatas.Add(blockItemData);
        }

        var blockValue = new BlockValue();
        blockValue.Layout = new Dictionary<string, JToken>();
        blockValue.Layout["layout"]["Umbraco.BlockList"] = blocksList;

        blockValue.ContentData = blockItemDatas;
        blockValue.SettingsData = [];

        return (JsonConvert.SerializeObject(blockValue), GetPropertyType(propertyAlias, parentContentTypeAlias));
    }
}
