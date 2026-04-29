using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CampaignContent = N3O.Umbraco.Cloud.Platforms.Content.CampaignContent;
using CrossSellContent = N3O.Umbraco.Cloud.Platforms.Content.CrossSellContent;
using Currency = N3O.Umbraco.Financial.Currency;
using OfferingContent = N3O.Umbraco.Cloud.Platforms.Content.OfferingContent;
using OurGiftType = N3O.Umbraco.Cloud.Platforms.Lookups.GiftType;
using PlatformsCurrency = N3O.Umbraco.Cloud.Platforms.Clients.Currency;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class DonationFormsStateReqMapping {
    private CartItemReq GetCartItemReq(CampaignContent campaign,
                                       OfferingContent offering,
                                       IFundDimensionValues fundDimensionValues,
                                       IReadOnlyDictionary<string, string> tags) {
        var currency = _lookups.GetAll<Currency>().Single(x => x.IsBaseCurrency).Code.ToEnum<PlatformsCurrency>();

        var cartItem = new CartItemReq();
        cartItem.Id = Guid.NewGuid().ToString();
        cartItem.Currency = currency;

        var allocationIntent = offering.FormState.ToAllocationIntentReq(fundDimensionValues, currency);
        allocationIntent.PlatformsContribution = GetPlatformsContributionInfoReq(campaign, offering);

        SetCartItemAllocation(cartItem, allocationIntent, offering.FormState.SuggestedGiftType);

        if (tags.HasAny()) {
            cartItem.Tags = tags.ToTagCollectionReq();
        }

        return cartItem;
    }

    private CartItemReq GetCartItemReq(CampaignContent campaign,
                                       CrossSellContent crossSell,
                                       IFundDimensionValues fundDimensionValues,
                                       IReadOnlyDictionary<string, string> tags) {
        var currency = _lookups.GetAll<Currency>().Single(x => x.IsBaseCurrency).Code.ToEnum<PlatformsCurrency>();

        var cartItem = new CartItemReq();
        cartItem.Id = Guid.NewGuid().ToString();
        cartItem.Currency = currency;

        var allocationIntent = crossSell.FormState.ToAllocationIntentReq(fundDimensionValues, currency);

        if (campaign.HasValue()) {
            allocationIntent.PlatformsContribution = GetBasePlatformsContributionInfoReq(campaign);
        }

        SetCartItemAllocation(cartItem, allocationIntent, crossSell.SuggestedGiftType);

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

    private PlatformsContributionInfoReq GetPlatformsContributionInfoReq(CampaignContent campaign,
                                                                         OfferingContent offering) {
        var platformsContribution = GetBasePlatformsContributionInfoReq(campaign);

        platformsContribution.Offering = new OfferingInfoReq();
        platformsContribution.Offering.Id = offering.Key.ToString();
        platformsContribution.Offering.Name = offering.Name.ToString();

        return platformsContribution;
    }

    private PlatformsContributionInfoReq GetBasePlatformsContributionInfoReq(CampaignContent campaign) {
        var publishedCampaign = _cdnClient.DownloadPublishedContentAsync<PublishedCampaign>(PublishedFileKinds.Campaign,
                                                                                            $"{campaign.Key}.json",
                                                                                            JsonSerializers.Simple)
                                          .GetAwaiter().GetResult();

        var platformsContribution = new PlatformsContributionInfoReq();
        platformsContribution.ContributionId = Guid.NewGuid().ToString();
        platformsContribution.Campaign = new CampaignInfoReq();
        platformsContribution.Campaign.Id = campaign.Key.ToString();
        platformsContribution.Campaign.Reference = publishedCampaign?.Reference;

        return platformsContribution;
    }
}
