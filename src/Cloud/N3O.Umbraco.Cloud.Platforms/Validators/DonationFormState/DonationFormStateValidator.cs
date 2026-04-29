using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public abstract class DonationFormStateValidator<TDonationFormStateContent> : ContentValidator {
    private static readonly string CompositionAlias = AliasHelper<TDonationFormStateContent>.ContentTypeAlias();
    private static readonly string AllowCrowdfundingAlias = AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.AllowCrowdfunding);

    private readonly ILookups _lookups;
    private readonly IFundStructureAccessor _fundStructureAccessor;

    protected DonationFormStateValidator(IContentHelper contentHelper,
                                         ILookups lookups,
                                         IFundStructureAccessor fundStructureAccessor)
        : base(contentHelper) {
        _lookups = lookups;
        _fundStructureAccessor = fundStructureAccessor;
    }

    public override bool IsValidator(ContentProperties content) {
        return content.IsComposedOf(CompositionAlias);
    }

    public override void Validate(ContentProperties content) {
        var fundDimensionOptions = GetFundDimensionOptions(content);

        if (fundDimensionOptions != null) {
            ValidateDimensionAllowed(content, fundDimensionOptions);
            ValidateFixedDimension(content, fundDimensionOptions);
        }
    }

    protected abstract IFundDimensionOptions GetFundDimensionOptions(ContentProperties content);

    private void ValidateDimensionAllowed(ContentProperties content, IFundDimensionOptions fundDimensionOptions) {
        DimensionAllowed(content, fundDimensionOptions.Dimension1, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension1));
        DimensionAllowed(content, fundDimensionOptions.Dimension2, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension2));
        DimensionAllowed(content, fundDimensionOptions.Dimension3, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension3));
        DimensionAllowed(content, fundDimensionOptions.Dimension4, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension4));
    }

    private void ValidateFixedDimension(ContentProperties content, IFundDimensionOptions fundDimensionOptions) {
        var fundStructure = _fundStructureAccessor.GetFundStructure();

        if (content.GetPropertyValueByAlias<int?>(AllowCrowdfundingAlias) == 1) {
            HasFixedDimension(content, fundDimensionOptions.Dimension1, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension1), fundStructure.Dimension1.IsActive);
            HasFixedDimension(content, fundDimensionOptions.Dimension2, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension2), fundStructure.Dimension2.IsActive);
            HasFixedDimension(content, fundDimensionOptions.Dimension3, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension3), fundStructure.Dimension3.IsActive);
            HasFixedDimension(content, fundDimensionOptions.Dimension4, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension4), fundStructure.Dimension4.IsActive);
        }
    }

    private void DimensionAllowed<T>(ContentProperties content,
                                     IEnumerable<T> allowedValues,
                                     string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetLookupValue<T>(_lookups, x));

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
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
