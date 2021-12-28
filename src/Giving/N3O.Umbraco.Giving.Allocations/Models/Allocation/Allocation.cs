using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public class Allocation : Value, IAllocation {
        [JsonConstructor]
        public Allocation(AllocationType type,
                          Money value,
                          FundDimension1Option dimension1,
                          FundDimension2Option dimension2,
                          FundDimension3Option dimension3,
                          FundDimension4Option dimension4,
                          FundAllocation fund,
                          SponsorshipAllocation sponsorship) {
            Type = type;
            Value = value;
            Dimension1 = dimension1;
            Dimension2 = dimension2;
            Dimension3 = dimension3;
            Dimension4 = dimension4;
            Fund = fund;
            Sponsorship = sponsorship;
        }

        public Allocation(IAllocation allocation)
            : this(allocation.Type,
                   allocation.Value,
                   allocation.Dimension1,
                   allocation.Dimension2,
                   allocation.Dimension3,
                   allocation.Dimension4,
                   allocation.Fund.IfNotNull(x => new FundAllocation(x)),
                   allocation.Sponsorship.IfNotNull(x => new SponsorshipAllocation(x))) { }

        public AllocationType Type { get; }
        public Money Value { get; }
        public FundDimension1Option Dimension1 { get; }
        public FundDimension2Option Dimension2 { get; }
        public FundDimension3Option Dimension3 { get; }
        public FundDimension4Option Dimension4 { get; }
        public FundAllocation Fund { get; }
        public SponsorshipAllocation Sponsorship { get; }

        [JsonIgnore]
        IFundAllocation IAllocation.Fund => Fund;
    
        [JsonIgnore]
        ISponsorshipAllocation IAllocation.Sponsorship => Sponsorship;
    
        public string Summary => Fund?.Summary ?? Sponsorship?.Summary;
    }
}