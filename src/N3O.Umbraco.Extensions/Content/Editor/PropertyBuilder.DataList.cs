using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class DataListPropertyBuilder : PropertyBuilder {
    public DataListPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void SetLookups<T>(IEnumerable<T> values) where T : ILookup {
        SetLookups(values.OrEmpty().ToArray());
    }

    public void SetLookups<T>(params T[] values) where T : ILookup {
        var value = $"[ {string.Join(", ", values.Select(x => x.Id.Quote()))} ]";
        
        Value = value;
    }
}
