using N3O.Headless.Communications.Clients;
using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class PreferencesSettingsMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;
    
    public PreferencesSettingsMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PreferencesStructureRes, PreferencesSettings>((_, _) => new PreferencesSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PreferencesStructureRes src, PreferencesSettings dest, MapperContext ctx) {
        var preferencesDataEntrySettings = _contentLocator.Single<PreferencesDataEntrySettingsContent>();
        
        var channels = src.Channels.Where(RequiresConsent);
        
        dest.PreferenceText = preferencesDataEntrySettings.PreferenceText;
        dest.PrivacyText = preferencesDataEntrySettings.PrivacyText;
        dest.SkipNoPreferences = preferencesDataEntrySettings.SkipNoPreferences;
        dest.Options = channels.Select(x => ctx.Map<ChannelPreferencesStructureRes, PreferencesOptionSettings>(x)).ToList();
    }
    
    private bool RequiresConsent(ChannelPreferencesStructureRes channelPreferencesStructureRes) {
        var channels = channelPreferencesStructureRes.CategoryGroups
                                                     .SelectMany(x => x.Categories)
                                                     .HasAny(x => x.ConsentRequired == true);

        return channels;
    }
}