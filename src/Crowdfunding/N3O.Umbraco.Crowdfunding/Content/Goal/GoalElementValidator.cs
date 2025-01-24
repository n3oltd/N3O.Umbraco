using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class GoalElementValidator<TGoalElement> : ContentValidator {
    private static readonly string Alias = AliasHelper<TGoalElement>.ContentTypeAlias();
    private static readonly string Dimension1Alias = AliasHelper<GoalElement>.PropertyAlias(x => x.FundDimension1);
    private static readonly string Dimension2Alias = AliasHelper<GoalElement>.PropertyAlias(x => x.FundDimension2);
    private static readonly string Dimension3Alias = AliasHelper<GoalElement>.PropertyAlias(x => x.FundDimension3);
    private static readonly string Dimension4Alias = AliasHelper<GoalElement>.PropertyAlias(x => x.FundDimension4);

    protected GoalElementValidator(IContentHelper contentHelper) : base(contentHelper) { }

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
            DimensionAllowed(content, fundDimensionOptions.Dimension1Options, Dimension1Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension2Options, Dimension2Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension3Options, Dimension3Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension4Options, Dimension4Alias);
        }
    }
    
    protected abstract IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content);
    protected abstract void ValidatePriceLocked(ContentProperties content);

    private void DimensionAllowed<T>(ContentProperties content, IEnumerable<T> allowedValues, string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(x).As<T>());

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }
}