using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedFeedbackDesignationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackDesignationContent, PublishedFeedbackDesignation>((_, _) => new PublishedFeedbackDesignation(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(FeedbackDesignationContent src, PublishedFeedbackDesignation dest, MapperContext ctx) {
       dest.Scheme = new PublishedFeedbackScheme();
       dest.Scheme.Id = src.Scheme.Name;

       dest.CustomFieldDefinitions = src.Scheme
                                        .CustomFields
                                        .OrEmpty()
                                        .Select(ctx.Map<IFeedbackCustomFieldDefinition, PublishedCustomFieldDefinition>)
                                        .ToList();

       if (src.Scheme.HasPricing()) {
           dest.Pricing = src.Scheme.Pricing.IfNotNull(ctx.Map<IPricing, PublishedPricing>);
       }
    }
}
