using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Elements.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class TermsOfServiceSettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<TermsOfServicesSettingsContent, TermsOfServiceSettings>((_, _) => new TermsOfServiceSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(TermsOfServicesSettingsContent src, TermsOfServiceSettings dest, MapperContext ctx) {
        dest.Text = src.Text;
        dest.Url = src.Url.ToString();
    }
}