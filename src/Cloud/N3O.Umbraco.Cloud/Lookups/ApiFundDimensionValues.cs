using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
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
public abstract class ApiFundDimensionValues<T> : ApiLookupsCollection<T> where T : IFundDimensionValue {
    private readonly ICdnClient _cdnClient;

    protected ApiFundDimensionValues(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<T>> FetchAsync(CancellationToken cancellationToken) {
        var fundStructure = await _cdnClient.DownloadSubscriptionContentAsync<PublishedFundStructure>(SubscriptionFiles.FundStructure,
                                                                                                      JsonSerializers.JsonProvider,
                                                                                                      cancellationToken);

        return GetFundDimensionValues(fundStructure).ToList();
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromMinutes(5);
    
    protected abstract IEnumerable<T> GetFundDimensionValues(PublishedFundStructure fundDimensionValues);
}


public class ApiFundDimension1Values : ApiFundDimensionValues<FundDimension1Value> {
    public ApiFundDimension1Values(ICdnClient cdnClient) : base(cdnClient) { }

    protected override IEnumerable<FundDimension1Value> GetFundDimensionValues(PublishedFundStructure fundStructure) {
        FundDimension1Value ToFundDimension1Value(string value, bool unrestricted) {
            return new FundDimension1Value(value, value, null, unrestricted);
        }
        
        var fundDimensionValues = fundStructure.Dimension1
                                               .RestrictedOptions
                                               .Select(x => ToFundDimension1Value(x, false))
                                               .ToList();

        fundStructure.Dimension1.UnrestrictedOption.IfNotNull(x => fundDimensionValues.Add(ToFundDimension1Value(x, true)));

        return fundDimensionValues;
    }
}

public class ApiFundDimension2Values : ApiFundDimensionValues<FundDimension2Value> {
    public ApiFundDimension2Values(ICdnClient cdnClient) : base(cdnClient) { }

    protected override IEnumerable<FundDimension2Value> GetFundDimensionValues(PublishedFundStructure fundStructure) {
        FundDimension2Value ToFundDimension2Value(string value, bool unrestricted) {
            return new FundDimension2Value(value, value, null, unrestricted);
        }
        
        var fundDimensionValues = fundStructure.Dimension2
                                               .RestrictedOptions
                                               .Select(x => ToFundDimension2Value(x, false))
                                               .ToList();

        fundStructure.Dimension2.UnrestrictedOption.IfNotNull(x => fundDimensionValues.Add(ToFundDimension2Value(x, true)));

        return fundDimensionValues;
    }
}

public class ApiFundDimension3Values : ApiFundDimensionValues<FundDimension3Value> {
    public ApiFundDimension3Values(ICdnClient cdnClient) : base(cdnClient) { }

    protected override IEnumerable<FundDimension3Value> GetFundDimensionValues(PublishedFundStructure fundStructure) {
        FundDimension3Value ToFundDimension3Value(string value, bool unrestricted) {
            return new FundDimension3Value(value, value, null, unrestricted);
        }
        
        var fundDimensionValues = fundStructure.Dimension3
                                               .RestrictedOptions
                                               .Select(x => ToFundDimension3Value(x, false))
                                               .ToList();

        fundStructure.Dimension3.UnrestrictedOption.IfNotNull(x => fundDimensionValues.Add(ToFundDimension3Value(x, true)));

        return fundDimensionValues;
    }
}

public class ApiFundDimension4Values : ApiFundDimensionValues<FundDimension4Value> {
    public ApiFundDimension4Values(ICdnClient cdnClient) : base(cdnClient) { }

    protected override IEnumerable<FundDimension4Value> GetFundDimensionValues(PublishedFundStructure fundStructure) {
        FundDimension4Value ToFundDimension4Value(string value, bool unrestricted) {
            return new FundDimension4Value(value, value, null, unrestricted);
        }
        
        var fundDimensionValues = fundStructure.Dimension4
                                               .RestrictedOptions
                                               .Select(x => ToFundDimension4Value(x, false))
                                               .ToList();

        fundStructure.Dimension4.UnrestrictedOption.IfNotNull(x => fundDimensionValues.Add(ToFundDimension4Value(x, true)));

        return fundDimensionValues;
    }
}