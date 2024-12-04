using N3O.Umbraco.Content;

namespace N3O.Umbraco.Elements.Content;

public class GeneralDonationCategoryValidator : DonationCategoryValidator<GeneralDonationCategoryContent> {
    public GeneralDonationCategoryValidator(IContentHelper contentHelper, IContentLocator contentLocator) 
        : base(contentHelper, contentLocator) { }
}