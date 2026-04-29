using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AllocationType = N3O.Umbraco.Cloud.Platforms.Clients.AllocationType;
using CampaignContent = N3O.Umbraco.Cloud.Platforms.Content.CampaignContent;
using CrossSellContent = N3O.Umbraco.Cloud.Platforms.Content.CrossSellContent;
using Currency = N3O.Umbraco.Financial.Currency;
using FundDimensionValuesReq = N3O.Umbraco.Cloud.Platforms.Clients.FundDimensionValuesReq;
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

        var allocation = GetAllocationIntent(campaign, offering, fundDimensionValues, currency);
        SetCartItemAllocation(cartItem, allocation, offering.SuggestedGiftType);

        if (tags.HasAny()) {
            cartItem.Tags = tags.ToTagCollectionReq();
        }

        return cartItem;
    }

    private AllocationIntentReq GetAllocationIntent(CampaignContent campaign,
                                                    OfferingContent offering,
                                                    IFundDimensionValues fundDimensionValues,
                                                    PlatformsCurrency? currency) {
        var allocationIntent = new AllocationIntentReq();
        allocationIntent.Type = offering.Type.ToEnum<AllocationType>();
        allocationIntent.PlatformsContribution = GetPlatformsContributionInfoReq(campaign, offering);

        SetFundDimensionsAndValue(allocationIntent, fundDimensionValues, currency);

        if (offering.Type == AllocationTypes.Fund) {
            allocationIntent.Fund = new FundIntentReq();
            allocationIntent.Fund.DonationItem = offering.Fund.DonationItem.Name;
        } else if (offering.Type == AllocationTypes.Feedback) {
            allocationIntent.Feedback = new FeedbackIntentReq();
            allocationIntent.Feedback.New = new NewFeedbackIntentReq();
            allocationIntent.Feedback.New.Scheme = offering.Feedback.Scheme.Name;
        } else if (offering.Type == AllocationTypes.Qurbani) {
            allocationIntent.Qurbani = new QurbaniIntentReq();
            allocationIntent.Qurbani.New = new NewQurbaniIntentReq();
            allocationIntent.Qurbani.New.QurbaniItem = offering.Qurbani.QurbaniItem.Name;
        } else if (offering.Type == AllocationTypes.Sponsorship) {
            allocationIntent.Sponsorship = new SponsorshipIntentReq();
            allocationIntent.Sponsorship.New = new NewSponsorshipIntentReq();
            allocationIntent.Sponsorship.New.Scheme = offering.Sponsorship.Scheme.Name;
        }

        return allocationIntent;
    }

    private PlatformsContributionInfoReq GetPlatformsContributionInfoReq(CampaignContent campaign,
                                                                         OfferingContent offering) {
        var platformsContribution = GetBasePlatformsContributionInfoReq(campaign);

        platformsContribution.Offering = new OfferingInfoReq();
        platformsContribution.Offering.Id = offering.Key.ToString();
        platformsContribution.Offering.Name = offering.Name.ToString();

        return platformsContribution;
    }

    private CartItemReq GetCartItemReq(CampaignContent campaign,
                                       CrossSellContent crossSell,
                                       IFundDimensionValues fundDimensionValues,
                                       IReadOnlyDictionary<string, string> tags) {
        var currency = _lookups.GetAll<Currency>().Single(x => x.IsBaseCurrency).Code.ToEnum<PlatformsCurrency>();

        var cartItem = new CartItemReq();
        cartItem.Id = Guid.NewGuid().ToString();
        cartItem.Currency = currency;

        var allocation = GetAllocationIntent(campaign, crossSell, fundDimensionValues, currency);
        SetCartItemAllocation(cartItem, allocation, crossSell.SuggestedGiftType);

        if (tags.HasAny()) {
            cartItem.Tags = tags.ToTagCollectionReq();
        }

        return cartItem;
    }

    private AllocationIntentReq GetAllocationIntent(CampaignContent campaign,
                                                    CrossSellContent crossSell,
                                                    IFundDimensionValues fundDimensionValues,
                                                    PlatformsCurrency? currency) {
        var allocationIntent = new AllocationIntentReq();
        allocationIntent.Type = crossSell.Type.ToEnum<AllocationType>();

        if (campaign.HasValue()) {
            allocationIntent.PlatformsContribution = GetBasePlatformsContributionInfoReq(campaign);
        }

        SetFundDimensionsAndValue(allocationIntent, fundDimensionValues, currency);

        if (crossSell.Type == AllocationTypes.Fund) {
            allocationIntent.Fund = new FundIntentReq();
            allocationIntent.Fund.DonationItem = crossSell.DonationItem.Name;
        } else if (crossSell.Type == AllocationTypes.Feedback) {
            allocationIntent.Feedback = new FeedbackIntentReq();
            allocationIntent.Feedback.New = new NewFeedbackIntentReq();
            allocationIntent.Feedback.New.Scheme = crossSell.FeedbackScheme.Name;
        } else if (crossSell.Type == AllocationTypes.Qurbani) {
            allocationIntent.Qurbani = new QurbaniIntentReq();
            allocationIntent.Qurbani.New = new NewQurbaniIntentReq();
            allocationIntent.Qurbani.New.QurbaniItem = crossSell.QurbaniItem.Name;
        } else if (crossSell.Type == AllocationTypes.Sponsorship) {
            allocationIntent.Sponsorship = new SponsorshipIntentReq();
            allocationIntent.Sponsorship.New = new NewSponsorshipIntentReq();
            allocationIntent.Sponsorship.New.Scheme = crossSell.SponsorshipScheme?.Name;
        }

        return allocationIntent;
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

    private void SetFundDimensionsAndValue(AllocationIntentReq allocationIntent,
                                           IFundDimensionValues fundDimensionValues,
                                           PlatformsCurrency? currency) {
        allocationIntent.FundDimensions = new FundDimensionValuesReq();
        allocationIntent.FundDimensions.Dimension1 = fundDimensionValues.Dimension1?.Name;
        allocationIntent.FundDimensions.Dimension2 = fundDimensionValues.Dimension2?.Name;
        allocationIntent.FundDimensions.Dimension3 = fundDimensionValues.Dimension3?.Name;
        allocationIntent.FundDimensions.Dimension4 = fundDimensionValues.Dimension4?.Name;

        allocationIntent.Value = new MoneyReq();
        allocationIntent.Value.Amount = 0d;
        allocationIntent.Value.Currency = currency;
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
