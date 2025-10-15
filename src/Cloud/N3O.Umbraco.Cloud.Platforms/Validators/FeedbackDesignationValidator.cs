using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class FeedbackDesignationValidator : DesignationValidator<FeedbackDesignationContent> {
    private readonly ILookups _lookups;

    public FeedbackDesignationValidator(IContentHelper contentHelper, ILookups lookups) 
        : base(contentHelper, lookups) {
        _lookups = lookups;
    }
    
    protected override IFundDimensionOptions GetFundDimensionOptions(ContentProperties content) {
        return GetFeedbackScheme(content).FundDimensionOptions;
    }

    private FeedbackScheme GetFeedbackScheme(ContentProperties content) {
        var feedbackScheme = content.GetPropertyByAlias(AliasHelper<FeedbackDesignationContent>.PropertyAlias(x => x.Scheme))
                                    .IfNotNull(x => ContentHelper.GetLookupValue<FeedbackScheme>(_lookups, x));

        return feedbackScheme;
    }
}