using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoConsentEntryReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ConsentEntryContent, UmbracoConsentEntryReq>((_, _) => new UmbracoConsentEntryReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ConsentEntryContent src, UmbracoConsentEntryReq dest, MapperContext ctx) {
        dest.ConsentText = src.ConsentText;
        dest.PrivacyText = src.PrivacyText;
        dest.PrivacyUrl = src.PrivacyUrl.GetPublishedUri();
    }
}