using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public abstract class ApiFundDimensions<T> : ApiLookupsCollection<T> where T : IFundDimension {
    private readonly ICdnClient _cdnClient;

    protected ApiFundDimensions(ICdnClient cdnClient, ILookups lookups) {
        _cdnClient = cdnClient;
        Lookups = lookups;
    }
    
    protected override async Task<IReadOnlyList<T>> FetchAsync(CancellationToken cancellationToken) {
        var fundStructure = await _cdnClient.DownloadSubscriptionContentAsync<PublishedFundStructure>(SubscriptionFiles.FundStructure,
                                                                                                      JsonSerializers.JsonProvider,
                                                                                                      cancellationToken);

        return GetFundDimension(fundStructure).Yield().ToList();
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromMinutes(1);
    protected ILookups Lookups { get; }

    protected abstract T GetFundDimension(PublishedFundStructure fundDimensions);
}


public class ApiFundDimension1 : ApiFundDimensions<FundDimension1> {
    public ApiFundDimension1(ICdnClient cdnClient, ILookups lookups) : base(cdnClient, lookups) { }

    protected override FundDimension1 GetFundDimension(PublishedFundStructure fundStructure) {
        var fundDimension1 = new FundDimension1(fundStructure.Dimension1.Name,
                                                fundStructure.Dimension1.Name,
                                                null,
                                                fundStructure.Dimension1.IsActive,
                                                fundStructure.Dimension1.GetOptions<FundDimension1Value>(Lookups).ToList(),
                                                fundStructure.Dimension1.Index);

        return fundDimension1;
    }
}

public class ApiFundDimension2 : ApiFundDimensions<FundDimension2> {
    public ApiFundDimension2(ICdnClient cdnClient, ILookups lookups) : base(cdnClient, lookups) { }

    protected override FundDimension2 GetFundDimension(PublishedFundStructure fundStructure) {
        var fundDimension2 = new FundDimension2(fundStructure.Dimension2.Name,
                                                fundStructure.Dimension2.Name,
                                                null,
                                                fundStructure.Dimension2.IsActive,
                                                fundStructure.Dimension2.GetOptions<FundDimension2Value>(Lookups).ToList(),
                                                fundStructure.Dimension2.Index);

        return fundDimension2;
    }
}

public class ApiFundDimension3 : ApiFundDimensions<FundDimension3> {
    public ApiFundDimension3(ICdnClient cdnClient, ILookups lookups) : base(cdnClient, lookups) { }

    protected override FundDimension3 GetFundDimension(PublishedFundStructure fundStructure) {
        var fundDimension3 = new FundDimension3(fundStructure.Dimension3.Name,
                                                fundStructure.Dimension3.Name,
                                                null,
                                                fundStructure.Dimension3.IsActive,
                                                fundStructure.Dimension3.GetOptions<FundDimension3Value>(Lookups).ToList(),
                                                fundStructure.Dimension3.Index);

        return fundDimension3;
    }
}

public class ApiFundDimension4 : ApiFundDimensions<FundDimension4> {
    public ApiFundDimension4(ICdnClient cdnClient, ILookups lookups) : base(cdnClient, lookups) { }

    protected override FundDimension4 GetFundDimension(PublishedFundStructure fundStructure) {
        var fundDimension4 = new FundDimension4(fundStructure.Dimension4.Name,
                                                fundStructure.Dimension4.Name,
                                                null,
                                                fundStructure.Dimension4.IsActive,
                                                fundStructure.Dimension4.GetOptions<FundDimension4Value>(Lookups).ToList(),
                                                fundStructure.Dimension4.Index);

        return fundDimension4;
    }
}