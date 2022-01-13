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
        private static readonly string DonationItemAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.DonationItem);
        private static readonly string Dimension1OptionsAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.Dimension1);
        private static readonly string Dimension2OptionsAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.Dimension2);
        private static readonly string Dimension3OptionsAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.Dimension3);
        private static readonly string Dimension4OptionsAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.Dimension4);
        private static readonly string HideRegularAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.HideRegular);
        private static readonly string HideSingleAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.HideSingle);
        private static readonly string RegularPriceHandlesAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.RegularPriceHandles);
        private static readonly string ShowQuantityAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.ShowQuantity);
        private static readonly string SinglePriceHandlesAlias = AliasHelper<FundDonationOption>.PropertyAlias(x => x.SinglePriceHandles);
        
        private static readonly IEnumerable<string> Aliases = new[] {
            AliasHelper<DonationFormFund>.ContentTypeAlias(),
        };
    
        public FundDonationOptionValidator(IContentHelper contentHelper) : base(contentHelper) { }
    
        public override bool IsValidator(ContentProperties content) {
            return Aliases.Contains(content.ContentTypeAlias, true);
        }
    
        public override void Validate(ContentProperties content) {
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

        private void SinglePriceHandlesValid(ContentProperties content, DonationItem donationItem) {
            var property = content.NestedContentProperties.SingleOrDefault(x => x.Alias.EqualsInvariant(SinglePriceHandlesAlias));
            var priceHandles = property.IfNotNull(x => ContentHelper.GetNestedContents(x))
                                       .OrEmpty()
                                       .As<PriceHandle>()
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

        private void RegularPriceHandlesValid(ContentProperties content, DonationItem donationItem) {
            var property = content.NestedContentProperties.SingleOrDefault(x => x.Alias.EqualsInvariant(RegularPriceHandlesAlias));
            var priceHandles = property.IfNotNull(x => ContentHelper.GetNestedContents(x))
                                       .OrEmpty()
                                       .As<PriceHandle>()
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

        private void FundDimensionAllowed(ContentProperties content,
                                          DonationItem donationItem,
                                          IEnumerable<FundDimensionOption> allowedOptions,
                                          string propertyAlias) {
            var property = content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(propertyAlias));
            var value = property.IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x).As<FundDimensionOption>());

            if (value != null && !allowedOptions.Contains(value)) {
                ErrorResult(property, $"{value.Name} is not permitted for item {donationItem.Name}");
            }
        }

        private void EnsureSingleAndRegularNotBothHidden(ContentProperties content) {
            var hideSingle = (bool?) content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(HideSingleAlias))?.Value;
            var hideRegular = (bool?) content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(HideRegularAlias))?.Value;
            
            if (hideSingle == true && hideRegular == true) {
                ErrorResult("Cannot hide both single and regular options");
            }
        }

        private void EnsureShowQuantityIsAllowed(ContentProperties content, DonationItem donationItem) {
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
