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
    
    
    protected void Map(FundDesignationContent src, PublishedFundDesignation dest, MapperContext ctx) {
        dest.Item = new PublishedDonationItem();
        dest.Item.Id = src.DonationItem.Name.Camelize();
        
        if (src.DonationItem.HasPricing()) {
            dest.Item.Pricing = new PublishedPricing();
            dest.Item.Pricing.Price = ctx.Map<IPrice, PublishedPrice>(src.DonationItem);
            dest.Item.Pricing.Rules = src.DonationItem.PriceRules.OrEmpty().Select(ctx.Map<IPricingRule, PublishedPricingRule>).ToList();
        }

        dest.SuggestedAmounts = new PublishedSuggestedAmounts();
        dest.SuggestedAmounts.OneTime = src.OneTimeSuggestedAmounts
                                           .OrEmpty()
                                           .Select(ctx.Map<SuggestedAmountContent, PublishedSuggestedAmount>)
                                           .ToList();

        dest.SuggestedAmounts.Recurring = src.RecurringSuggestedAmounts
                                             .OrEmpty()
                                             .Select(ctx.Map<SuggestedAmountContent, PublishedSuggestedAmount>)
                                             .ToList();
    }
}