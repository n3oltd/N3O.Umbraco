using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Donations.Models;
using N3O.Umbraco.Giving.Pricing.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class FundDonationOptionValidator : ContentValidator {
        private static readonly string DonationItemAlias = AliasHelper.ForProperty<FundDonationOption, DonationItem>(x => x.DonationItem);
        private static readonly string Dimension1OptionsAlias = AliasHelper.ForProperty<FundDonationOption, FundDimension1Option>(x => x.Dimension1);
        private static readonly string Dimension2OptionsAlias = AliasHelper.ForProperty<FundDonationOption, FundDimension2Option>(x => x.Dimension2);
        private static readonly string Dimension3OptionsAlias = AliasHelper.ForProperty<FundDonationOption, FundDimension3Option>(x => x.Dimension3);
        private static readonly string Dimension4OptionsAlias = AliasHelper.ForProperty<FundDonationOption, FundDimension4Option>(x => x.Dimension4);
        private static readonly string HideRegularAlias = AliasHelper.ForProperty<FundDonationOption, bool>(x => x.HideRegular);
        private static readonly string HideSingleAlias = AliasHelper.ForProperty<FundDonationOption, bool>(x => x.HideSingle);
        private static readonly string RegularPriceHandlesAlias = AliasHelper.ForProperty<FundDonationOption, IEnumerable<PriceHandle>>(x => x.RegularPriceHandles);
        private static readonly string ShowQuantityAlias = AliasHelper.ForProperty<FundDonationOption, bool>(x => x.ShowQuantity);
        private static readonly string SinglePriceHandlesAlias = AliasHelper.ForProperty<FundDonationOption, IEnumerable<PriceHandle>>(x => x.SinglePriceHandles);
        
        private static readonly IEnumerable<string> Aliases = new[] {
            AliasHelper.ForContentType<FundDonationOption>(),
        };
    
        public FundDonationOptionValidator(IContentHelper contentHelper) : base(contentHelper) { }
    
        public override bool IsValidator(ContentNode content) {
            return Aliases.Contains(content.ContentTypeAlias);
        }
    
        public override void Validate(ContentNode content) {
            var donationItem = content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(DonationItemAlias))
                                      .IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x)
                                                                   .As<DonationItem>());

            if (donationItem != null) {
                SinglePriceHandlesValid(content, donationItem);
                RegularPriceHandlesValid(content, donationItem);
                FundDimensionAllowed(content, donationItem, donationItem.Dimension1Options, Dimension1OptionsAlias);
                FundDimensionAllowed(content, donationItem, donationItem.Dimension2Options, Dimension2OptionsAlias);
                FundDimensionAllowed(content, donationItem, donationItem.Dimension3Options, Dimension3OptionsAlias);
                FundDimensionAllowed(content, donationItem, donationItem.Dimension4Options, Dimension4OptionsAlias);
            }

            EnsureSingleAndRegularNotBothHidden(content);

            EnsureShowQuantityIsAllowed(content, donationItem);
        }

        private void SinglePriceHandlesValid(ContentNode content, DonationItem donationItem) {
            var property = content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(SinglePriceHandlesAlias));
            var priceHandles = (property?.IfNotNull(x => ContentHelper.GetNestedContents(x))).OrEmpty().As<PriceHandle>()
                                                                                             .ToList();

            if (priceHandles.HasAny()) {
                if (donationItem.HasPrice()) {
                    ErrorResult(property, $"{donationItem.Name} has a price so does not allow price handles");
                }

                if (!donationItem.AllowSingleDonations) {
                    ErrorResult(property, $"{donationItem.Name} does not allow single donations so single donation price handles are not permitted");
                }
            }
        }

        private void RegularPriceHandlesValid(ContentNode content, DonationItem donationItem) {
            var property = content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(RegularPriceHandlesAlias));
            var priceHandles = (property?.IfNotNull(x => ContentHelper.GetNestedContents(x))).OrEmpty().As<PriceHandle>()
                                                                                             .ToList();

            if (priceHandles.HasAny()) {
                if (donationItem.HasPrice()) {
                    ErrorResult(property, $"{donationItem.Name} has a price so does not allow price handles");
                }

                if (!donationItem.AllowRegularDonations) {
                    ErrorResult(property, $"{donationItem.Name} does not allow regular donations so regular donation price handles are not permitted");
                }
            }
        }

        private void FundDimensionAllowed(ContentNode content,
                                          DonationItem donationItem,
                                          IEnumerable<FundDimensionOption> allowedOptions,
                                          string propertyAlias) {
            var property = content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(propertyAlias));
            var value = property.IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x).As<FundDimensionOption>());

            if (value != null && !allowedOptions.Contains(value)) {
                ErrorResult(property, $"{value.Name} is not permitted for item {donationItem.Name}");
            }
        }

        private void EnsureSingleAndRegularNotBothHidden(ContentNode content) {
            var hideSingle = (bool?) content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(HideSingleAlias))?.Value;
            var hideRegular = (bool?) content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(HideRegularAlias))?.Value;
            
            if (hideSingle == true && hideRegular == true) {
                ErrorResult("Cannot hide both single and regular options");
            }
        }

        private void EnsureShowQuantityIsAllowed(ContentNode content, DonationItem donationItem) {
            var property = content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(ShowQuantityAlias));
            var showQuantity = (bool?) property?.Value;
    
            if (showQuantity == true && !donationItem.HasPrice()) {
                ErrorResult(property, "Show quantity can only be enabled for donation items that have a price");
            }

            if (showQuantity == true &&
                (donationItem.AllowRegularDonations == false || donationItem.AllowSingleDonations == false)) {
                ErrorResult(property, "Show quantity can only be enabled for donation items that allow single or regular donations but not both");
            }
        }
    }
}
