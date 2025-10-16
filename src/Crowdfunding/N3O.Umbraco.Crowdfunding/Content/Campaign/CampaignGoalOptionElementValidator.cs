using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class CampaignGoalOptionElementValidator<TGoalOption> : ContentValidator {
    private readonly ILookups _lookups;
    private static readonly string Alias = AliasHelper<TGoalOption>.ContentTypeAlias();
    private static readonly string Dimension1Alias = AliasHelper<CampaignGoalOptionElement>.PropertyAlias(x => x.FundDimension1);
    private static readonly string Dimension2Alias = AliasHelper<CampaignGoalOptionElement>.PropertyAlias(x => x.FundDimension2);
    private static readonly string Dimension3Alias = AliasHelper<CampaignGoalOptionElement>.PropertyAlias(x => x.FundDimension3);
    private static readonly string Dimension4Alias = AliasHelper<CampaignGoalOptionElement>.PropertyAlias(x => x.FundDimension4);
    
    public CampaignGoalOptionElementValidator(IContentHelper contentHelper, ILookups lookups) : base(contentHelper) {
        _lookups = lookups;
    }
    
    public override bool IsValidator(ContentProperties content) {
        return Alias.EqualsInvariant(content.ContentTypeAlias);
    }
    
    public override void Validate(ContentProperties content) {
        ValidateFundDimensions(content);
        ValidatePriceLocked(content);
    }

    private void ValidateFundDimensions(ContentProperties content) {
        var fundDimensionOptions = GetFundDimensionOptions(content);

        if (fundDimensionOptions != null) {
            DimensionAllowed(content, fundDimensionOptions.Dimension1, Dimension1Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension2, Dimension2Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension3, Dimension3Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension4, Dimension4Alias);
        }
    }

    protected abstract IFundDimensionOptions GetFundDimensionOptions(ContentProperties content);
    protected abstract void ValidatePriceLocked(ContentProperties content);

    private void DimensionAllowed<T>(ContentProperties content, IEnumerable<T> allowedValues, string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var fundDimensionValues = property.IfNotNull(x => ContentHelper.GetLookupValues<T>(_lookups, x));

        foreach (var fundDimensionValue in fundDimensionValues.OrEmpty()) {
            if (fundDimensionValue != null && allowedValues != null && !allowedValues.Contains(fundDimensionValue)) {
                ErrorResult(property, $"{fundDimensionValue.Name} is not a permitted fund dimension value");
            }
        }
    }
}