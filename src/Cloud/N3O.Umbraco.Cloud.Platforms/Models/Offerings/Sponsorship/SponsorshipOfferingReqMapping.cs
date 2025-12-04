using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class SponsorshipOfferingReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipOfferingContent, SponsorshipOfferingReq>((_, _) => new SponsorshipOfferingReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(SponsorshipOfferingContent src, SponsorshipOfferingReq dest, MapperContext ctx) {
        dest.Scheme = src.Scheme.Id;
    }
}
