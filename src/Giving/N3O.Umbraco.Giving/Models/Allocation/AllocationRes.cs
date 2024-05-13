using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class AllocationRes : IAllocation {
    public AllocationType Type { get; set; }
    public MoneyRes Value { get; set; }
    public FundDimensionValuesRes FundDimensions { get; set; }
    public FeedbackAllocationRes Feedback { get; set; }
    public FundAllocationRes Fund { get; set; }
    public SponsorshipAllocationRes Sponsorship { get; set; }
    public Guid? UpsellOfferId { get; set; }
    public bool Upsell { get; set; }
    public IDictionary<string, JToken> Extensions { get; set; }
    
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
}
