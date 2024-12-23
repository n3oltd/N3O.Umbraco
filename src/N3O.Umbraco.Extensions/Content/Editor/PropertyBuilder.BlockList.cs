using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class BlockListPropertyBuilder : PropertyBuilder {
    private readonly List<(string, (IContentBuilder ContentBuilder, Guid Key))> _contentBuilders = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly IContentTypeService _contentTypeService;

    public BlockListPropertyBuilder(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _contentTypeService = serviceProvider.GetRequiredService<IContentTypeService>();
    }

    public IContentBuilder Add(string contentTypeAlias, Guid? customKey = null, int? order = null) {
        var contentBuilder = new ContentBuilder(_serviceProvider);
        var key = customKey ?? Guid.NewGuid();
        
        if (order.HasValue()) {
            _contentBuilders.Insert(order.GetValueOrThrow() - 1, (contentTypeAlias, (contentBuilder, key)));
        } else {
            _contentBuilders.Add((contentTypeAlias, (contentBuilder, key)));
        }

        return contentBuilder;
    }

    public override object Build() {
        var jArray1 = new JArray();
        foreach (var (_, (_, key)) in _contentBuilders) {
            var jObject = new JObject();
            jObject["contentUdi"] = $"umb://element/{key}";

            jArray1.Add(jObject);
        }
        
        var jArray2 = new JArray();
        
        foreach (var (contentTypeAlias, (contentBuilder, key)) in _contentBuilders) {
            var contentType = _contentTypeService.Get(contentTypeAlias);
            
            var jObject = JObject.FromObject(contentBuilder.Build());
            jObject["contentTypeKey"] = contentType.Key.ToString();
            jObject["contentUdi"] = $"umb://element/{key}";

            jArray2.Add(jObject);
        }

        var jResult = new JObject();
        jResult["layout"] = new JObject();
        jResult["layout"]["Umbraco.BlockList"] = jArray1;

        jResult["contentData"] = jArray2;
        jResult["settingsData"] = new JArray();

        return JsonConvert.SerializeObject(jResult);
    }
}
