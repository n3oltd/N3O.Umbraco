using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Content;

public class UpsellContentValidator<TUpsellContent> : ContentValidator {
    private static readonly string Dimension1Alias = AliasHelper<UpsellContent>.PropertyAlias(x => x.Dimension1);
    private static readonly string Dimension2Alias = AliasHelper<UpsellContent>.PropertyAlias(x => x.Dimension2);
    private static readonly string Dimension3Alias = AliasHelper<UpsellContent>.PropertyAlias(x => x.Dimension3);
    private static readonly string Dimension4Alias = AliasHelper<UpsellContent>.PropertyAlias(x => x.Dimension4);
    private static readonly string DonationItemAlias = AliasHelper<UpsellContent>.PropertyAlias(x => x.DonationItem);

    public UpsellContentValidator(IContentHelper contentHelper) : base(contentHelper) { }

    private static readonly IEnumerable<string> Aliases = new[] {
        AliasHelper<TUpsellContent>.ContentTypeAlias(),
    };
    
    public override bool IsValidator(ContentProperties content) {
        return Aliases.Contains(content.ContentTypeAlias, true);
    }

    public override void Validate(ContentProperties content) {
        var donationItem = GetDonationItem(content);

        if (donationItem != null) {
            DimensionAllowed(content, donationItem.Dimension1Options, Dimension1Alias);
            DimensionAllowed(content, donationItem.Dimension2Options, Dimension2Alias);
            DimensionAllowed(content, donationItem.Dimension3Options, Dimension3Alias);
            DimensionAllowed(content, donationItem.Dimension4Options, Dimension4Alias);
        }
    }

    private DonationItem GetDonationItem(ContentProperties content) {
        var donationItem = content.GetPropertyByAlias(DonationItemAlias)
                                  .IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x)
                                                               .As<DonationItem>());

        return donationItem;
    }

    private void DimensionAllowed<T>(ContentProperties content,
                                     IEnumerable<T> allowedValues,
                                     string propertyAlias)
        where T : FundDimensionValue<T> {
        var property = content.GetPropertyByAlias(propertyAlias);
        var value = property.IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x).As<T>());

        if (value != null && allowedValues != null && !allowedValues.Contains(value)) {
            ErrorResult(property, $"{value.Name} is not a permitted fund dimension value");
        }
    }
}