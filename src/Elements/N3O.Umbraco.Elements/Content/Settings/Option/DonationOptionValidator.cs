using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Content;

public abstract class DonationOptionValidator<TDonationOptionContent> : ContentValidator {
    private static readonly string Alias = AliasHelper<TDonationOptionContent>.ContentTypeAlias();
    private static readonly string Dimension1Alias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.Dimension1);
    private static readonly string Dimension2Alias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.Dimension2);
    private static readonly string Dimension3Alias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.Dimension3);
    private static readonly string Dimension4Alias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.Dimension4);
    private static readonly string HideDonationAlias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.HideDonation);
    private static readonly string HideRegularGivingAlias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.HideRegularGiving);
    
    private readonly IContentLocator _contentLocator;

    protected DonationOptionValidator(IContentHelper contentHelper, IContentLocator contentLocator)
        : base(contentHelper) {
        _contentLocator = contentLocator;
    }

    public override bool IsValidator(ContentProperties content) {
        return Alias.EqualsInvariant(content.ContentTypeAlias);
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
        EnsureNotDuplicate(content);

        ValidateOption(content);
    }
    
    protected abstract IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content);
    protected abstract bool IsDuplicate(ContentProperties content, TDonationOptionContent other);
    protected abstract void ValidateOption(ContentProperties content);

    private void DimensionAllowed<T>(ContentProperties content, IEnumerable<T> allowedValues, string propertyAlias)
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
    
    private void EnsureNotDuplicate(ContentProperties content) {
        var existingOption = _contentLocator.All(Alias)
                                            .FirstOrDefault(x => x.Key != content.Id &&
                                                                 IsDuplicate(content, x.As<TDonationOptionContent>()));
        
        if (existingOption.HasValue()) {
            ErrorResult($"Cannot add this option as it is a duplicate of {existingOption.Name} ({existingOption.Id})");
        }
    }
}
