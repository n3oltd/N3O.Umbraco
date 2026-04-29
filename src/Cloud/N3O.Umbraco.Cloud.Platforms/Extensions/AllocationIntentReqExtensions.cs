using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using AllocationType = N3O.Umbraco.Cloud.Platforms.Clients.AllocationType;
using FundDimensionValuesReq = N3O.Umbraco.Cloud.Platforms.Clients.FundDimensionValuesReq;
using PlatformsCurrency = N3O.Umbraco.Cloud.Platforms.Clients.Currency;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class AllocationIntentReqExtensions {
    public static AllocationIntentReq ToAllocationIntentReq(this DonationFormStateContent formState,
                                                            IFundDimensionValues fundDimensionValues,
                                                            PlatformsCurrency? currency) {
        var allocationIntent = new AllocationIntentReq();
        allocationIntent.Type = formState.Type.ToEnum<AllocationType>();

        allocationIntent.FundDimensions = new FundDimensionValuesReq();
        allocationIntent.FundDimensions.Dimension1 = fundDimensionValues.Dimension1?.Name;
        allocationIntent.FundDimensions.Dimension2 = fundDimensionValues.Dimension2?.Name;
        allocationIntent.FundDimensions.Dimension3 = fundDimensionValues.Dimension3?.Name;
        allocationIntent.FundDimensions.Dimension4 = fundDimensionValues.Dimension4?.Name;

        allocationIntent.Value = new MoneyReq();
        allocationIntent.Value.Amount = 0d;
        allocationIntent.Value.Currency = currency;

        if (formState.Type == AllocationTypes.Fund) {
            allocationIntent.Fund = new FundIntentReq();
            allocationIntent.Fund.DonationItem = formState.Fund.DonationItem.Name;
        } else if (formState.Type == AllocationTypes.Feedback) {
            allocationIntent.Feedback = new FeedbackIntentReq();
            allocationIntent.Feedback.New = new NewFeedbackIntentReq();
            allocationIntent.Feedback.New.Scheme = formState.Feedback.Scheme.Name;
        } else if (formState.Type == AllocationTypes.Qurbani) {
            allocationIntent.Qurbani = new QurbaniIntentReq();
            allocationIntent.Qurbani.New = new NewQurbaniIntentReq();
            allocationIntent.Qurbani.New.QurbaniItem = formState.Qurbani.QurbaniItem.Name;
        } else if (formState.Type == AllocationTypes.Sponsorship) {
            allocationIntent.Sponsorship = new SponsorshipIntentReq();
            allocationIntent.Sponsorship.New = new NewSponsorshipIntentReq();
            allocationIntent.Sponsorship.New.Scheme = formState.Sponsorship.Scheme.Name;
        }

        return allocationIntent;
    }
}
