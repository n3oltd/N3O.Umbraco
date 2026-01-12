using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Cloud.Platforms.Lookups.Elements;

public class DonationButtonCustomElementsDataSource : IContentmentDataSource {
    private readonly ILookups _lookups;

    public DonationButtonCustomElementsDataSource(ILookups lookups) {
        _lookups = lookups;
    }
    
    public string Name => "Donation Button (Custom) Elements";
    public string Description => "Data source for custom donation button elements";
    public string Icon => "icon-categories";
    public Dictionary<string, object> DefaultValues => default;
    public IEnumerable<ConfigurationField> Fields => default;
    public string Group => "N3O";
    public OverlaySize OverlaySize => OverlaySize.Small;
    
    public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config) {
        return _lookups.GetAll<Element>()
                       .Where(x => x.ElementKind == ElementKind.DonationButtonCustom)
                       .Select(ToDataListItem)
                       .OrderBy(x => x.Name)
                       .ToList();
    }

    public Type GetValueType(Dictionary<string, object> config) {
        return typeof(Element);
    }

    public object ConvertValue(Type type, string value) {
        return _lookups.FindById<Element>(value);
    }
    
    private DataListItem ToDataListItem(Element lookup) {
        var dataListItem = new DataListItem();
        dataListItem.Name = lookup.Name;
        dataListItem.Description = "Custom element";
        dataListItem.Icon = "icon-categories";
        dataListItem.Value = lookup.Id;
        dataListItem.Group = "N3O";

        return dataListItem;
    }
}