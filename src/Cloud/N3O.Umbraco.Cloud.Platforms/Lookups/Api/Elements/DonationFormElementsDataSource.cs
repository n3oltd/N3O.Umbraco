using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;

namespace N3O.Umbraco.Cloud.Platforms.Lookups.Elements;

public class DonationFormElementsDataSource : IContentmentDataSource {
    private readonly ILookups _lookups;

    protected DonationFormElementsDataSource(ILookups lookups) {
        _lookups = lookups;
    }
    
    public string Name => "Donation Form Elements";
    public string Description => "Data source for donation form elements";
    public string Icon => "icon-categories";
    public Dictionary<string, object> DefaultValues => default;
    public IEnumerable<ConfigurationField> Fields => default;
    public string Group => "N3O";
    public OverlaySize OverlaySize => OverlaySize.Small;
    
    public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config) {
        return _lookups.GetAll<Element>().Where(x => x.ElementKind == ElementKind.DonationFormCustom).Select(ToDataListItem).OrderBy(x => x.Name).ToList();
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
        dataListItem.Description = "Data source for donation form elements";
        dataListItem.Icon = "icon-categories";
        dataListItem.Value = lookup.Id;
        dataListItem.Group = "N3O";

        return dataListItem;
    }
}