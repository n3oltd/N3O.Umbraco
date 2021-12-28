using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Donations.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class FundDonationOption : DonationOption {
        public DonationItem DonationItem => GetAs<FundDonationOption, DonationItem>(x => x.DonationItem);
        public FundDimension1Option Dimension1 => GetAs<FundDonationOption, FundDimension1Option>(x => x.Dimension1);
        public FundDimension2Option Dimension2 => GetAs<FundDonationOption, FundDimension2Option>(x => x.Dimension2);
        public FundDimension3Option Dimension3 => GetAs<FundDonationOption, FundDimension3Option>(x => x.Dimension3);
        public FundDimension4Option Dimension4 => GetAs<FundDonationOption, FundDimension4Option>(x => x.Dimension4);
        public bool ShowQuantity => GetValue<FundDonationOption, bool>(x => x.ShowQuantity);

        public bool HideSingle => GetValue<FundDonationOption, bool>(x => x.HideSingle);
        public IEnumerable<PriceHandle> SinglePriceHandles => GetCollectionAs<FundDonationOption, PriceHandle>(x => x.SinglePriceHandles);

        public bool HideRegular => GetValue<FundDonationOption, bool>(x => x.HideRegular);
        public IEnumerable<PriceHandle> RegularPriceHandles => GetCollectionAs<FundDonationOption, PriceHandle>(x => x.RegularPriceHandles);

        public IEnumerable<PriceHandle> GetPriceHandles(DonationType donationType) {
            if (donationType == DonationTypes.Single) {
                return SinglePriceHandles;
            } else if (donationType == DonationTypes.Regular) {
                return RegularPriceHandles;
            } else {
                throw UnrecognisedValueException.For(donationType);
            }
        }
    }
}
