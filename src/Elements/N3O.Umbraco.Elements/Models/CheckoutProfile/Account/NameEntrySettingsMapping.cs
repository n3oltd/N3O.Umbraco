using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;
using FieldSettings = N3O.Umbraco.Elements.Clients.FieldSettings;
using TextFieldSettings = N3O.Umbraco.Elements.Clients.TextFieldSettings;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class NameEntrySettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<NameDataEntrySettings, NameEntrySettings>((_, _) => new NameEntrySettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(NameDataEntrySettings src, NameEntrySettings dest, MapperContext ctx) {
        dest.Title = GetFieldSettings(src.Title.Label, src.Title.Required);
        //TODO length
        dest.First = GetTextFieldSettings(src.FirstName.Label, src.FirstName.Required, 6, src.FirstName.Capitalisation);
        dest.Last = GetTextFieldSettings(src.LastName.Label, src.LastName.Required, 6, src.LastName.Capitalisation);
        //dest.Layout = GetFieldSettings(settings.Address.Country.Label, settings.Address.Country.Required);
    }
    
    private static FieldSettings GetFieldSettings(string label, bool required) {
        var fieldSettings = new FieldSettings();
        fieldSettings.Label = label;
        fieldSettings.Required = required;
        
        return fieldSettings;
    }
    
    private static TextFieldSettings GetTextFieldSettings(string label, bool required, int minimumLength, Capitalisation capitalisation) {
        var fieldSettings = new TextFieldSettings();
        fieldSettings.Label = label;
        fieldSettings.Required = required;
        fieldSettings.MinimumLength = minimumLength;
        //fieldSettings.Transformation = required;
        
        return fieldSettings;
    }
}