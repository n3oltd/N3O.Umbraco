using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedFeedbackDesignationMapping : IMapDefinition {
    private readonly ILookups _lookups;
    public PublishedFeedbackDesignationMapping(ILookups lookups) {
        _lookups = lookups;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackDesignationContent, PublishedFeedbackDesignation>((_, _) => new PublishedFeedbackDesignation(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(FeedbackDesignationContent src, PublishedFeedbackDesignation dest, MapperContext ctx) {
       dest.Scheme = new PublishedFeedbackScheme();
       dest.Scheme.Id = src.GetScheme(_lookups).Id;

       dest.CustomFieldDefinitions = src.GetScheme(_lookups)
                                        .CustomFields
                                        .OrEmpty()
                                        .Select(ctx.Map<IFeedbackCustomFieldDefinition, PublishedCustomFieldDefinition>)
                                        .ToList();

       if (src.GetScheme(_lookups).HasPricing()) {
           dest.Pricing = src.GetScheme(_lookups).Pricing.IfNotNull(ctx.Map<IPricing, PublishedPricing>);
       }
    }
}
