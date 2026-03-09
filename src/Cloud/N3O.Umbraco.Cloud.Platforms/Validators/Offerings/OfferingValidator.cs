using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public abstract class OfferingValidator<TOfferingContent> : ContentValidator {
    private static readonly string AllowCrowdfunding = AliasHelper<OfferingContent>.PropertyAlias(x => x.AllowCrowdfunding);
    
    private static readonly IEnumerable<string> Aliases = new[] {
        AliasHelper<TOfferingContent>.ContentTypeAlias(),
    };
    
    private readonly ILookups _lookups;
    private readonly IFundStructureAccessor _fundStructureAccessor;

    public OfferingValidator(IContentHelper contentHelper,
                             ILookups lookups,
                             IFundStructureAccessor fundStructureAccessor)
        : base(contentHelper) {
        _lookups = lookups;
        _fundStructureAccessor = fundStructureAccessor;
    }
    
    public override bool IsValidator(ContentProperties content) {
        return Aliases.Contains(content.ContentTypeAlias, true);
    }
    
    public override void Validate(ContentProperties content) {
        var fundDimensionOptions = GetFundDimensionOptions(content);

        if (fundDimensionOptions != null) {
            ValidateDimensionAllowed(content, fundDimensionOptions);
            ValidateFixedDimension(content, fundDimensionOptions);
        }
    }
    
    private void ValidateDimensionAllowed(ContentProperties content, IFundDimensionOptions  fundDimensionOptions) {
        DimensionAllowed(content, fundDimensionOptions.Dimension1, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension1));
        DimensionAllowed(content, fundDimensionOptions.Dimension2, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension2));
        DimensionAllowed(content, fundDimensionOptions.Dimension3, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension3));
        DimensionAllowed(content, fundDimensionOptions.Dimension4, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension4));
    } 

    private void ValidateFixedDimension(ContentProperties content, IFundDimensionOptions  fundDimensionOptions) {
        var fundStructure = _fundStructureAccessor.GetFundStructure();
        
        if (content.GetPropertyValueByAlias<int?>(AllowCrowdfunding) == 1) {
            HasFixedDimension(content, fundDimensionOptions.Dimension1, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension1), fundStructure.Dimension1.IsActive);
            HasFixedDimension(content, fundDimensionOptions.Dimension2, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension2), fundStructure.Dimension2.IsActive);
            HasFixedDimension(content, fundDimensionOptions.Dimension3, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension3), fundStructure.Dimension3.IsActive);
            HasFixedDimension(content, fundDimensionOptions.Dimension4, AliasHelper<OfferingContent>.PropertyAlias(x => x.Dimension4), fundStructure.Dimension4.IsActive);
        }
    }
    
    protected abstract IFundDimensionOptions GetFundDimensionOptions(ContentProperties content);

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