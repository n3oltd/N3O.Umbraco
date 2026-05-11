using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public abstract class CrossSellValidator : ContentValidator {
    private static readonly string AmountAlias = AliasHelper<CrossSellContent>.PropertyAlias(x => x.Amount);
    
    private readonly ILookups _lookups;
    private readonly IFundStructureAccessor _fundStructureAccessor;

    protected CrossSellValidator(IContentHelper contentHelper,
                                 ILookups lookups,
                                 IFundStructureAccessor fundStructureAccessor)
        : base(contentHelper) {
        _lookups = lookups;
        _fundStructureAccessor = fundStructureAccessor;
    }

    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias == ContentTypeAlias;
    }

    public override void Validate(ContentProperties content) {
       ValidateFundDimensions(content);
       ValidateAmount(content);
    }

    protected abstract IFundDimensionOptions GetFundDimensionOptions(ContentProperties content);
    protected abstract bool HasLockedPrice(ContentProperties content);
    protected abstract string ContentTypeAlias { get; }

    private void ValidateFundDimensions(ContentProperties content) {
        var fundDimensionOptions = GetFundDimensionOptions(content);

        if (fundDimensionOptions != null) {
            ValidateFixedDimension(content, fundDimensionOptions);
        }
    }
    
    private void ValidateFixedDimension(ContentProperties content, IFundDimensionOptions fundDimensionOptions) {
        var fundStructure = _fundStructureAccessor.GetFundStructure();
            
        HasFixedDimension(content, fundDimensionOptions.Dimension1, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension1), fundStructure.Dimension1.IsActive);
        HasFixedDimension(content, fundDimensionOptions.Dimension2, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension2), fundStructure.Dimension2.IsActive);
        HasFixedDimension(content, fundDimensionOptions.Dimension3, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension3), fundStructure.Dimension3.IsActive);
        HasFixedDimension(content, fundDimensionOptions.Dimension4, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension4), fundStructure.Dimension4.IsActive);
    }
    
    private void ValidateAmount(ContentProperties content) {
        var amount = content.GetPropertyValueByAlias<decimal?>(AmountAlias);
        
        if (ContentTypeAlias == PlatformsConstants.CrossSells.Sponsorship) {
            ErrorResult("Sponsorship item cannot have an amount");
        }

        if (HasLockedPrice(content) && amount.HasValue()) {
            ErrorResult("Amount cannot be specified");
        }
    }

    private void HasFixedDimension<T>(ContentProperties content,
                                      IEnumerable<T> allowedValues,
                                      string propertyAlias,
                                      bool isActive)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetLookupValue<T>(_lookups, x));

        if (isActive && value == null && !allowedValues.IsSingle()) {
            ErrorResult(property, $"{typeof(T).Name} should have a fixed value");
        }
    }
}
