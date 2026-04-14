using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Exceptions;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class IHoldDonationFormStateExtensions {
    public static DonationFormStateReq ToDonationFormStateReq(this IHoldDonationFormState src) {
        var formState = new DonationFormStateReq();
        formState.CartItem = new CartItemReq();

        if (src.GiftType == GiftTypes.OneTime) {
            formState.CartItem.NewDonation = new NewDonationReq();
            formState.CartItem.NewDonation.Allocation = GetAllocationIntentReq(src);
        } else if (src.GiftType == GiftTypes.OneTime) {
            formState.CartItem.NewRegularGiving = new NewRegularGivingWithOptionsReq();
            formState.CartItem.NewRegularGiving.Allocation = GetAllocationIntentReq(src);
            formState.CartItem.NewRegularGiving.Options = new ConnectRegularGivingOptionsReq();
            formState.CartItem.NewRegularGiving.Options.Frequency = RegularGivingFrequency.Monthly;
        } else {
            throw UnrecognisedValueException.For(src.GiftType);
        }

        return formState;
    }
    
    private static AllocationIntentReq GetAllocationIntentReq(IHoldDonationFormState src) {
        var allocation = new AllocationIntentReq();
        allocation.Type = AllocationType.Fund;
        
        allocation.Fund = new FundIntentReq();
        allocation.Fund.DonationItem = src.DonationItem?.Name;
        
        allocation.FundDimensions = new FundDimensionValuesReq();
        allocation.FundDimensions.Dimension1 = src.Dimension1?.Name;
        allocation.FundDimensions.Dimension2 = src.Dimension2?.Name;
        allocation.FundDimensions.Dimension3 = src.Dimension3?.Name;
        allocation.FundDimensions.Dimension4 = src.Dimension4?.Name;

        return allocation;
    }
}