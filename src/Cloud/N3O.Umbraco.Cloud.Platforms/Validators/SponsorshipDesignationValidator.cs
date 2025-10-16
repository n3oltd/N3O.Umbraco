using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class SponsorshipDesignationValidator : DesignationValidator<SponsorshipDesignationContent> {
    private readonly ILookups _lookups;

    public SponsorshipDesignationValidator(IContentHelper contentHelper, ILookups lookups) 
        : base(contentHelper, lookups) {
        _lookups = lookups;
    }
    
    protected override IFundDimensionOptions GetFundDimensionOptions(ContentProperties content) {
        return GetSponsorshipScheme(content).FundDimensionOptions;
    }

    private SponsorshipScheme GetSponsorshipScheme(ContentProperties content) {
        var sponsorshipScheme = content.GetPropertyByAlias(AliasHelper<SponsorshipDesignationContent>.PropertyAlias(x => x.Scheme))
                                       .IfNotNull(x => ContentHelper.GetLookupValue<SponsorshipScheme>(_lookups, x));

        return sponsorshipScheme;
    }
}