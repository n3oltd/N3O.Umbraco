using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Elements.Content;

public class DimensionDonationCategoryValidator : DonationCategoryValidator<DimensionDonationCategoryContent> {
    public DimensionDonationCategoryValidator(IContentHelper contentHelper) : base(contentHelper) { }

    protected override void ValidateCategory(ContentProperties content) {
        if (content.Properties.SingleOrDefault(x => x.Alias.EqualsInvariant(ElementsConstants.DonationCategory.Dimension.Properties.FundDimension))?.Value.HasValue() != true) {
            ErrorResult("Fund dimension must be specified");
        }
    }
}