using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Lookups;
using N3O.Umbraco.Elements.Models;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Elements.Content;

public class SponsorshipDonationOptionValidator : DonationOptionValidator<SponsorshipDonationOptionContent> {
    private readonly IContentLocator _contentLocator;
    
    private static readonly string SponsorshipSchemeAlias = AliasHelper<SponsorshipDonationOptionContent>.PropertyAlias(x => x.Scheme);

    public SponsorshipDonationOptionValidator(IContentHelper contentHelper, IContentLocator contentLocator) : base(contentHelper) {
        _contentLocator = contentLocator;
    }

    protected override IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content) {
        return GetSponsorshipScheme(content);
    }

    protected override void EnsureNotDuplicate(ContentProperties content) {
        var sponsorshipScheme = GetSponsorshipScheme(content);

        var allOptions = _contentLocator.All<SponsorshipDonationOptionContent>().Where(x => x.Content().Key != content.Id);
        
        if (allOptions.Any(x => x.Scheme == sponsorshipScheme)) {
            ErrorResult("Cannot add duplicate sponsorship donation option");
        }
    }

    private SponsorshipScheme GetSponsorshipScheme(ContentProperties content) {
        var sponsorshipScheme = content.GetPropertyByAlias(SponsorshipSchemeAlias)
                                       .IfNotNull(x => ContentHelper.GetPickerValue<IPublishedContent>(x)
                                                                    .As<SponsorshipScheme>());

        return sponsorshipScheme;
    }
}
