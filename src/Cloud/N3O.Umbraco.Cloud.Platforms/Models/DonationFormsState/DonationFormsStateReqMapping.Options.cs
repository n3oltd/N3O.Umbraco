using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using GiftType = N3O.Umbraco.Cloud.Platforms.Clients.GiftType;
using OurGiftType = N3O.Umbraco.Cloud.Platforms.Lookups.GiftType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class ElementDonationFormsStateReqMapping {
    private DonationFormOptionsReq GetDonationFormOptionsReq(MapperContext ctx,
                                                             IFundDimensionValues fundDimensionValues,
                                                             params (OurGiftType GiftType, IEnumerable<SuggestedAmountElement> SuggestedAmounts)[] suggestedAmountsElements) {
        var options = new DonationFormOptionsReq();
        
        options.FundDimensions = new HiddenFundDimensionsReq();
        options.FundDimensions.Dimension1 = fundDimensionValues.HasValue(x => x.Dimension1?.Name);
        options.FundDimensions.Dimension2 = fundDimensionValues.HasValue(x => x.Dimension2?.Name);
        options.FundDimensions.Dimension3 = fundDimensionValues.HasValue(x => x.Dimension3?.Name);
        options.FundDimensions.Dimension4 = fundDimensionValues.HasValue(x => x.Dimension4?.Name);

        options.SuggestedAmounts = GetDonationFormSuggestedAmountsReq(ctx, suggestedAmountsElements);

        return options;
    }

    private List<DonationFormSuggestedAmountsReq> GetDonationFormSuggestedAmountsReq(MapperContext ctx,
                                                                                     params (OurGiftType GiftType, IEnumerable<SuggestedAmountElement> SuggestedAmounts)[] suggestedAmountsElements) {
        var items = new List<DonationFormSuggestedAmountsReq>();

        foreach (var suggestedAmountsElement in suggestedAmountsElements) {
            var req = new DonationFormSuggestedAmountsReq();
            req.GiftType = suggestedAmountsElement.GiftType.ToEnum<GiftType>();
            req.Amounts = suggestedAmountsElement.SuggestedAmounts.Select(ctx.Map<SuggestedAmountElement, DonationFormSuggestedAmountReq>).ToList();
            
            items.Add(req);
        }

        return items;
    }
}