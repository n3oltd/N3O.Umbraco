using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class IHoldDonationFormStateExtensions {
    public static DonationFormStateReq ToDonationFormStateReq(this IHoldDonationFormState src) {
        var formState = new DonationFormStateReq();
        formState.CartItem = new CartItemReq();
        formState.CartItem.NewDonation = new NewDonationReq();
        formState.CartItem.NewDonation.Allocation = new AllocationIntentReq();
        formState.CartItem.NewDonation.Allocation.Type = AllocationType.Fund;
        
        formState.CartItem.NewDonation.Allocation.Fund = new FundIntentReq();
        formState.CartItem.NewDonation.Allocation.Fund.DonationItem = src.DonationItem?.Name;
        
        formState.CartItem.NewDonation.Allocation.FundDimensions = new FundDimensionValuesReq();
        formState.CartItem.NewDonation.Allocation.FundDimensions.Dimension1 = src.Dimension1?.Name;
        formState.CartItem.NewDonation.Allocation.FundDimensions.Dimension2 = src.Dimension2?.Name;
        formState.CartItem.NewDonation.Allocation.FundDimensions.Dimension3 = src.Dimension3?.Name;
        formState.CartItem.NewDonation.Allocation.FundDimensions.Dimension4 = src.Dimension4?.Name;

        return formState;
    }
}