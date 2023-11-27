using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

/*
 * We would need to make this more dynamic (similar to AccountReq in K2.Accounts where we can
 * pass in additional properties on the frontend, e.g.
 * {
 *      crowdFunding: {
 *          comment: ...
 *          etc.
 *      }
 * }
 *
 * these then can be stored in the JSON extension data or similar to at least ensure the data
 * is available at runtime to us
 */

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
    
    [Name("Upsell Offer ID")]
    public Guid? UpsellOfferId { get; set; }
    
    [JsonExtensionData]
    public IDictionary<string, JToken> JsonData { get; set; }
    
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
