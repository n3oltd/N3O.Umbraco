using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Donations.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class FundDonationOptionContent : UmbracoContent<FundDonationOptionContent> {
        public DonationItem DonationItem => GetAs(x => x.DonationItem);
        public FundDimension1Option Dimension1 => GetAs(x => x.Dimension1);
        public FundDimension2Option Dimension2 => GetAs(x => x.Dimension2);
        public FundDimension3Option Dimension3 => GetAs(x => x.Dimension3);
        public FundDimension4Option Dimension4 => GetAs(x => x.Dimension4);
        public bool ShowQuantity => GetValue(x => x.ShowQuantity);

        public bool HideSingle => GetValue(x => x.HideSingle);
        public IEnumerable<PriceHandleElement> SinglePriceHandles => GetNestedAs(x => x.SinglePriceHandles);

        public bool HideRegular => GetValue(x => x.HideRegular);
        public IEnumerable<PriceHandleElement> RegularPriceHandles => GetNestedAs(x => x.RegularPriceHandles);

        public IEnumerable<PriceHandleElement> GetPriceHandles(DonationType donationType) {
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
