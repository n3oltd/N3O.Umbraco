using N3O.Umbraco.Content;

namespace N3O.Umbraco.Elements.Content;

public class GeneralDonationCategoryValidator : DonationCategoryValidator<GeneralDonationCategoryContent> {
    public GeneralDonationCategoryValidator(IContentHelper contentHelper) : base(contentHelper) { }

    protected override void ValidateCategory(ContentProperties content) {
        // No validation required
    }
}