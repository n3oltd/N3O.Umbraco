using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class CampaignGoalOptionElementValidator<TGoalOption> : ContentValidator {
    private static readonly string Alias = AliasHelper<TGoalOption>.ContentTypeAlias();
    private const string Dimension1Alias = CrowdfundingConstants.CampaignGoalOption.Properties.FundDimension1;
    private const string Dimension2Alias = CrowdfundingConstants.CampaignGoalOption.Properties.FundDimension2;
    private const string Dimension3Alias = CrowdfundingConstants.CampaignGoalOption.Properties.FundDimension3;
    private const string Dimension4Alias = CrowdfundingConstants.CampaignGoalOption.Properties.FundDimension4;

    public CampaignGoalOptionElementValidator(IContentHelper contentHelper) : base(contentHelper) { }
    
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
        var fundDimensionValues = property.IfNotNull(x => ContentHelper.GetMultiNodeTreePickerValues<IPublishedContent>(x).As<T>());

        foreach (var fundDimensionValue in fundDimensionValues.OrEmpty()) {
            if (fundDimensionValue != null && allowedValues != null && !allowedValues.Contains(fundDimensionValue)) {
                ErrorResult(property, $"{fundDimensionValue.Name} is not a permitted fund dimension value");
            }
        }
    }
}