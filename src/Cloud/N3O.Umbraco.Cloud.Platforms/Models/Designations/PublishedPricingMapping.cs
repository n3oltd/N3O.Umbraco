using N3O.Umbraco.Cloud.Platforms.Content;
using MuslimHands.Website.Connect.Clients;
using Umbraco.Cms.Core.Mapping;
using IPrice = N3O.Umbraco.Cloud.Platforms.Content.IPrice;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedPricingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPrice, PublishedPrice>((_, _) => new PublishedPrice(), MapPrice);
        mapper.Define<PricingRule, PublishedPricingRule>((_, _) => new PublishedPricingRule(), MapPricingRule);
    }

    private void MapPricingRule(PricingRule src, PublishedPricingRule dest, MapperContext ctx) {
        dest.Price = ctx.Map<IPrice, PublishedPrice>(src);
        dest.FundDimensions = ctx.Map<IFundDimensionsValues, PublishedFundDimensionValues>(src);
    }

    private void MapPrice(IPrice src, PublishedPrice dest, MapperContext ctx) {
        dest.Amount = (double) src.PriceAmount;
        dest.Locked = src.PriceLocked;
    }
}
