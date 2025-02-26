using N3O.Headless.Communications.Clients;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class ConsentSettingsMapping : IMapDefinition {
    private readonly IFormatter _formatter;
    public ConsentSettingsMapping(IFormatter formatter) {
        _formatter = formatter;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PreferencesStructureRes, ConsentSettings>((_, _) => new ConsentSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PreferencesStructureRes src, ConsentSettings dest, MapperContext ctx) {
        var channels = src.Channels.Where(RequiresConsent);
        
        dest.Text = _formatter.Text.Format<Strings>(s => s.ConsentText);
        dest.Options = channels.Select(x => ctx.Map<ChannelPreferencesStructureRes, ConsentOptionSettings>(x)).ToList();
    }
    
    private bool RequiresConsent(ChannelPreferencesStructureRes channelPreferencesStructureRes) {
        var channels = channelPreferencesStructureRes.CategoryGroups
                                                     .SelectMany(x => x.Categories)
                                                     .HasAny(x => x.ConsentRequired == true);

        return channels;
    }
    
    public class Strings : CodeStrings {
        public string ConsentText => "We may occasionally contact you by post or phone about our projects, "
                                     + "fundraising, and appeals. I am happy to be contacted via:";
    }
}