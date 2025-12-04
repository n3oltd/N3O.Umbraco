using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class FeedbackOfferingReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackOfferingContent, FeedbackOfferingReq>((_, _) => new FeedbackOfferingReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(FeedbackOfferingContent src, FeedbackOfferingReq dest, MapperContext ctx) {
       dest.Scheme = src.Scheme.Id;
    }
}
