using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class Allocation : Value, IAllocation {
    [JsonConstructor]
    public Allocation(AllocationType type,
                      Money value,
                      FundDimensionValues fundDimensions,
                      FundAllocation fund,
                      SponsorshipAllocation sponsorship,
                      bool upsell) {
        Type = type;
        Value = value;
        FundDimensions = fundDimensions;
        Fund = fund;
        Sponsorship = sponsorship;
        Upsell = upsell;
    }

    public Allocation(IAllocation allocation)
        : this(allocation.Type,
               allocation.Value,
               allocation.FundDimensions.IfNotNull(x => new FundDimensionValues(x)),
               allocation.Fund.IfNotNull(x => new FundAllocation(x)),
               allocation.Sponsorship.IfNotNull(x => new SponsorshipAllocation(x)),
               allocation.Upsell) { }

    public AllocationType Type { get; }
    public Money Value { get; }
    public FundDimensionValues FundDimensions { get; }
    public FundAllocation Fund { get; }
    public SponsorshipAllocation Sponsorship { get; }
    public bool Upsell { get; }

    [JsonIgnore]
    IFundDimensionValues IAllocation.FundDimensions => FundDimensions;
    
    [JsonIgnore]
    IFundAllocation IAllocation.Fund => Fund;

    [JsonIgnore]
    ISponsorshipAllocation IAllocation.Sponsorship => Sponsorship;

    public string Summary => Fund?.Summary ?? Sponsorship?.Summary;
}
