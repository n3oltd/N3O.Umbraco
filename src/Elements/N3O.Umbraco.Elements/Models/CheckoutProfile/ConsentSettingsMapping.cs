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
        mapper.Define<PreferencesStructureRes, PreferencesSettings>((_, _) => new PreferencesSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PreferencesStructureRes src, PreferencesSettings dest, MapperContext ctx) {
        var channels = src.Channels.Where(RequiresConsent);
        
        dest.PreferenceText = _formatter.Text.Format<Strings>(s => s.ConsentPreferenceText);
        dest.PrivacyText = _formatter.Text.Format<Strings>(s => s.ConsentPrivacyText);
        dest.Options = channels.Select(x => ctx.Map<ChannelPreferencesStructureRes, PreferencesOptionSettings>(x)).ToList();
    }
    
    private bool RequiresConsent(ChannelPreferencesStructureRes channelPreferencesStructureRes) {
        var channels = channelPreferencesStructureRes.CategoryGroups
                                                     .SelectMany(x => x.Categories)
                                                     .HasAny(x => x.ConsentRequired == true);

        return channels;
    }
    
    public class Strings : CodeStrings {
        public string ConsentPreferenceText => "We may contact you occasionally by post or phone to tell you more about our projects, fundraising and appeals. \n"
                                               + "Please let us know if you're also happy to hear from us via:";
        
        public string ConsentPrivacyText => "We promise to keep your details safe and will never sell or swap them with anyone. You can also opt out of communications at any time.";
    }
}