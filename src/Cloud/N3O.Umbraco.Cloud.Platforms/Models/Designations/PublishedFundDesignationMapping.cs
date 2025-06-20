using MuslimHands.Website.Connect.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedFundDesignationMapping : PublishedDesignationTypeMapping<PublishedFundDesignation> {
    protected override void Map(IDesignation src, PublishedFundDesignation dest, MapperContext ctx) {
        var fundDesignation = src as FundDesignation;

        dest.Item = new PublishedDonationItem();
        dest.Item.Id = fundDesignation.DonationItem.Name.Camelize();
        
        if (HasPricing(fundDesignation.DonationItem)) {
            dest.Item.Pricing = new PublishedPricing();
            dest.Item.Pricing.Price = ctx.Map<IPrice, PublishedPrice>(fundDesignation.DonationItem);
            dest.Item.Pricing.Rules = fundDesignation.DonationItem.PriceRules.OrEmpty().Select(ctx.Map<PricingRule, PublishedPricingRule>).ToList();
        }

        dest.SuggestedAmounts = new PublishedSuggestedAmounts();
        dest.SuggestedAmounts.OneTime = fundDesignation.OneTimeSuggestedAmounts
                                                       .OrEmpty()
                                                       .Select(ctx.Map<SuggestedAmount, PublishedSuggestedAmount>)
                                                       .ToList();

        dest.SuggestedAmounts.Recurring = fundDesignation.RecurringSuggestedAmounts
                                                         .OrEmpty()
                                                         .Select(ctx.Map<SuggestedAmount, PublishedSuggestedAmount>)
                                                         .ToList();
    }
}