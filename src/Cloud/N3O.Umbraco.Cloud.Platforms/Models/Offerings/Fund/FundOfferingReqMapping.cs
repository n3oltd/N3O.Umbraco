using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class FundOfferingReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundOfferingContent, FundOfferingReq>((_, _) => new FundOfferingReq(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(FundOfferingContent src, FundOfferingReq dest, MapperContext ctx) {
        dest.Item = src.DonationItem.Id;
        
        if (src.OneTimeSuggestedAmounts.HasAny() || src.RecurringSuggestedAmounts.HasAny()) {
            dest.SuggestedAmounts = new SuggestedAmountsReq();
            dest.SuggestedAmounts.OneTime = src.OneTimeSuggestedAmounts
                                               .OrEmpty()
                                               .Select(ctx.Map<SuggestedAmountElement, SuggestedAmountReq>)
                                               .ToList();

            dest.SuggestedAmounts.Recurring = src.RecurringSuggestedAmounts
                                                 .OrEmpty()
                                                 .Select(ctx.Map<SuggestedAmountElement, SuggestedAmountReq>)
                                                 .ToList();
        }
    }
}