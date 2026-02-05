using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class DonationButtonElementValidator : DonationElementValidator<DonationButtonElementContent> {
    private static readonly string AmountAlias = AliasHelper<DonationButtonElementContent>.PropertyAlias(x => x.Amount);
    private static readonly string ActionAlias = AliasHelper<DonationButtonElementContent>.PropertyAlias(x => x.Action);
    
    private readonly IContentHelper _contentHelper;
    private readonly IContentLocator _contentLocator;
    private readonly ILookups _lookups;

    public DonationButtonElementValidator(IContentHelper contentHelper,
                                          IContentLocator contentLocator,
                                          ILookups lookups)
        : base(contentHelper, contentLocator, lookups) {
        _contentHelper = contentHelper;
        _contentLocator = contentLocator;
        _lookups = lookups;
    }

    protected override void ValidateProperties(ContentProperties content) {
        var offering = GetOfferingContent(content);
        
        var action = _contentHelper.GetDataListValue<DonationButtonAction>(content, ActionAlias);

        if (action.IsAnyOf(DonationButtonActions.AddToCart, DonationButtonActions.BeginCheckout)) {
            ValidateHasPricing(content, offering);
            ValidateHasFixedDimensions(content, offering);
        }
    }

    private void ValidateHasPricing(ContentProperties content, OfferingContent offering) {
        var amount = content.GetPropertyValueByAlias<decimal?>(AmountAlias);

        if (!offering.HasPricing && !amount.HasValue()) {
            ErrorResult("Amount must be specified");
        }
        
        if (offering.HasPricing && amount.HasValue()) {
            ErrorResult("Offering has a pricing, amount cannot be specified");
        }
    }
    
    private void ValidateHasFixedDimensions(ContentProperties content, OfferingContent offering) {
        var offeringFixedFundDimensionValue = offering.GetFixedFundDimensionValues();
        
        ValidateHasFixedDimension<FundDimension1Value>(content, offeringFixedFundDimensionValue.Dimension1?.Name, Dimension1Alias);
        ValidateHasFixedDimension<FundDimension2Value>(content, offeringFixedFundDimensionValue.Dimension2.Name, Dimension2Alias);
        ValidateHasFixedDimension<FundDimension3Value>(content, offeringFixedFundDimensionValue.Dimension3.Name, Dimension3Alias);
        ValidateHasFixedDimension<FundDimension4Value>(content, offeringFixedFundDimensionValue.Dimension4.Name, Dimension4Alias);
    }
    
    private void ValidateHasFixedDimension<T>(ContentProperties content,
                                              string fixedDimensionValue,
                                              string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetLookupValue<T>(_lookups, x));

        if (property.HasValue() && !fixedDimensionValue.HasValue() && !value.HasValue()) {
            ErrorResult(property, "Fund dimension value must be specified");
        }
    }
}