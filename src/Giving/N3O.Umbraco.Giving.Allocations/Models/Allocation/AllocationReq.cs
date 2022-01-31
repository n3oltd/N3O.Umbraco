using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;
using N3O.Umbraco.FundDimensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public class AllocationReq : IAllocation {
        [Name("Type")]
        public AllocationType Type { get; set; }

        [Name("Value")]
        public MoneyReq Value { get; set; }
    
        [Name("Dimension 1")]
        public FundDimension1Option Dimension1 { get; set; }
    
        [Name("Dimension 2")]
        public FundDimension2Option Dimension2 { get; set; }
    
        [Name("Dimension 3")]
        public FundDimension3Option Dimension3 { get; set; }
    
        [Name("Dimension 4")]
        public FundDimension4Option Dimension4 { get; set; }
    
        [Name("Fund")]
        public FundAllocationReq Fund { get; set; }
    
        [Name("Sponsorship")]
        public SponsorshipAllocationReq Sponsorship { get; set; }

        [JsonIgnore]
        IFundAllocation IAllocation.Fund => Fund;
    
        [JsonIgnore]
        ISponsorshipAllocation IAllocation.Sponsorship => Sponsorship;
    
        [JsonIgnore]
        Money IAllocation.Value => Value;
    }
}