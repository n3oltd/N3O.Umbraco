using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Mapping;
using Country = N3O.Umbraco.Elements.Clients.Country;
using FieldSettings = N3O.Umbraco.Elements.Clients.FieldSettings;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class AddressEntrySettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<AddressDataEntrySettings, AddressEntrySettings>((_, _) => new AddressEntrySettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(AddressDataEntrySettings src, AddressEntrySettings dest, MapperContext ctx) {
        dest.Line1 = GetFieldSettings(src.Line1.Label, src.Line1.Required);
        dest.Line2 = GetFieldSettings(src.Line2.Label, src.Line2.Required);
        dest.Line3 = GetFieldSettings(src.Line3.Label, src.Line3.Required);
        dest.Country = GetFieldSettings(src.Country.Label, src.Country.Required);
        dest.Locality = GetFieldSettings(src.Locality.Label, src.Locality.Required);
        dest.AdministrativeArea = GetFieldSettings(src.AdministrativeArea.Label, src.AdministrativeArea.Required);
        dest.PostalCode = GetFieldSettings(src.PostalCode.Label, src.PostalCode.Required);
        dest.AddressLookupApiKey = src.LookupApiKey;
        dest.DomesticCountry = (Country) Enum.Parse(typeof(Country), src.DefaultCountry.Id, true);
        dest.Layout = src.Layout.HasValue() ?
                          (AddressLayout) Enum.Parse(typeof(AddressLayout), src.Layout.Id, true)
                          : null;
    }
    
    private static FieldSettings GetFieldSettings(string label, bool required) {
        var fieldSettings = new FieldSettings();
        fieldSettings.Label = label;
        fieldSettings.Required = required;
        
        return fieldSettings;
    }
}