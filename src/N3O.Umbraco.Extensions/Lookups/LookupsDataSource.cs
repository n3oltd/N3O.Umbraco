using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Lookups;

public abstract class LookupsDataSource<T> : IDataListSourceValueConverter where T : INamedLookup {
    private readonly ILookups _lookups;

    protected LookupsDataSource(ILookups lookups) {
        _lookups = lookups;
    }
    
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract string Icon { get; }
    public Dictionary<string, object> DefaultValues => default;
    public IEnumerable<ConfigurationField> Fields => default;
    public string Group => "N3O";
    public OverlaySize OverlaySize => OverlaySize.Small;
    
    public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config) {
        return _lookups.GetAll<T>().Select(ToDataListItem).OrderBy(x => x.Name).ToList();
    }

    public Type GetValueType(Dictionary<string, object> config) {
        return typeof(T);
    }

    public object ConvertValue(Type type, string value) {
        return _lookups.FindById<T>(value);
    }

    protected virtual string GetDescription(T lookup) => null;
    protected abstract string GetIcon(T lookup);
    
    private DataListItem ToDataListItem(T lookup) {
        var dataListItem = new DataListItem();
        dataListItem.Name = lookup.Name;
        dataListItem.Description = GetDescription(lookup);
        dataListItem.Icon = GetIcon(lookup);
        dataListItem.Value = lookup.Id;
        dataListItem.Group = "N3O";

        return dataListItem;
    }
}
