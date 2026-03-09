using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class DonationFormElementValidator : DonationElementValidator<DonationFormElementContent> {
    public DonationFormElementValidator(IContentHelper contentHelper,
                                        IContentLocator contentLocator,
                                        ILookups lookups)
        : base(contentHelper, contentLocator, lookups) { }

    protected override void ValidateProperties(ContentProperties content) { }
}