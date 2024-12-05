using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Content;

public class FundDonationOptionValidator : DonationOptionValidator<FundDonationOptionContent> {
    private static readonly string DonationItemAlias = AliasHelper<FundDonationOptionContent>.PropertyAlias(x => x.DonationItem);
    private static readonly string DonationPriceHandlesAlias = AliasHelper<FundDonationOptionContent>.PropertyAlias(x => x.DonationPriceHandles);
    private static readonly string RegularGivingPriceHandlesAlias = AliasHelper<FundDonationOptionContent>.PropertyAlias(x => x.RegularGivingPriceHandles);
    
    public FundDonationOptionValidator(IContentHelper contentHelper, IContentLocator contentLocator)
        : base(contentHelper, contentLocator) { }

    protected override void ValidateOption(ContentProperties content) {
        var donationItem = GetDonationItem(content);

        if (!donationItem.HasValue()) {
            ErrorResult("Donation item is required");
        }
        
        if (donationItem != null) {
            ValidatePriceHandles(content, donationItem, GivingTypes.Donation, DonationPriceHandlesAlias);
            ValidatePriceHandles(content, donationItem, GivingTypes.RegularGiving, RegularGivingPriceHandlesAlias);
        }
    }

    protected override IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content) {
        return GetDonationItem(content);
    }

    protected override bool IsDuplicate(ContentProperties content, FundDonationOptionContent other) {
        return GetDonationItem(content) == other.DonationItem;
    }

    private DonationItem GetDonationItem(ContentProperties content) {
        var donationItem = content.GetPropertyByAlias(DonationItemAlias)
                                  .IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x)
                                                               .As<DonationItem>());

        return donationItem;
    }

    private void ValidatePriceHandles(ContentProperties content,
                                      DonationItem donationItem,
                                      GivingType givingType,
                                      string propertyAlias) {
        var property = content.NestedContentProperties.SingleOrDefault(x => x.Alias.EqualsInvariant(propertyAlias));
        var priceHandles = property.IfNotNull(x => ContentHelper.GetNestedContents(x))
                                   .OrEmpty()
                                   .As<PriceHandleElement>(null)
                                   .ToList();

        if (priceHandles.HasAny()) {
            if (donationItem.HasPricing()) {
                ErrorResult(property, $"{donationItem.Name} has pricing so does not allow price handles");
            }

            if (!donationItem.AllowedGivingTypes.OrEmpty().Contains(givingType)) {
                ErrorResult(property, $"{donationItem.Name} does not allow {givingType.Name.Quote()} so these price handles are not permitted");
            }
        }
    }
}
