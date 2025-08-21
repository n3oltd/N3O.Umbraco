using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Content;

public abstract class DonationOptionValidator<TDonationOptionContent> : ContentValidator {
    private static readonly string HideDonationAlias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.HideDonation);
    private static readonly string HideRegularGivingAlias = AliasHelper<DonationOptionContent>.PropertyAlias(x => x.HideRegularGiving);
    private readonly ILookups _lookups;

    private static readonly IEnumerable<string> Aliases = new[] {
        AliasHelper<TDonationOptionContent>.ContentTypeAlias(),
    };

    protected DonationOptionValidator(IContentHelper contentHelper, ILookups lookups) : base(contentHelper) {
        _lookups = lookups;
    }

    public override bool IsValidator(ContentProperties content) {
        return Aliases.Contains(content.ContentTypeAlias, true);
    }

    public override void Validate(ContentProperties content) {
        var fundDimensionOptions = GetFundDimensionOptions(content);

        if (fundDimensionOptions != null) {
            DimensionAllowed(content, fundDimensionOptions.Dimension1, AllocationsConstants.Aliases.DonationOption.Properties.Dimension1);
            DimensionAllowed(content, fundDimensionOptions.Dimension2, AllocationsConstants.Aliases.DonationOption.Properties.Dimension2);
            DimensionAllowed(content, fundDimensionOptions.Dimension3, AllocationsConstants.Aliases.DonationOption.Properties.Dimension3);
            DimensionAllowed(content, fundDimensionOptions.Dimension4, AllocationsConstants.Aliases.DonationOption.Properties.Dimension4);
        }

        EnsureNotAllHidden(content);
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

    private void EnsureNotAllHidden(ContentProperties content) {
        var hideDonation = content.GetPropertyValueByAlias<int?>(HideDonationAlias) == 1;
        var hideRegularGiving = content.GetPropertyValueByAlias<int?>(HideRegularGivingAlias) == 1;
        
        if (hideDonation && hideRegularGiving) {
            ErrorResult("Cannot hide both donation and regular giving options");
        }
    }
}
