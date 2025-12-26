using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Clients.Platforms;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Currency = N3O.Umbraco.Financial.Currency;
using FundDimensionValuesReq = N3O.Umbraco.Cloud.Clients.Platforms.FundDimensionValuesReq;
using PlatformsCurrency = N3O.Umbraco.Cloud.Clients.Platforms.Currency;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class ElementDonationFormsStateReqMapping {
    private CartItemReq GetCartItemReq(CampaignContent campaign,
                                       OfferingContent offering,
                                       IFundDimensionValues fundDimensionValues,
                                       IReadOnlyDictionary<string, string> tags) {
        var currency = _lookups.GetAll<Currency>().Single(x => x.IsBaseCurrency).Code.ToEnum<PlatformsCurrency>();
        
        var cartItem = new CartItemReq();
        cartItem.Id = Guid.NewGuid().ToString();
        cartItem.Type = CartItemType.NewDonation;
        cartItem.Currency = currency;
        
        cartItem.NewDonation = new NewDonationReq();

        var allocation = GetAllocationIntent(campaign, offering, fundDimensionValues, currency);

        if (offering.SuggestedGiftType == GiftTypes.OneTime) {
            cartItem.NewDonation = new NewDonationReq();
            cartItem.NewDonation.Allocation = allocation;
        } else if (offering.SuggestedGiftType == GiftTypes.Recurring) {
            cartItem.NewRegularGiving = new NewRegularGivingWithOptionsReq();
            cartItem.NewRegularGiving.Allocation = allocation;
        } else {
            throw UnrecognisedValueException.For(offering.SuggestedGiftType);
        }

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
        
        allocationIntent.FundDimensions = new FundDimensionValuesReq();
        allocationIntent.FundDimensions.Dimension1 = fundDimensionValues.Dimension1?.Name;
        allocationIntent.FundDimensions.Dimension2 = fundDimensionValues.Dimension2?.Name;
        allocationIntent.FundDimensions.Dimension3 = fundDimensionValues.Dimension3?.Name;
        allocationIntent.FundDimensions.Dimension4 = fundDimensionValues.Dimension4?.Name;
        
        allocationIntent.Value = new MoneyReq();
        allocationIntent.Value.Amount = 0d;
        allocationIntent.Value.Currency = currency;

        if (offering.Type == OfferingTypes.Fund) {
            allocationIntent.Fund = new FundIntentReq();
            allocationIntent.Fund.DonationItem = offering.Fund.DonationItem.Name;
        } else if (offering.Type == OfferingTypes.Feedback) {
            allocationIntent.Feedback = new FeedbackIntentReq();
            allocationIntent.Feedback.Scheme = offering.Feedback.Scheme.Name;
        } else if (offering.Type == OfferingTypes.Sponsorship) {
            allocationIntent.Sponsorship = new SponsorshipIntentReq();
            allocationIntent.Sponsorship.Scheme = offering.Sponsorship.Scheme.Name;
        }

        return allocationIntent;
    }

    private PlatformsContributionInfoReq GetPlatformsContributionInfoReq(CampaignContent campaign,
                                                                         OfferingContent offering) {
        var publishedCampaign = _cdnClient.DownloadPublishedContentAsync<PublishedCampaign>(PublishedFileKinds.Campaign,
                                                                                            $"{campaign.Key}.json",
                                                                                            JsonSerializers.Simple)
                                          .GetAwaiter().GetResult();
        
       var platformsContribution = new PlatformsContributionInfoReq();
       platformsContribution.ContributionId = Guid.NewGuid().ToString();
       platformsContribution.Campaign = new CampaignInfoReq();
       platformsContribution.Campaign.Id = campaign.Key.ToString();
       platformsContribution.Campaign.Reference = publishedCampaign.Reference;
        
       platformsContribution.Offering = new OfferingInfoReq();
       platformsContribution.Offering.Id = offering.Key.ToString();

       return platformsContribution;
    }
}