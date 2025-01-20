using N3O.Umbraco.Accounts.Content;
using N3O.Umbraco.Elements.Clients;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class ConsentEntrySettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConsentOptionContent, ConsentOptionSettings>((_, _) => new ConsentOptionSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConsentOptionContent src, ConsentOptionSettings dest, MapperContext ctx) {
        dest.OptIn = src.OptIn;
        dest.Channel = (Channel) Enum.Parse(typeof(Channel), src.Channel.Id, true);
        dest.Categories = src.Categories.Select(x => x.Id).ToList();
    }
}