using N3O.Headless.Communications.Clients;
using N3O.Umbraco.Elements.Clients;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Channel = N3O.Umbraco.Elements.Clients.Channel;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class ConsentEntrySettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ChannelPreferencesStructureRes, PreferencesOptionSettings>((_, _) => new PreferencesOptionSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ChannelPreferencesStructureRes src, PreferencesOptionSettings dest, MapperContext ctx) {
        dest.Channel = (Channel) Enum.Parse(typeof(Channel), src.Channel.ToString(), true);
        dest.Categories = src.CategoryGroups
                             .SelectMany(x => x.Categories)
                             .Where(x => x.ConsentRequired == true)
                             .Select(x => x.Category)
                             .ToList();

        dest.OptIn = false;
    }
}