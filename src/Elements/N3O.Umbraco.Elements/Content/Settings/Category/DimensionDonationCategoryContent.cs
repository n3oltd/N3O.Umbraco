using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Content;

[UmbracoContent(ElementsConstants.DonationCategory.Dimension.Alias)]
public class DimensionDonationCategoryContent : UmbracoContent<DimensionDonationCategoryContent> {
    public IPublishedContent Dimension => GetPickedAs(x => x.Dimension);

    public int? FundDimension => Dimension.ContentType.Alias[^1..].TryParseAs<int>();
}