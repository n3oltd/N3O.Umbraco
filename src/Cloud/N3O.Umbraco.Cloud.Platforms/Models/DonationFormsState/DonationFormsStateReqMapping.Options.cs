using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using GiftType = N3O.Umbraco.Cloud.Platforms.Clients.GiftType;
using OurGiftType = N3O.Umbraco.Cloud.Platforms.Lookups.GiftType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class ElementDonationFormsStateReqMapping {
    private DonationFormOptionsReq GetDonationFormOptionsReq(MapperContext ctx,
                                                             OfferingContent offering) {
        var oneTimeSuggestedAmounts = offering.Fund?.OneTimeSuggestedAmounts.OrEmpty().ToList();
        var recurringSuggestedAmounts = offering.Fund?.RecurringSuggestedAmounts.OrEmpty().ToList();
        
        var options = new DonationFormOptionsReq();

        if (oneTimeSuggestedAmounts.HasAny() || recurringSuggestedAmounts.HasAny()) {
            options.SuggestedAmounts = GetDonationFormSuggestedAmountsReq(ctx,
                                                                          (GiftTypes.OneTime, oneTimeSuggestedAmounts),
                                                                          (GiftTypes.Recurring, recurringSuggestedAmounts));
            
        }
        
        return options;
    }

    private List<DonationFormSuggestedAmountsReq> GetDonationFormSuggestedAmountsReq(MapperContext ctx,
                                                                                     params (OurGiftType GiftType, IEnumerable<SuggestedAmountElement> SuggestedAmounts)[] suggestedAmountsElements) {
        var items = new List<DonationFormSuggestedAmountsReq>();

        foreach (var suggestedAmountsElement in suggestedAmountsElements) {
            if (suggestedAmountsElement.SuggestedAmounts.HasAny()) {
                var req = new DonationFormSuggestedAmountsReq();
                req.GiftType = suggestedAmountsElement.GiftType.ToEnum<GiftType>();
                req.Amounts = suggestedAmountsElement.SuggestedAmounts.Select(ctx.Map<SuggestedAmountElement, DonationFormSuggestedAmountReq>).ToList();
            
                items.Add(req);
            }
        }

        return items;
    }
}