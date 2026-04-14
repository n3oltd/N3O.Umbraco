using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public abstract class QurbaniSeasonOfferContent<T> : UmbracoContent<T>, IHoldDonationFormState
    where T : QurbaniSeasonOfferContent<T>{
    public string Name => Content().Name;
    public string Summary => GetValue(x => x.Summary);
    public IEnumerable<QurbaniSeasonCategoryContent> Categories => GetPickedAs(x => x.Categories);
    public DonationItem DonationItem => GetValue(x => x.DonationItem);
    public FundDimension1Value Dimension1 => GetValue(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetValue(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetValue(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetValue(x => x.Dimension4);
    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public MediaWithCrops Image => GetValue(x => x.Image);
    public IHtmlEncodedString Description => GetValue(x => x.Description);
    public bool SoldOut => GetValue(x => x.SoldOut);

    [JsonIgnore]
    public GiftType GiftType => GiftTypes.OneTime;
}