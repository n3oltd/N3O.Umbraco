using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Lookups;
using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Content;

public class FeedbackDonationOptionValidator : DonationOptionValidator<FeedbackDonationOptionContent> {
    private readonly IContentLocator _contentLocator;
    
    private static readonly string FeedbackSchemeAlias = AliasHelper<FeedbackDonationOptionContent>.PropertyAlias(x => x.Scheme);

    public FeedbackDonationOptionValidator(IContentHelper contentHelper, IContentLocator contentLocator) : base(contentHelper) {
        _contentLocator = contentLocator;
    }

    protected override IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content) {
        return GetFeedbackScheme(content);
    }

    protected override void EnsureNotDuplicate(ContentProperties content) {
        var feedbackScheme = GetFeedbackScheme(content);

        var allOptions = _contentLocator.All<FeedbackDonationOptionContent>().Where(x => x.Content().Key != content.Id);
        
        if (allOptions.Any(x => x.Scheme == feedbackScheme)) {
            ErrorResult("Cannot add duplicate feedback donation option");
        }
    }

    private FeedbackScheme GetFeedbackScheme(ContentProperties content) {
        var feedbackScheme = content.GetPropertyByAlias(FeedbackSchemeAlias)
                                       .IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x)
                                                                    .As<FeedbackScheme>());

        return feedbackScheme;
    }
}
