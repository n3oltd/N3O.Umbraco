using N3O.Umbraco.Cloud.Platforms.Content;
using MuslimHands.Website.Connect.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class OurTerminologiesMapper : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsTerminologies, PublishedTerminology>((_, _) => new PublishedTerminology(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PlatformsTerminologies src, PublishedTerminology dest, MapperContext ctx) {
        dest.Campaigns = src.Campaigns;
    }
}