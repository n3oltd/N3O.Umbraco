using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Donations.Models;
using N3O.Umbraco.Giving.Pricing.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class FundDonationOptionValidator : ContentValidator {
        private static readonly IEnumerable<string> Aliases = new[] {
            AliasHelper.ForContentType<FundDonationOption>(),
        };
    
        public FundDonationOptionValidator(IContentHelper contentHelper) : base(contentHelper) { }
    
        public override bool IsValidator(IContent content) {
            return Aliases.Contains(content.ContentType.Alias);
        }
    
        public override void Validate(IContent content) {
            var donationItem = GetDonationItem(content);

            if (donationItem != null) {
                SinglePriceHandlesValid(content, donationItem);
                RegularPriceHandlesValid(content, donationItem);
                FundDimensionAllowed(content, donationItem, x => x.Dimension1Options, x => x.Dimension1);
                FundDimensionAllowed(content, donationItem, x => x.Dimension2Options, x => x.Dimension2);
                FundDimensionAllowed(content, donationItem, x => x.Dimension3Options, x => x.Dimension3);
                FundDimensionAllowed(content, donationItem, x => x.Dimension4Options, x => x.Dimension4);
            }

            EnsureSingleAndRegularNotBothHidden(content);

            EnsureShowQuantityIsAllowed(content, donationItem);
        }

        private DonationItem GetDonationItem(IContent content) {
            return ContentHelper.GetPickerValue<FundDonationOption, DonationItem>(content, x => x.DonationItem);
        }

        private void SinglePriceHandlesValid(IContent content, DonationItem donationItem) {
            var priceHandles = ContentHelper.GetMultiNestedContentValue<FundDonationOption, PriceHandle>(content,
                                                                                                         x => x.SinglePriceHandles);

            if (priceHandles.HasValue()) {
                if (donationItem.HasPrice()) {
                    ErrorResult<FundDonationOption, IEnumerable<PriceHandle>>(content, x => x.SinglePriceHandles, $"{donationItem.Name} has a price so does not allow price handles");
                }

                if (!donationItem.AllowSingleDonations) {
                    ErrorResult<FundDonationOption, IEnumerable<PriceHandle>>(content, x => x.SinglePriceHandles, $"{donationItem.Name} does not allow single donations so single donation price handles are not permitted");
                }
            }
        }

        private void RegularPriceHandlesValid(IContent content, DonationItem donationItem) {
            var priceHandles = ContentHelper.GetMultiNestedContentValue<FundDonationOption, PriceHandle>(content,
                                                                                                         x => x.RegularPriceHandles);

            if (priceHandles.HasValue()) {
                if (donationItem.HasPrice()) {
                    ErrorResult<FundDonationOption, IEnumerable<PriceHandle>>(content, x => x.RegularPriceHandles, $"{donationItem.Name} has a price so does not allow price handles");
                }

                if (!donationItem.AllowRegularDonations) {
                    ErrorResult<FundDonationOption, IEnumerable<PriceHandle>>(content, x => x.SinglePriceHandles, $"{donationItem.Name} does not allow monthly donations so monthly donation price handles are not permitted");
                }
            }
        }

        private void FundDimensionAllowed(IContent content,
                                          DonationItem donationItem,
                                          Func<DonationItem, IEnumerable<FundDimensionOption>> getAllowedOptions,
                                          Expression<Func<FundDonationOption, FundDimensionOption>> getValue) {
            var allowedValues = getAllowedOptions(donationItem).ToList();
            var value = ContentHelper.GetPickerValue(content, getValue);

            if (value != null && allowedValues.All(x => x.Id != value.Id)) {
                ErrorResult(content,
                            getValue,
                            $"{value.Name} is not permitted for item {donationItem.Name}");
            }
        }

        private void EnsureSingleAndRegularNotBothHidden(IContent content) {
            if (content.GetValue<FundDonationOption, bool>(x => x.HideSingle) &&
                content.GetValue<FundDonationOption, bool>(x => x.HideRegular)) {
                ErrorResult("Cannot hide both single and regular options");
            }
        }

        private void EnsureShowQuantityIsAllowed(IContent content, DonationItem donationItem) {
            var showQuantity = content.GetValue<FundDonationOption, bool>(x => x.ShowQuantity);
    
            if (showQuantity && !donationItem.HasPrice()) {
                ErrorResult<FundDonationOption, bool>(content, x => x.ShowQuantity, "Show quantity can only be enabled for donation items that have a price");
            }

            if (showQuantity &&
                (donationItem.AllowRegularDonations == false || donationItem.AllowSingleDonations == false)) {
                ErrorResult<FundDonationOption, bool>(content, x => x.ShowQuantity, "Show quantity can only be enabled for donation items that allow single or regular donations but not both");
            }
        }
    }
}
