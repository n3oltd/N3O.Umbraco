using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models.Settings.DataEntry;

public class ConsentMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConsentEntryContent, UmbracoConsentEntryReq>((_, _) => new UmbracoConsentEntryReq(), Map);
    }

    private void Map(ConsentEntryContent src, UmbracoConsentEntryReq dest, MapperContext ctx) {
        var privacyUrl = src.PrivacyUrl.Content?.AbsoluteUrl() ?? src.PrivacyUrl.Url;
        
        dest.ConsentText = src.ConsentText;
        dest.PrivacyText = src.PrivacyText;
        dest.PrivacyUrl = new Uri(privacyUrl);
    }
}