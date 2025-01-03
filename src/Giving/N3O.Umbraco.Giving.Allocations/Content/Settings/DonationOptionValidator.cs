using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Allocations.Content;

public abstract class DonationOptionValidator<TDonationOptionContent> : ContentValidator {
    private static readonly string Dimension1Alias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.Dimension1);
    private static readonly string Dimension2Alias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.Dimension2);
    private static readonly string Dimension3Alias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.Dimension3);
    private static readonly string Dimension4Alias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.Dimension4);
    private static readonly string HideDonationAlias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.HideDonation);
    private static readonly string HideRegularGivingAlias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.HideRegularGiving);

    private static readonly IEnumerable<string> Aliases = new[] {
        AliasHelper<TDonationOptionContent>.ContentTypeAlias(),
    };

    protected DonationOptionValidator(IContentHelper contentHelper) : base(contentHelper) { }

    public override bool IsValidator(ContentProperties content) {
        return Aliases.Contains(content.ContentTypeAlias, true);
    }

    public override void Validate(ContentProperties content) {
        var fundDimensionOptions = GetFundDimensionOptions(content);

        if (fundDimensionOptions != null) {
            DimensionAllowed(content, fundDimensionOptions.Dimension1Options, Dimension1Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension2Options, Dimension2Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension3Options, Dimension3Alias);
            DimensionAllowed(content, fundDimensionOptions.Dimension4Options, Dimension4Alias);
        }

        EnsureNotAllHidden(content);
    }

    protected abstract IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content);

    private void DimensionAllowed<T>(ContentProperties content,
                                     IEnumerable<T> allowedValues,
                                     string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(x).As<T>());

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }

    private void EnsureNotAllHidden(ContentProperties content) {
        var hideDonation = content.GetPropertyValueByAlias<int?>(HideDonationAlias) == 1;
        var hideRegularGiving = content.GetPropertyValueByAlias<int?>(HideRegularGivingAlias) == 1;
        
        if (hideDonation && hideRegularGiving) {
            ErrorResult("Cannot hide both donation and regular giving options");
        }
    }
}
