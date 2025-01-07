using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Content;

public class ContentBuilder : IContentBuilder {
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, IPropertyBuilder> _propertyBuilders = new();

    public ContentBuilder(IServiceProvider serviceProvider, string contentTypeAlias) {
        ContentTypeAlias = contentTypeAlias;
        _serviceProvider = serviceProvider;
    }

    public T Property<T>(string propertyAlias) where T : IPropertyBuilder {
        var propertyBuilder = _serviceProvider.GetRequiredService<T>();

        _propertyBuilders.Add(propertyAlias, propertyBuilder);
        
        return propertyBuilder;
    }

    public IDictionary<string, object> Build() {
        var propertyValues = new Dictionary<string, object>();
        
        foreach (var (propertyAlias, builder) in _propertyBuilders) {
            var (propertyValue, _) = builder.Build(propertyAlias, ContentTypeAlias);
            
            propertyValues[propertyAlias] = propertyValue;
        }

        RaiseBuilt();

        return propertyValues;
    }
    
    private void RaiseBuilt() {
        var handler = OnBuilt;
        handler?.Invoke(this, EventArgs.Empty);
    }
    
    public event EventHandler<EventArgs> OnBuilt;
    
    public string ContentTypeAlias { get; }
}
