using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class QurbaniDonationFormStateValidator : DonationFormStateValidator<QurbaniDonationFormStateContent> {
    private readonly ILookups _lookups;

    public QurbaniDonationFormStateValidator(IContentHelper contentHelper,
                                             ILookups lookups,
                                             IFundStructureAccessor fundStructureAccessor)
        : base(contentHelper, lookups, fundStructureAccessor) {
        _lookups = lookups;
    }

    public override void Validate(ContentProperties content) {
        var qurbaniItem = GetQurbaniItem(content);

        if (qurbaniItem == null) {
            var property = content.GetPropertyByAlias(AliasHelper<QurbaniDonationFormStateContent>.PropertyAlias(x => x.QurbaniItem));
            ErrorResult(property, "is required");
        }
    }

    protected override IFundDimensionOptions GetFundDimensionOptions(ContentProperties content) {
        return null;
    }

    private QurbaniItem GetQurbaniItem(ContentProperties content) {
        return content.GetPropertyByAlias(AliasHelper<QurbaniDonationFormStateContent>.PropertyAlias(x => x.QurbaniItem))
                      .IfNotNull(x => ContentHelper.GetLookupValue<QurbaniItem>(_lookups, x));
    }
}
