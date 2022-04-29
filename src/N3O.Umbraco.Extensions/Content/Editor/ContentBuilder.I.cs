using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Content {
    public interface IContentBuilder {
        IDictionary<string, object> Build();
        T Property<T>(string propertyTypeAlias) where T : IPropertyBuilder;
        
        event EventHandler<EventArgs> OnBuilt;
    }
}