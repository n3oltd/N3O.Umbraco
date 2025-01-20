using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Elements.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class EmailEntrySettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<EmailDataEntrySettings, EmailEntrySettings>((_, _) => new EmailEntrySettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(EmailDataEntrySettings src, EmailEntrySettings dest, MapperContext ctx) {
        dest.Required = src.Required;
        dest.Validate = src.Validate;
    }
}