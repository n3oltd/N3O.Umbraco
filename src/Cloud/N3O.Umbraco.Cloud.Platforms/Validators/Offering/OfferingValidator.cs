using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public abstract class OfferingValidator : ContentValidator {
    private static readonly string AllowCrowdfundingAlias = AliasHelper<OfferingContent>.PropertyAlias(x => x.AllowCrowdfunding);

    private readonly ILookups _lookups;
    private readonly IFundStructureAccessor _fundStructureAccessor;

    protected OfferingValidator(IContentHelper contentHelper,
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
        var fundDimensionOptions = GetFundDimensionOptions(content);

        if (fundDimensionOptions != null) {
            ValidateFixedDimension(content, fundDimensionOptions);
        }
    }

    protected abstract IFundDimensionOptions GetFundDimensionOptions(ContentProperties content);
    protected abstract string ContentTypeAlias { get; }

    private void ValidateFixedDimension(ContentProperties content, IFundDimensionOptions fundDimensionOptions) {
        if (content.GetPropertyValueByAlias<int?>(AllowCrowdfundingAlias) == 1) {
            var fundStructure = _fundStructureAccessor.GetFundStructure();
            
            HasFixedDimension(content, fundDimensionOptions.Dimension1, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension1), fundStructure.Dimension1.IsActive);
            HasFixedDimension(content, fundDimensionOptions.Dimension2, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension2), fundStructure.Dimension2.IsActive);
            HasFixedDimension(content, fundDimensionOptions.Dimension3, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension3), fundStructure.Dimension3.IsActive);
            HasFixedDimension(content, fundDimensionOptions.Dimension4, AliasHelper<DonationFormStateContent>.PropertyAlias(x => x.Dimension4), fundStructure.Dimension4.IsActive);
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
