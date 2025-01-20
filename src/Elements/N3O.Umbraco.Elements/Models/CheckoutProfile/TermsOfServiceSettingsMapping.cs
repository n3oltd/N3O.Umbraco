using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class TermsOfServiceSettingsMapping : IMapDefinition {
    private readonly IFormatter _formatter;

    public TermsOfServiceSettingsMapping(IFormatter formatter) {
        _formatter = formatter;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<TermsDataEntrySettingsContent, TermsOfServiceSettings>((_, _) => new TermsOfServiceSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(TermsDataEntrySettingsContent src, TermsOfServiceSettings dest, MapperContext ctx) {
        dest.Text = _formatter.Text.Format<Strings>(s => s.TermsOfServiceText);
        dest.Url = src.Link.Content?.AbsoluteUrl() ?? src.Link.Url;
    }
    
    public class Strings : CodeStrings {
        public string TermsOfServiceText => "Agree to";
    }
}