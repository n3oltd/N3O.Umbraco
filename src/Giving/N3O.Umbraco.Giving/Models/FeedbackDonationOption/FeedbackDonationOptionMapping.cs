using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackDonationOptionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackDonationOptionContent, FeedbackDonationOptionRes>((_, _) => new FeedbackDonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FeedbackDonationOptionContent src, FeedbackDonationOptionRes dest, MapperContext ctx) {
        dest.Scheme = src.Scheme;
        dest.CustomFields = src.Scheme.CustomFields.OrEmpty().Select(ctx.Map<FeedbackCustomFieldElement, FeedbackCustomFieldRes>).ToList();
        
        dest.DonationPriceHandles = src.DonationPriceHandles
                                       .OrEmpty()
                                       .Select(ctx.Map<PriceHandleElement, PriceHandleRes>)
                                       .ToList();
        
        dest.RegularGivingPriceHandles = src.RegularGivingPriceHandles
                                            .OrEmpty()
                                            .Select(ctx.Map<PriceHandleElement, PriceHandleRes>)
                                            .ToList();
    }
}