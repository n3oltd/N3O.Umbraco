using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Content;

public interface IContentBuilder {
    IDictionary<string, object> Build();
    T Property<T>(string propertyAlias) where T : IPropertyBuilder;
    string ContentTypeAlias { get; }
    
    event EventHandler<EventArgs> OnBuilt;
}
