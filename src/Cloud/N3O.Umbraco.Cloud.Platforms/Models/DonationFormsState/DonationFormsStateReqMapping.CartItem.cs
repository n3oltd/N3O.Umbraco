using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using FundDimensionValuesReq = N3O.Umbraco.Cloud.Platforms.Clients.FundDimensionValuesReq;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public partial class ElementDonationFormsStateReqMapping {
    private CartItemReq GetCartItemReq(CampaignContent campaign,
                                       OfferingContent offering,
                                       IFundDimensionValues fundDimensionValues,
                                       IReadOnlyDictionary<string, string> tags) {
        var cartItem = new CartItemReq();
        cartItem.Type = CartItemType.NewDonation;
        
        cartItem.PlatformsContribution = new PlatformsContributionInfoReq();
        cartItem.PlatformsContribution.Campaign = new CampaignInfoReq();
        cartItem.PlatformsContribution.Campaign.Id = campaign.Key.ToString();
        
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