using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Qurbani.Settings.Season.Upsells.Upsell.Alias)]
public class QurbaniSeasonUpsellContent : UmbracoContent<QurbaniSeasonUpsellContent>, IHoldDonationFormState {
    public string Name => Content().Name;
    public Guid Key => Content().Key;

    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public string Summary => GetValue(x => x.Summary);
    public GiftType GiftType => GetValue(x => x.GiftType);
    public DonationItem DonationItem => GetValue(x => x.DonationItem);
    public FundDimension1Value Dimension1 => GetValue(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetValue(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetValue(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetValue(x => x.Dimension4);
    public decimal Amount => GetValue(x => x.Amount);
    public string Notes => GetValue(x => x.Notes);
}
