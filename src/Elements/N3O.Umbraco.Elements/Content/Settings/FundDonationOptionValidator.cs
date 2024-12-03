using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Extensions;
using N3O.Umbraco.Elements.Lookups;
using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Content;

public class FundDonationOptionValidator : DonationOptionValidator<FundDonationOptionContent> {
    private readonly IContentLocator _contentLocator;
    
    private static readonly string DonationItemAlias = AliasHelper<FundDonationOptionContent>.PropertyAlias(x => x.DonationItem);
    private static readonly string DonationPriceHandlesAlias = AliasHelper<FundDonationOptionContent>.PropertyAlias(x => x.DonationPriceHandles);
    private static readonly string RegularGivingPriceHandlesAlias = AliasHelper<FundDonationOptionContent>.PropertyAlias(x => x.RegularGivingPriceHandles);

    public FundDonationOptionValidator(IContentHelper contentHelper, IContentLocator contentLocator) : base(contentHelper) {
        _contentLocator = contentLocator;
    }

    public override void Validate(ContentProperties content) {
        base.Validate(content);

        var donationItem = GetDonationItem(content);
        
        if (donationItem != null) {
            ValidatePriceHandles(content, donationItem, GivingTypes.Donation, DonationPriceHandlesAlias);
            ValidatePriceHandles(content, donationItem, GivingTypes.RegularGiving, RegularGivingPriceHandlesAlias);
        }
    }

    protected override IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content) {
        return GetDonationItem(content);
    }

    protected override void EnsureNotDuplicate(ContentProperties content) {
        var donationItem = GetDonationItem(content);

        var allOptions = _contentLocator.All<FundDonationOptionContent>().Where(x => x.Content().Key != content.Id);
        
        if (allOptions.Any(x => x.DonationItem == donationItem)) {
            ErrorResult("Cannot add duplicate fund items");
        }
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
                ErrorResult(property, $"{donationItem.Name} has a pricing so does not allow price handles");
            }

            if (!donationItem.AllowedGivingTypes.OrEmpty().Contains(givingType)) {
                ErrorResult(property, $"{donationItem.Name} does not allow {givingType.Name.Quote()} so these price handles are not permitted");
            }
        }
    }
}
