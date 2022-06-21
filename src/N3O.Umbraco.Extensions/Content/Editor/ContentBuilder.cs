using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Content;

public class ContentBuilder : IContentBuilder {
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, IPropertyBuilder> _propertyBuilders = new();

    public ContentBuilder(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public T Property<T>(string propertyTypeAlias) where T : IPropertyBuilder {
        var propertyBuilder = _serviceProvider.GetRequiredService<T>();

        _propertyBuilders.Add(propertyTypeAlias, propertyBuilder);
        
        return propertyBuilder;
    }

    public IDictionary<string, object> Build() {
        var propertyValues = new Dictionary<string, object>();
        
        foreach (var (name, builder) in _propertyBuilders) {
            propertyValues[name] = builder.Build();
        }

        RaiseBuilt();

        return propertyValues;
    }
    
    private void RaiseBuilt() {
        var handler = OnBuilt;
        handler?.Invoke(this, EventArgs.Empty);
    }
    
    public event EventHandler<EventArgs> OnBuilt;
}
