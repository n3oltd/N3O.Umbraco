using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Currency = N3O.Umbraco.Financial.Currency;
using OurGiftType = N3O.Umbraco.Cloud.Platforms.Lookups.GiftType;
using PlatformsCurrency = N3O.Umbraco.Cloud.Platforms.Clients.Currency;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class DonationFormStateReqMapping {
    private CartItemReq GetCartItemReq(IHoldDonationFormStateContent src, IReadOnlyDictionary<string, string> tags) {
        var currency = _lookups.GetAll<Currency>().Single(x => x.IsBaseCurrency).Code.ToEnum<PlatformsCurrency>();

        var cartItem = new CartItemReq();
        cartItem.Id = Guid.NewGuid().ToString();
        cartItem.Currency = currency;

        var allocationIntent = src.FormState.ToAllocationIntentReq(currency);
        allocationIntent.PlatformsContribution = GetPlatformsContributionInfoReq(src);

        SetCartItemAllocation(cartItem, allocationIntent, src.FormState.SuggestedGiftType);

        if (tags.HasAny()) {
            cartItem.Tags = tags.ToTagCollectionReq();
        }

        return cartItem;
    }

    private void SetCartItemAllocation(CartItemReq cartItem,
                                       AllocationIntentReq allocation,
                                       OurGiftType suggestedGiftType) {
        if (suggestedGiftType == GiftTypes.Recurring) {
            cartItem.Type = CartItemType.NewRegularGiving;
            cartItem.NewRegularGiving = new NewRegularGivingWithOptionsReq();
            cartItem.NewRegularGiving.Allocation = allocation;
        } else {
            cartItem.Type = CartItemType.NewDonation;
            cartItem.NewDonation = new NewDonationReq();
            cartItem.NewDonation.Allocation = allocation;
        }
    }

    private PlatformsContributionInfoReq GetPlatformsContributionInfoReq(IHoldDonationFormStateContent src) {
        var platformsContribution = new PlatformsContributionInfoReq();
        platformsContribution.ContributionId = Guid.NewGuid().ToString();

        src.PopulateContributionInfo(_cdnClient, platformsContribution);

        return platformsContribution;
    }
}
