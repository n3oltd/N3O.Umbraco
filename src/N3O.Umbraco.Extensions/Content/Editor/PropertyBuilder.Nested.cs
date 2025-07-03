using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class NestedPropertyBuilder : PropertyBuilder {
    private readonly List<(string, (IContentBuilder ContentBuilder, Guid Key))> _contentBuilders = [];
    private readonly IServiceProvider _serviceProvider;

    public NestedPropertyBuilder(IContentTypeService contentTypeService, IServiceProvider serviceProvider)
        : base(contentTypeService) {
        _serviceProvider = serviceProvider;
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
        var rowValues = new List<NestedContentRowValue>();

        var index = 1;
        foreach (var (contentTypeAlias, (contentBuilder, key)) in _contentBuilders) {
            var rowValue = new NestedContentRowValue();
            rowValue.Id = key;
            rowValue.Name = $"Item {index}";
            rowValue.ContentTypeAlias = contentTypeAlias;
            rowValue.RawPropertyValues = contentBuilder.Build();

            rowValues.Add(rowValue);

            index++;
        }

        return (JsonConvert.SerializeObject(rowValues), GetPropertyType(propertyAlias, parentContentTypeAlias));
    }

    // https://github.com/umbraco/Umbraco-CMS/blob/864fd2a45a6afba067cc2d03cafed0d54b602889/src/Umbraco.Infrastructure/PropertyEditors/NestedContentPropertyEditor.cs#L442
    private class NestedContentRowValue {
        [JsonProperty("key")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ncContentTypeAlias")]
        public string ContentTypeAlias { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> RawPropertyValues { get; set; }
    }
}
