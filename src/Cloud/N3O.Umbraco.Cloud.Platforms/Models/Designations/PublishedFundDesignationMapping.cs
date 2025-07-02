using Humanizer;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedFundDesignationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundDesignationContent, PublishedFundDesignation>((_, _) => new PublishedFundDesignation(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(FundDesignationContent src, PublishedFundDesignation dest, MapperContext ctx) {
        dest.Item = new PublishedDonationItem();
        dest.Item.Id = src.DonationItem.Name.Camelize();
        
        if (src.DonationItem.HasPricing()) {
            dest.Item.Pricing = ctx.Map<IPricing, PublishedPricing>(src.DonationItem);
        }

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