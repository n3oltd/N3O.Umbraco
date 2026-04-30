using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class FeedbackDonationFormStateValidator : DonationFormStateValidator<FeedbackDonationFormStateContent> {
    private readonly ILookups _lookups;

    public FeedbackDonationFormStateValidator(IContentHelper contentHelper,
                                              ILookups lookups,
                                              IFundStructureAccessor fundStructureAccessor)
        : base(contentHelper, lookups, fundStructureAccessor) {
        _lookups = lookups;
    }

    protected override IFundDimensionOptions GetFundDimensionOptions(ContentProperties content) {
        return GetFeedbackScheme(content)?.FundDimensionOptions;
    }

    private FeedbackScheme GetFeedbackScheme(ContentProperties content) {
        return content.GetPropertyByAlias(AliasHelper<FeedbackDonationFormStateContent>.PropertyAlias(x => x.Scheme))
                      .IfNotNull(x => ContentHelper.GetLookupValue<FeedbackScheme>(_lookups, x));
    }
}
