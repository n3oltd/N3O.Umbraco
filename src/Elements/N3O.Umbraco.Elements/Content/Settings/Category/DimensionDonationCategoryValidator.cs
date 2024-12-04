using N3O.Umbraco.Content;

namespace N3O.Umbraco.Elements.Content;

public class DimensionDonationCategoryValidator : DonationCategoryValidator<DimensionDonationCategoryContent> {
    public DimensionDonationCategoryValidator(IContentHelper contentHelper, IContentLocator contentLocator) 
        : base(contentHelper, contentLocator) { }
}