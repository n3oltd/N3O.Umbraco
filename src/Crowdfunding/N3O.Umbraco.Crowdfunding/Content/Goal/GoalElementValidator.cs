using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class GoalElementValidator<TGoalElement> : ContentValidator {
    private static readonly string Alias = AliasHelper<TGoalElement>.ContentTypeAlias();
    private const string Dimension1Alias = CrowdfundingConstants.Goal.Properties.FundDimension1;
    private const string Dimension2Alias = CrowdfundingConstants.Goal.Properties.FundDimension2;
    private const string Dimension3Alias = CrowdfundingConstants.Goal.Properties.FundDimension3;
    private const string Dimension4Alias = CrowdfundingConstants.Goal.Properties.FundDimension4;

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
        var value = property.IfNotNull(x => ContentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(x).As<T>());

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }
}