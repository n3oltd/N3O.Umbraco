using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Content;

public class SponsorshipDonationOptionValidator : DonationOptionValidator<SponsorshipDonationOptionContent> {
    private static readonly string SponsorshipSchemeAlias = AliasHelper<SponsorshipDonationOptionContent>.PropertyAlias(x => x.Scheme);

    public SponsorshipDonationOptionValidator(IContentHelper contentHelper, IContentLocator contentLocator)
        : base(contentHelper, contentLocator) { }

    protected override void ValidateOption(ContentProperties content) {
        var sponsorshipScheme = GetSponsorshipScheme(content);

        if (!sponsorshipScheme.HasValue()) {
            ErrorResult("Sponsorship scheme is required");
        }
    }
    
    protected override IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content) {
        return GetSponsorshipScheme(content);
    }

    protected override bool IsDuplicate(ContentProperties content, SponsorshipDonationOptionContent other) {
        return GetSponsorshipScheme(content) == other.Scheme;
    }
    
    private SponsorshipScheme GetSponsorshipScheme(ContentProperties content) {
        var sponsorshipScheme = content.GetPropertyByAlias(SponsorshipSchemeAlias)
                                    .IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x)
                                                                 .As<SponsorshipScheme>());

        return sponsorshipScheme;
    }
}