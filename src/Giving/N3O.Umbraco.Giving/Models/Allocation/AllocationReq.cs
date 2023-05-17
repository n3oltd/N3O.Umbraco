using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class AllocationReq : IAllocation {
    [Name("Type")]
    public AllocationType Type { get; set; }

    [Name("Value")]
    public MoneyReq Value { get; set; }

    [Name("Fund Dimensions")]
    public FundDimensionValuesReq FundDimensions { get; set; }
    
    [Name("Feedback")]
    public FeedbackAllocationReq Feedback { get; set; }

    [Name("Fund")]
    public FundAllocationReq Fund { get; set; }

    [Name("Sponsorship")]
    public SponsorshipAllocationReq Sponsorship { get; set; }
    
    [Name("Upsell")]
    public bool? Upsell { get; set; }
    
    [JsonIgnore]
    IFundDimensionValues IAllocation.FundDimensions => FundDimensions;
    
    [JsonIgnore]
    IFeedbackAllocation IAllocation.Feedback => Feedback;
    
    [JsonIgnore]
    IFundAllocation IAllocation.Fund => Fund;

    [JsonIgnore]
    ISponsorshipAllocation IAllocation.Sponsorship => Sponsorship; 
    
    [JsonIgnore]
    Money IAllocation.Value => Value;

    [JsonIgnore]
    bool IAllocation.Upsell => Upsell.GetValueOrDefault(false);
}
