using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models;

public class AllocationRes : IAllocation {
    public AllocationType Type { get; set; }
    public MoneyRes Value { get; set; }
    public FundDimensionValuesRes FundDimensions { get; set; }
    public FundAllocationRes Fund { get; set; }
    public SponsorshipAllocationRes Sponsorship { get; set; }
    public FeedbackAllocationRes Feedback { get; set; }
    public bool Upsell { get; set; }
    
    [JsonIgnore]
    IFundDimensionValues IAllocation.FundDimensions => FundDimensions;
    
    [JsonIgnore]
    IFundAllocation IAllocation.Fund => Fund;

    [JsonIgnore]
    ISponsorshipAllocation IAllocation.Sponsorship => Sponsorship;
    
    [JsonIgnore]
    IFeedbackAllocation IAllocation.Feedback => Feedback;

    [JsonIgnore]
    Money IAllocation.Value => Value;
}
