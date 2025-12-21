using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Currency = N3O.Umbraco.Financial.Currency;
using FundDimensionValuesReq = N3O.Umbraco.Cloud.Platforms.Clients.FundDimensionValuesReq;
using PlatformsCurrency = N3O.Umbraco.Cloud.Platforms.Clients.Currency;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class ElementDonationFormsStateReqMapping {
    private CartItemReq GetCartItemReq(CampaignContent campaign,
                                       OfferingContent offering,
                                       IFundDimensionValues fundDimensionValues,
                                       IReadOnlyDictionary<string, string> tags) {
        var currency = _lookups.GetAll<Currency>().Single(x => x.IsBaseCurrency).Code.ToEnum<PlatformsCurrency>();

        var publishedCampaign = _cdnClient.DownloadPublishedContentAsync<PublishedCampaign>(PublishedFileKinds.Campaign,
                                                                                            $"{campaign.Key}.json",
                                                                                            JsonSerializers.Simple)
                                          .GetAwaiter().GetResult();

        var referenceType = new References.ReferenceType(nameof(ReferenceType.FC), 0);
        var campaignReference = referenceType.ToReference(publishedCampaign.Reference);
        
        var cartItem = new CartItemReq();
        cartItem.Id = Guid.NewGuid().ToString();
        cartItem.Type = CartItemType.NewDonation;
        cartItem.Currency = currency;
        
        cartItem.PlatformsContribution = new PlatformsContributionInfoReq();
        cartItem.Id = Guid.NewGuid().ToString();
        cartItem.PlatformsContribution.Campaign = new CampaignInfoReq();
        cartItem.PlatformsContribution.Campaign.Id = campaign.Key.ToString();
        cartItem.PlatformsContribution.Campaign.Reference = new ReferenceReq();
        cartItem.PlatformsContribution.Campaign.Reference.Type = ReferenceType.FC;
        cartItem.PlatformsContribution.Campaign.Reference.Number = campaignReference.Number;
        
        cartItem.PlatformsContribution.Offering = new OfferingInfoReq();
        cartItem.PlatformsContribution.Offering.Id = offering.Key.ToString();
        
        cartItem.NewDonation = new NewDonationReq();
        
        cartItem.NewDonation.Allocation = new AllocationIntentReq();
        cartItem.NewDonation.Allocation.Type = offering.Type.ToEnum<AllocationType>();
        cartItem.NewDonation.Allocation.PlatformsContribution = cartItem.PlatformsContribution;
        
        cartItem.NewDonation.Allocation.FundDimensions = new FundDimensionValuesReq();
        cartItem.NewDonation.Allocation.FundDimensions.Dimension1 = fundDimensionValues.Dimension1?.Name;
        cartItem.NewDonation.Allocation.FundDimensions.Dimension2 = fundDimensionValues.Dimension2?.Name;
        cartItem.NewDonation.Allocation.FundDimensions.Dimension3 = fundDimensionValues.Dimension3?.Name;
        cartItem.NewDonation.Allocation.FundDimensions.Dimension4 = fundDimensionValues.Dimension4?.Name;
        
        
        cartItem.NewDonation.Allocation.Value = new MoneyReq(); // TODO remove
        cartItem.NewDonation.Allocation.Value.Amount = 0d;
        cartItem.NewDonation.Allocation.Value.Currency = currency;

        if (offering.Type == OfferingTypes.Fund) {
            cartItem.NewDonation.Allocation.Fund = new FundIntentReq();
            cartItem.NewDonation.Allocation.Fund.DonationItem = offering.Fund.DonationItem.Name;
        } else if (offering.Type == OfferingTypes.Feedback) {
            cartItem.NewDonation.Allocation.Feedback = new FeedbackIntentReq();
            cartItem.NewDonation.Allocation.Feedback.Scheme = offering.Feedback.Scheme.Name;
        } else if (offering.Type == OfferingTypes.Sponsorship) {
            cartItem.NewDonation.Allocation.Sponsorship = new SponsorshipIntentReq();
            cartItem.NewDonation.Allocation.Sponsorship.Scheme = offering.Sponsorship.Scheme.Name;
        }

        if (tags.HasAny()) {
            cartItem.Tags = tags.ToTagCollectionReq();
        }

        return cartItem;
    }
}