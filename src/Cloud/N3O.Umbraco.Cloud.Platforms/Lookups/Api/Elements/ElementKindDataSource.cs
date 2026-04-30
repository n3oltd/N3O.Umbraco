using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public abstract class ElementKindDataSource : LookupsDataSource<Element> {
    private readonly ILookups _lookups;

    protected ElementKindDataSource(ILookups lookups) : base(lookups) {
        _lookups = lookups;
    }
    
    public override IEnumerable<DataListItem> GetItems(Dictionary<string, object> config) {
        return _lookups.GetAll<Element>()
                       .Where(x => x.ElementKind == Kind)
                       .Select(ToDataListItem)
                       .OrderBy(x => x.Name)
                       .ToList();
    }
    
    protected override string GetIcon(Element _) => Icon;
    
    protected abstract ElementKind Kind { get; }
    
    protected static string GetOfferingId(Element element) {
        var bits = element.Id.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var elementKind = bits[0].ToEnum<ElementKind>().GetValueOrThrow();

        return bits[1];
    }
}