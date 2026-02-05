using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class DonationPopupElementValidator : DonationElementValidator<DonationPopupElementContent> {
    private static readonly string TimeDelaySecondsAlias = AliasHelper<DonationPopupElementContent>.PropertyAlias(x => x.TimeDelaySeconds);
    
    public DonationPopupElementValidator(IContentHelper contentHelper,
                                         IContentLocator contentLocator,
                                         ILookups lookups)
        : base(contentHelper, contentLocator, lookups) { }

    protected override void ValidateProperties(ContentProperties content) {
        var timeDelaySeconds = content.GetPropertyValueByAlias<int?>(TimeDelaySecondsAlias);

        if (timeDelaySeconds < 1 || timeDelaySeconds > 300) {
            ErrorResult("Time delay must be between 1 and 300 seconds");
        }
    }
}