using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Content;

public class FeedbackDonationOptionValidator : DonationOptionValidator<FeedbackDonationOptionContent> {
    private static readonly string FeedbackSchemeAlias = AliasHelper<FeedbackDonationOptionContent>.PropertyAlias(x => x.Scheme);

    public FeedbackDonationOptionValidator(IContentHelper contentHelper, IContentLocator contentLocator)
        : base(contentHelper, contentLocator) { }

    protected override void ValidateOption(ContentProperties content) {
        var feedbackScheme = GetFeedbackScheme(content);

        if (!feedbackScheme.HasValue()) {
            ErrorResult("Feedback scheme is required");
        }
    }
    
    protected override IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content) {
        return GetFeedbackScheme(content);
    }

    protected override bool IsDuplicate(ContentProperties content, FeedbackDonationOptionContent other) {
        return GetFeedbackScheme(content) == other.Scheme;
    }
    
    private FeedbackScheme GetFeedbackScheme(ContentProperties content) {
        var feedbackScheme = content.GetPropertyByAlias(FeedbackSchemeAlias)
                                    .IfNotNull(x => ContentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(x)
                                                                 .As<FeedbackScheme>());

        return feedbackScheme;
    }
}
