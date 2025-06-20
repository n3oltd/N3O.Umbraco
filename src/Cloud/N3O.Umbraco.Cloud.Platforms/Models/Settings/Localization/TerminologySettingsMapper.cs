using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class OurTerminologiesMapper : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<TerminologiesContent, PublishedTerminology>((_, _) => new PublishedTerminology(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(TerminologiesContent src, PublishedTerminology dest, MapperContext ctx) {
        dest.Campaigns = src.Campaigns;
    }
}