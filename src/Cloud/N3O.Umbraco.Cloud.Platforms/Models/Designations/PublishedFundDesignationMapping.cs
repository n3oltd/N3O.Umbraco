using Humanizer;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedFundDesignationMapping : IMapDefinition {
    private readonly ILookups _lookups;
    
    public PublishedFundDesignationMapping(ILookups lookups) {
        _lookups = lookups;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundDesignationContent, PublishedFundDesignation>((_, _) => new PublishedFundDesignation(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(FundDesignationContent src, PublishedFundDesignation dest, MapperContext ctx) {
        var donationItem = src.GetDonationItem(_lookups);
        
        dest.Item = new PublishedDonationItem();
        dest.Item.Id = donationItem.Id;
        dest.Item.Pricing = donationItem.Pricing.IfNotNull(ctx.Map<IPricing, PublishedPricing>);

        if (src.OneTimeSuggestedAmounts.HasAny() || src.RecurringSuggestedAmounts.HasAny()) {
            dest.SuggestedAmounts = new PublishedSuggestedAmounts();
            dest.SuggestedAmounts.OneTime = src.OneTimeSuggestedAmounts
                                               .OrEmpty()
                                               .Select(ctx.Map<SuggestedAmountElement, PublishedSuggestedAmount>)
                                               .ToList();

            dest.SuggestedAmounts.Recurring = src.RecurringSuggestedAmounts
                                                 .OrEmpty()
                                                 .Select(ctx.Map<SuggestedAmountElement, PublishedSuggestedAmount>)
                                                 .ToList();
        }
    }
}