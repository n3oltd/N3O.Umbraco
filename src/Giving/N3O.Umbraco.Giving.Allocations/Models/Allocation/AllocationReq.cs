using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

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
    
    [Name("Pledge URL")]
    public string PledgeUrl { get; set; }
    
    [Name("Upsell Offer ID")]
    public Guid? UpsellOfferId { get; set; }

    [JsonIgnore]
    public bool LinkedToPledge => PledgeUrl.HasValue();

    [JsonExtensionData]
    public IDictionary<string, JToken> Extensions { get; set; }
    
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