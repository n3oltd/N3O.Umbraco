using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class ConsentEntrySettingsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConsentDataEntrySettings, ConsentSettings>((_, _) => new ConsentSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConsentDataEntrySettings src, ConsentSettings dest, MapperContext ctx) {
        //dest.Text = src.Number.Required;
        dest.Options = new List<ConsentOptionSettings>();

        foreach (var consentOption in src.ConsentOptions) {
            var option = new ConsentOptionSettings();
            //option.Default
            option.Channel = (Channel) Enum.Parse(typeof(Channel), consentOption.Channel.Id, true);
            option.Categories = consentOption.Categories.Select(x => x.Id).ToList();
        }
    }
}